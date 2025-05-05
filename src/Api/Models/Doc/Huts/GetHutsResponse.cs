
using AdventureArchive.Api.Domain.Entities;

namespace AdventureArchive.Api.Api.Models.Doc.Huts;

public class GetHutsResponse
{
    public List<HutContract> Huts { get; set; }

    public GetHutsResponse()
    {
        Huts = [];
    }
    
    public GetHutsResponse(IEnumerable<Hut> huts)
    {
        Huts = huts.Select(hut => hut.ToContract()).ToList();
    }
}