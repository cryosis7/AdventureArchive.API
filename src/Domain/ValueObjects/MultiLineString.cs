using System.ComponentModel.DataAnnotations;

namespace AdventureArchive.Api.Domain.ValueObjects;

public class MultiLineString : ValueObject
{
    [Required]
    public List<List<Location>> Lines { get; }

    public MultiLineString(List<List<List<double>>> coordinates)
    {
        if (coordinates == null)
        {
            throw new ValidationException("Coordinates cannot be null.");
        }

        if (coordinates.Count == 0)
        {
            throw new ValidationException("Coordinates cannot be empty.");
        }

        if (coordinates.Any(line => line.Count == 0))
        {
            throw new ValidationException("Each line must contain at least one point.");
        }

        if (coordinates.Any(line => line.Any(point => point.Count != 2)))
        {
            throw new ValidationException("Each point must have exactly two values (longitude and latitude).");
        }

        Lines = coordinates
            .Select(line => line
                .Select(point => Location.CreateLocation(point[1], point[0]))
                .ToList())
            .ToList();
    }

    public List<List<List<double>>> ToCoordinateList()
    {
        return Lines
            .Select(line => line
                .Select(location => new List<double> { location.Lon, location.Lat })
                .ToList())
            .ToList();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        foreach (var line in Lines)
        {
            foreach (var location in line)
            {
                yield return location;
            }
        }
    }
}