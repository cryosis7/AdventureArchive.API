namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;

public class HutDto
{
    public long AssetId { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public string? Region { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}