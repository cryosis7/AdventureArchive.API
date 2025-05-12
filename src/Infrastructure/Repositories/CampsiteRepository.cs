using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Factories;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AdventureArchive.Infrastructure.Repositories;

public class CampsiteRepository(
    IDocService docService,
    IMemoryCache cache,
    ILandmarkFactory factory,
    ILogger<CampsiteRepository> logger)
    : ICampsiteRepository
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);
    private const string CacheKeyPrefix = "Campsites";

    public async Task<IEnumerable<ILandmark>> GetByRegion(RegionEnum regionCode)
    {
        return await GetCampsitesInternalAsync(regionCode);
    }

    public async Task<IEnumerable<ILandmark>> GetAllAsync()
    {
        return await GetCampsitesInternalAsync(null);
    }

    private async Task<IEnumerable<ILandmark>> GetCampsitesInternalAsync(RegionEnum? regionCode)
    {
        var cacheKey = regionCode.HasValue ? $"{CacheKeyPrefix}_{regionCode.Value}" : $"{CacheKeyPrefix}_All";
        if (cache.TryGetValue(cacheKey, out IEnumerable<ILandmark>? cachedCampsites) && cachedCampsites != null)
        {
            return cachedCampsites;
        }

        logger.LogInformation("Cache miss for campsites CacheKey: {CacheKey} - Fetching from DocApi", cacheKey);
        var campsitesResponse = await docService.GetCampsitesAsync(regionCode?.ToString());
        var campsites = campsitesResponse
            .Select(campsiteDto =>
            {
                try
                {
                    var parameters = new LandmarkCreationParameters
                    {
                        DocId = campsiteDto.AssetId.ToString(),
                        Name = campsiteDto.Name,
                        Latitude = campsiteDto.Lat,
                        Longitude = campsiteDto.Lon,
                        LandmarkType = LandmarkType.Campsite
                    };
                    return factory.CreateLandmark(parameters);
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex, "Argument Validation error for campsite with AssetId {AssetId}: {Message}", campsiteDto.AssetId, ex.Message);
                    return null;
                }
            })
            .Where(campsite => campsite != null)
            .Select(campsite => campsite!)
            .ToList();

        cache.Set(cacheKey, campsites, CacheDuration);
        return campsites;
    }

    public ILandmark GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}