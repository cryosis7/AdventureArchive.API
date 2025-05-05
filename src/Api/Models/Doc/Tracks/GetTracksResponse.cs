using AdventureArchive.Api.Domain.Entities;

namespace AdventureArchive.Api.Api.Models.Doc.Tracks;

public class GetTracksResponse
{
    public List<TrackContract> Tracks { get; set; }

    public GetTracksResponse()
    {
        Tracks = [];
    }

    public GetTracksResponse(IEnumerable<Track> tracks)
    {
        Tracks = tracks.Select(track => track.ToContract()).ToList();
    }
}
