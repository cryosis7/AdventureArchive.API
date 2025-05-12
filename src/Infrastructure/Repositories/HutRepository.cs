using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Factories;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureArchive.Api.Infrastructure.Repositories;

public class HutRepository(IDocService docService, IMemoryCache cache, ILandmarkFactory factory) : IHutRepository
{
    private readonly IDocService _docService = docService ?? throw new ArgumentNullException(nameof(docService));
    private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);
    private const string CacheKeyPrefix = "Huts";

    public async Task<IEnumerable<ILandmark>> GetByRegion(RegionEnum regionCode)
    {
        return await GetHutsInternalAsync(regionCode);
    }

    public async Task<IEnumerable<ILandmark>> GetAllAsync()
    {
        return await GetHutsInternalAsync(null);
    }

    private async Task<IEnumerable<ILandmark>> GetHutsInternalAsync(RegionEnum? regionCode)
    {
        var cacheKey = regionCode.HasValue ? $"{CacheKeyPrefix}_{regionCode.Value}" : $"{CacheKeyPrefix}_All";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<ILandmark>? cachedHuts) && cachedHuts != null)
        {
            return cachedHuts;
        }

        Log.Information("Cache miss for huts CacheKey: {CacheKey} - Fetching from DocApi", cacheKey);
        var hutsResponse = await _docService.GetHutsAsync(regionCode?.ToString());
        var huts = hutsResponse
            .Select(hutDto =>
            {
                try
                {
                    var parameters = new LandmarkCreationParameters
                    {
                        DocId = hutDto.AssetId.ToString(),
                        Name = hutDto.Name,
                        Latitude = hutDto.Lat,
                        Longitude = hutDto.Lon,
                        LandmarkType = LandmarkType.Hut
                    };
                    return factory.CreateLandmark(parameters);
                }
                catch (ArgumentException ex)
                {
                    Log.Error(ex, "Argument Validation error for hut with AssetId {AssetId}: {Message}", hutDto.AssetId,
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

    public ILandmark GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}