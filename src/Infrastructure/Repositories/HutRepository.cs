using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureArchive.Api.Infrastructure.Repositories;

public class HutRepository(IDocService docService, IMemoryCache cache, ILandmarkFactory factory) : IDocLandmarkRepository
{
    private readonly IDocService _docService = docService ?? throw new ArgumentNullException(nameof(docService));
    private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);
    private const string CacheKeyPrefix = "Huts";

    public async Task<IEnumerable<IDocLandmark>> GetByRegion(RegionEnum regionCode)
    {
        return await GetHutsInternalAsync(regionCode);
    }

    public async Task<IEnumerable<ILandmark>> GetAllAsync()
    {
        return await GetHutsInternalAsync(null);
    }

    private async Task<IEnumerable<IDocLandmark>> GetHutsInternalAsync(RegionEnum? regionCode)
    {
        var cacheKey = regionCode.HasValue ? $"{CacheKeyPrefix}_{regionCode.Value}" : $"{CacheKeyPrefix}_All";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<IDocLandmark>? cachedHuts) && cachedHuts != null)
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
                    return factory.CreateDocLandmark(
                        docId: hutDto.AssetId.ToString(),
                        name: hutDto.Name,
                        latitude: hutDto.Lat,
                        longitude: hutDto.Lon,
                        landmarkType: DocLandmarkType.Hut
                    );
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