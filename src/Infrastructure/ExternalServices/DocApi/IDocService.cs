using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Campsite;

namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi
{
    public interface IDocService
    {
        Task<IEnumerable<TrackDto>> GetTracksAsync(string? regionCode);
        Task<IEnumerable<HutDto>> GetHutsAsync(string? regionCode);
        Task<IEnumerable<CampsiteDto>> GetCampsitesAsync(string? regionCode);
    }
}