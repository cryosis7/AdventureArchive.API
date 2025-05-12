namespace AdventureArchive.Api.Domain.ValueObjects;

public class Location : ValueObject
{
    public double Lat { get; }
    public double Lon { get; }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Lat;
        yield return Lon;
    }
    
    internal Location(double lat, double lon)
    {
        if (lat is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(lat), "Latitude must be between -90 and 90");
        if (lon is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(lon), "Longitude must be between -180 and 180");
        Lat = lat;
        Lon = lon;
    }
}