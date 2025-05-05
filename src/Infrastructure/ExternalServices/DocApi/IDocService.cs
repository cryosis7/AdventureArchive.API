using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;

namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi
{
    public interface IDocService
    {
        Task<IEnumerable<TrackDto>> GetTracksAsync(string? regionCode);
        Task<IEnumerable<HutDto>> GetHutsAsync(string? regionCode);
    }
}