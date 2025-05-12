using AdventureArchive.Api.Domain.Entities.Landmark;

namespace AdventureArchive.Api.Api.Models.Doc.Huts;

public class GetHutsResponse(IEnumerable<HutLandmark> huts)
{
    public IEnumerable<HutLandmark> Huts { get; init; } = huts;

    public GetHutsResponse() : this([])
    {
    }
}