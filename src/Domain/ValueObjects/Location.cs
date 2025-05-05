using System.ComponentModel.DataAnnotations;

namespace AdventureArchive.Api.Domain.ValueObjects;

public class Location : ValueObject
{
    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Lat { get; private init; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Lon { get; private init; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Lat;
        yield return Lon;
    }
    
    private Location() { }

    public static Location CreateLocation(double lat, double lon)
    {
        var location = new Location { Lat = lat, Lon = lon };
        var context = new ValidationContext(location);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(location, context, results, true))
        {
            return location;
        }
        
        var errors = string.Join(", ", results.Select(r => r.ErrorMessage));
        throw new ValidationException($"Validation failed: {errors}");
    }
}