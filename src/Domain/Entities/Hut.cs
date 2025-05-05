using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Api.Models.Doc.Huts;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Extensions;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities;

public class Hut : IValidatableEntity
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

    [EnumDataType(typeof(RegionEnum), ErrorMessage = "Region must be a valid from the list of regions (or null)")]
    public RegionEnum? Region { get; set; }

    [Required]
    public required Location Location { get; set; }
    
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
