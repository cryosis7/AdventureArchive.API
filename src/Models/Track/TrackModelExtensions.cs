namespace AdventureArchive.Api.Models.Track;

public static class TrackModelExtensions
{
    public static TrackContract ToContract(this TrackModel model)
    {
        return new TrackContract
        {
            AssetId = model.AssetId,
            Name = model.Name,
            Region = model.Region,
            Lon = model.Lon,
            Lat = model.Lat,
            Line = model.Line
        };
    }
    
    public static TracksContract ToContract(this List<TrackModel> models)
    {
        return new TracksContract
        {
            Tracks = models.Select(m => m.ToContract()).ToList()
        };
    }
}