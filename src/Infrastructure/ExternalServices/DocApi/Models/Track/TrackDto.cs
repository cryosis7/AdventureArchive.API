namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;

public class TrackDto
{
    public Guid AssetId { get; set; }
    public required string Name { get; set; }
    public List<string>? RegionList { get; set; }
    public double Lon { get; set; }
    public double Lat { get; set; }
    public required List<List<List<double>>> Line { get; set; }
}
