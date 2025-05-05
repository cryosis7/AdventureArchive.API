using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Api.Models.Doc.Huts;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Hut;

public class Hut
{
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "Asset ID must be a positive number")]
    public long AssetId { get; set; }

    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [Required]
    [EnumDataType(typeof(StatusEnum), ErrorMessage = "Status must be a valid value of the StatusEnum")]
    public required StatusEnum Status { get; set; }

    [EnumDataType(typeof(StatusEnum), ErrorMessage = "Region must be a valid from the list of regions (or null)")]
    public Region? Region { get; set; }

    [Required]
    public required Location Location { get; set; }

    public void Validate()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(this, context, results, true))
        {
            return;
        }

        var errors = string.Join(", ", results.Select(r => r.ErrorMessage));
        throw new ValidationException($"Validation failed: {errors}");
    }

    public HutContract ToContract()
    {
        return new HutContract
        {
            AssetId = AssetId,
            Name = Name,
            Status = Status.ToString(),
            Region = Region.GetName(),
            Lat = Location.Lat,
            Lon = Location.Lon
        };
    }
}
