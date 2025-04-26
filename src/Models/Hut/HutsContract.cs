namespace AdventureArchive.Api.Models.Hut;

public class HutsContract
{
    public required List<HutContract> Huts { get; set; }
}

public class HutContract
{
    public long AssetId { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public string? Region { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}