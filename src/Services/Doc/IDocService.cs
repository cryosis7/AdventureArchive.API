using AdventureArchive.Api.Models.Hut;
using AdventureArchive.Api.Models.Track;

namespace AdventureArchive.Api.Services.Doc
{
    public interface IDocService
    {
        Task<List<TrackModel>> GetTracksAsync(string? regionCode);
        Task<List<HutModel>> GetHutsAsync();
    }
}