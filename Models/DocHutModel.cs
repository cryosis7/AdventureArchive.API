using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Constants;

namespace AdventureArchive.Api.Models;

public class DocHutModel : IValidatableObject
{
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "Asset ID must be a positive number")]

    public long AssetId { get; set; }

    [Required] [StringLength(255)] public string Name { get; set; }

    [Required] public string Status { get; set; }

    public string? Region { get; set; }

    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Lat { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Lon { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("Name cannot be empty or whitespace", new[] { nameof(Name) });
        }

        if (!string.Equals(Status, "OPEN", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(Status, "CLSD", StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult("Status must be either OPEN or CLSD",
                new[] { nameof(Status) });
        }

        if (Region != null)
        {
            if (string.IsNullOrWhiteSpace(Region))
            {
                yield return new ValidationResult("Region cannot be empty or whitespace",
                    new[] { nameof(Region) });
            }
            else if (!Regions.RegionNames.ContainsValue(Region))
            {
                yield return new ValidationResult("Region must be a valid region name",
                    new[] { nameof(Region) });
            }
        }
        
        if (Lat is < -90 or > 90)
        {
            yield return new ValidationResult("Latitude must be between -90 and 90",
                new[] { nameof(Lat) });
        }

        if (Lon is < -180 or > 180)
        {
            yield return new ValidationResult("Longitude must be between -180 and 180",
                new[] { nameof(Lon) });
        }
    }
}