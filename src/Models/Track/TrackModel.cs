using System.ComponentModel.DataAnnotations;

namespace AdventureArchive.Api.Models.Track;

public class TrackModel
{
    [Required]
    public Guid AssetId { get; set; }

    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [Required]
    public List<string> Region { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Lon { get; set; }

    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Lat { get; set; }

    [Required]
    public required List<List<List<double>>> Line { get; set; }
}
