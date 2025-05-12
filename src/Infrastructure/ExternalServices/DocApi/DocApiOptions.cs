namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;

public class DocApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public EndpointsOptions Endpoints { get; set; } = new();
    public string CoordinateSystem { get; set; } = string.Empty;

    public class EndpointsOptions
    {
        public string Tracks { get; set; } = string.Empty;
        public string TracksByRegion { get; set; } = string.Empty;
        public string Huts { get; set; } = string.Empty;
        public string Campsites { get; set; } = string.Empty;
    }
}
