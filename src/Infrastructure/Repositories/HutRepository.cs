using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Domain.Entities;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Extensions;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Domain.ValueObjects;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureArchive.Api.Infrastructure.Repositories;

public class HutRepository : IHutProvider
{
    private readonly IDocService _docService;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);

    public HutRepository(IDocService docService, IMemoryCache cache)
    {
        _docService = docService ?? throw new ArgumentNullException(nameof(docService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<IEnumerable<Hut>> GetAllAsync(RegionEnum? regionCode)
    {
        var cacheKey = $"Huts_{regionCode}";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Hut>? cachedHuts) && cachedHuts != null)
        {
            return cachedHuts;
        }

        Log.Information("Cache miss for huts CacheKey: {CacheKey} - Fetching from DocApi", cacheKey);
        var hutsResponse = await _docService.GetHutsAsync(regionCode.ToString());
        var huts = hutsResponse
            .Select(hutDto =>
            {
                try
                {
                    var hut = new Hut
                    {
                        AssetId = hutDto.AssetId,
                        Name = hutDto.Name,
                        Status = Enum.Parse<StatusEnum>(hutDto.Status),
                        Region = hutDto.Region.ToRegionEnum(),
                        Location = Location.CreateLocation(hutDto.Lat, hutDto.Lon)
                    };
                    hut.Validate();
                    return hut;
                }
                catch (ValidationException ex)
                {
                    Log.Error(ex, "Validation error for hut with AssetId {AssetId}: {Message}", hutDto.AssetId,
                        ex.Message);
                    return null;
                }
            })
            .Where(hut => hut != null)
            .Select(hut => hut!)
            .ToList();

        _cache.Set(cacheKey, huts, CacheDuration);
        return huts;
    }
}