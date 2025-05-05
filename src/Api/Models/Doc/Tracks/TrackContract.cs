namespace AdventureArchive.Api.Api.Models.Doc.Tracks;

public class TrackContract
{
    public Guid AssetId { get; set; }
    public required string Name { get; set; }
    public List<string>? Region { get; set; }
    public double Lon { get; set; }
    public double Lat { get; set; }
    public required List<List<List<double>>> Line { get; set; }
}