using AdventureArchive.Api.Domain.Entities.Landmark;

namespace AdventureArchive.Api.Api.Models.Doc;

public class GetCampsitesResponse(IEnumerable<CampsiteLandmark> campsites)
{
    public IEnumerable<CampsiteLandmark> Campsites { get; init; } = campsites;

    public GetCampsitesResponse() : this([])
    {
    }
}