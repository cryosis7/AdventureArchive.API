using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Factories;

public class LandmarkCreationParameters
{
    public required LandmarkType LandmarkType { get; set; }
    public required string Name { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    
    public string? Description { get; set; }
    public string? DocId { get; set; }
}