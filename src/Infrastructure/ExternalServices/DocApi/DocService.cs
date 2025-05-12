using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Campsite;

namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;

public class DocService(IDocHttpClient docHttpClient) : IDocService
{
    private readonly IDocHttpClient _docHttpClient = docHttpClient ?? throw new ArgumentNullException(nameof(docHttpClient));

    public async Task<IEnumerable<TrackDto>> GetTracksAsync(string? regionCode)
    {
        if (string.IsNullOrEmpty(regionCode))
        {
            return await _docHttpClient.GetAllTracksAsync();
        }

        return await _docHttpClient.GetTracksByRegionAsync(regionCode);
    }

    public async Task<IEnumerable<HutDto>> GetHutsAsync(string? regionCode)
    {
        if (string.IsNullOrEmpty(regionCode))
        {
            return await _docHttpClient.GetHutsAsync();
        }

        return await _docHttpClient.GetHutsByRegionAsync(regionCode);
    }

    public async Task<IEnumerable<CampsiteDto>> GetCampsitesAsync(string? regionCode)
    {
        if (string.IsNullOrEmpty(regionCode))
        {
            return await _docHttpClient.GetCampsitesAsync();
        }
        return await _docHttpClient.GetCampsitesByRegionAsync(regionCode);
    }
}