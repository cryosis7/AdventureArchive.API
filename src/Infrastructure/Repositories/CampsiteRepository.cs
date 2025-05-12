using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureArchive.Api.Infrastructure.Repositories;

public class CampsiteRepository(IDocService docService, IMemoryCache cache, ILandmarkFactory factory) : ICampsiteRepository
{
    private readonly IDocService _docService = docService ?? throw new ArgumentNullException(nameof(docService));
    private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);
    private const string CacheKeyPrefix = "Campsites";

    public async Task<IEnumerable<IDocLandmark>> GetByRegion(RegionEnum regionCode)
    {
        return await GetCampsitesInternalAsync(regionCode);
    }

    public async Task<IEnumerable<ILandmark>> GetAllAsync()
    {
        return await GetCampsitesInternalAsync(null);
    }

    private async Task<IEnumerable<IDocLandmark>> GetCampsitesInternalAsync(RegionEnum? regionCode)
    {
        var cacheKey = regionCode.HasValue ? $"{CacheKeyPrefix}_{regionCode.Value}" : $"{CacheKeyPrefix}_All";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<IDocLandmark>? cachedCampsites) && cachedCampsites != null)
        {
            return cachedCampsites;
        }

        Log.Information("Cache miss for campsites CacheKey: {CacheKey} - Fetching from DocApi", cacheKey);
        // TODO: Implement GetCampsitesAsync in IDocService and DocService
        var campsitesResponse = await _docService.GetCampsitesAsync(regionCode?.ToString());
        var campsites = campsitesResponse
            .Select(campsiteDto =>
            {
                try
                {
                    return factory.CreateDocLandmark(
                        docId: campsiteDto.AssetId.ToString(),
                        name: campsiteDto.Name,
                        latitude: campsiteDto.Lat,
                        longitude: campsiteDto.Lon,
                        landmarkType: DocLandmarkType.Campsite
                    );
                }
                catch (ArgumentException ex)
                {
                    Log.Error(ex, "Argument Validation error for campsite with AssetId {AssetId}: {Message}", campsiteDto.AssetId,
                        ex.Message);
                    return null;
                }
            })
            .Where(campsite => campsite != null)
            .Select(campsite => campsite!)
            .ToList();

        _cache.Set(cacheKey, campsites, CacheDuration);
        return campsites;
    }

    public ILandmark GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
