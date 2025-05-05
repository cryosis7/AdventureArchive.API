using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;

namespace AdventureArchive.Api.Api.Models.Doc.Tracks;

public class GetTracksResponse
{
    public List<TrackContract> Tracks { get; set; }

    public GetTracksResponse()
    {
        Tracks = [];
    }

    public GetTracksResponse(IEnumerable<TrackModel> tracks)
    {
        Tracks = tracks.Select(track => track.ToContract()).ToList();
    }
}
