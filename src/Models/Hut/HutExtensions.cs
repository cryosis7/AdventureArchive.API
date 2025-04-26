namespace AdventureArchive.Api.Models.Hut;

public static class HutExtensions
{
    public static HutsContract ToContract(this List<HutModel> models)
    {
        return new HutsContract { Huts = models.Select(m => m.ToContract()).ToList() };
    }

    public static HutContract ToContract(this HutModel model)
    {
        return new HutContract
        {
            AssetId = model.AssetId,
            Name = model.Name,
            Status = model.Status,
            Region = model.Region,
            Lat = model.Lat,
            Lon = model.Lon
        };
    }
}