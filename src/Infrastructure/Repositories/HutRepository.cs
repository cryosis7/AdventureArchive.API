using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Factories;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AdventureArchive.Infrastructure.Repositories;

public class HutRepository(
    IDocService docService,
    IMemoryCache cache,
    ILandmarkFactory factory,
    ILogger<HutRepository> logger)
    : IHutRepository
{
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
        if (cache.TryGetValue(cacheKey, out IEnumerable<ILandmark>? cachedHuts) && cachedHuts != null)
        {
            return cachedHuts;
        }

        logger.LogInformation("Cache miss for huts CacheKey: {CacheKey} - Fetching from DocApi", cacheKey);
        var hutsResponse = await docService.GetHutsAsync(regionCode?.ToString());
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
                    logger.LogError(ex, "Argument Validation error for hut with AssetId {AssetId}: {Message}", hutDto.AssetId,
                        ex.Message);
                    return null;
                }
            })
            .Where(hut => hut != null)
            .Select(hut => hut!)
            .ToList();

        cache.Set(cacheKey, huts, CacheDuration);
        return huts;
    }

    public ILandmark GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}