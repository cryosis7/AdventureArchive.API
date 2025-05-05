using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Domain.Entities;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Extensions;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Domain.ValueObjects;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureArchive.Api.Infrastructure.Repositories;

public class TrackRepository : ITrackProvider
{
    private readonly IDocService _docService;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);

    public TrackRepository(IDocService docService, IMemoryCache cache)
    {
        _docService = docService ?? throw new ArgumentNullException(nameof(docService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<IEnumerable<Track>> GetAllAsync(RegionEnum? regionCode)
    {
        var cacheKey = $"Tracks_{regionCode}";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Track>? cachedTracks) && cachedTracks != null)
        {
            return cachedTracks;
        }

        Log.Information("Cache miss for tracks CacheKey: {CacheKey} - Fetching from DocApi", cacheKey);
        var tracksResponse = await _docService.GetTracksAsync(regionCode.ToString());
        var tracks = tracksResponse.Select(dto =>
            {
                try
                {
                    var track = MapToTrack(dto);
                    track.Validate();
                    return track;
                }
                catch (ValidationException ex)
                {
                    Log.Error(ex, "Validation error for track with AssetId {AssetId}: {Message}", dto.AssetId,
                        ex.Message);
                    return null;
                }
            })
            .Where(track => track != null)
            .Select(track => track!)
            .ToList();

        _cache.Set(cacheKey, tracks, CacheDuration);
        return tracks;
    }
    
    private static Track MapToTrack(TrackDto dto)
    {
        return new Track
        {
            AssetId = dto.AssetId,
            Name = dto.Name,
            RegionList = dto.RegionList
                ?.Select(regionString => regionString.ToRegionEnum())
                .Where(region => region.HasValue)
                .Select(region => region!.Value),
            Location = Location.CreateLocation(dto.Lat, dto.Lon),
            Line = new MultiLineString(dto.Line)
        };
    }
}