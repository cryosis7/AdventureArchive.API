namespace AdventureArchive.Api.Models.Response;

public class DocHutsResponse
{
    public List<DocHutModel> Huts { get; set; } = [];
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = [];
}