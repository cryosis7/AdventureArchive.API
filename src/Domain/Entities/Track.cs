using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Api.Models.Doc.Tracks;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Extensions;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities;

public class Track : IValidatableEntity
{
    [Required]
    [RegularExpression(@"^(\{?[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\}?)$", ErrorMessage = "AssetId must be a valid GUID")]
    public Guid AssetId { get; set; }

    [Required]
    [StringLength(255)]
    public required string Name { get; set; }

    [EnumDataType(typeof(RegionEnum), ErrorMessage = "Region must be a valid from the list of regions (or null)")]
    public IEnumerable<RegionEnum>? RegionList { get; set; }

    [Required]
    public required Location Location { get; set; }

    [Required]
    public required MultiLineString Line { get; set; }
    
    public TrackContract ToContract()
    {
        return new TrackContract
        {
            AssetId = AssetId,
            Name = Name,
            RegionList = RegionList?.Select(r => r.GetName()).ToList(),
            Lat = Location.Lat,
            Lon = Location.Lon,
            Line = Line.ToCoordinateList()
        };
    }
}