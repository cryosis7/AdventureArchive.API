namespace AdventureArchive.Api.Api.Models.Doc.Huts;

public class HutContract
{
    public long AssetId { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public string? Region { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}