using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;

namespace AdventureArchive.Api.Api.Models.Doc.Huts;

public class GetHutsResponse
{
    public List<HutContract> Huts { get; set; }

    public GetHutsResponse()
    {
        Huts = [];
    }
    
    public GetHutsResponse(IEnumerable<HutDto> huts)
    {
        Huts = huts.Select(hut => hut.ToContract()).ToList();
    }
}