namespace AdventureArchive.Infrastructure.ExternalServices.DocApi.Models.Campsite;

public class CampsiteDto
{
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string? Description { get; set; }
    // Add other properties as needed
}
