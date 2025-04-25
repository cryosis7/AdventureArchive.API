using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Attributes.Validation;

namespace AdventureArchive.Api.Models.Hut;

public class HutModel
{
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "Asset ID must be a positive number")]
    public long AssetId { get; set; }

    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [Required]
    [RegularExpression("^(OPEN|CLSD)$", ErrorMessage = "Status must be either OPEN or CLSD")]
    public required string Status { get; set; }

    [RegionCode]
    public string? Region { get; set; }

    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Lat { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Lon { get; set; }
}