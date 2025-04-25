using AdventureArchive.Api.Models.Hut;
using AdventureArchive.Api.Models.Track;

namespace AdventureArchive.Api.Services.Doc;

public class DocService(IDocHttpClient docHttpClient) : IDocService
{
    private readonly IDocHttpClient _docHttpClient = docHttpClient ?? throw new ArgumentNullException(nameof(docHttpClient));

    public async Task<List<TrackModel>> GetTracksAsync(string? regionCode)
    {
        if (string.IsNullOrEmpty(regionCode))
        {
            return await _docHttpClient.GetAllTracksAsync();
        }

        return await _docHttpClient.GetTracksByRegionAsync(regionCode);
    }

    public async Task<List<HutModel>> GetHutsAsync()
    {
        return await _docHttpClient.GetHutsAsync();
    }
}