using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Campsite;
using Microsoft.Extensions.Options;

namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;

public interface IDocHttpClient
{
    Task<List<TrackDto>> GetAllTracksAsync();
    Task<List<TrackDto>> GetTracksByRegionAsync(string regionCode);
    Task<List<HutDto>> GetHutsAsync();
    Task<List<HutDto>> GetHutsByRegionAsync(string regionCode);
    Task<List<CampsiteDto>> GetCampsitesAsync();
    Task<List<CampsiteDto>> GetCampsitesByRegionAsync(string regionCode);
}

public class DocHttpClient : IDocHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly DocApiOptions _docApiOptions;
    
    private readonly JsonSerializerOptions _jsonSerialisationOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public DocHttpClient(HttpClient httpClient, IOptions<DocApiOptions> docApiOptions)
    {
        _docApiOptions = docApiOptions.Value ?? throw new ArgumentNullException(nameof(docApiOptions));

        if (string.IsNullOrWhiteSpace(_docApiOptions.BaseUrl) || string.IsNullOrWhiteSpace(_docApiOptions.ApiKey))
        {
            throw new InvalidOperationException("DocApi BaseUrl and ApiKey must be configured.");
        }

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_docApiOptions.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _docApiOptions.ApiKey);
    }

    public async Task<List<TrackDto>> GetAllTracksAsync()
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Tracks);

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        
        var jsonString = await response.Content.ReadAsStringAsync();
        var tracks = JsonSerializer.Deserialize<List<TrackDto>>(jsonString, _jsonSerialisationOptions);
        return tracks ?? [];
    }

    public async Task<List<TrackDto>> GetTracksByRegionAsync(string regionCode) // TODO: Change to enum
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Tracks, new Dictionary<string, string>
        {
            { "region", regionCode }
        });

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        
        var jsonString = await response.Content.ReadAsStringAsync();
        var tracks = JsonSerializer.Deserialize<List<TrackDto>>(jsonString, _jsonSerialisationOptions);
        return tracks ?? [];
    }

    public async Task<List<HutDto>> GetHutsAsync()
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Huts);
        
        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var huts = JsonSerializer.Deserialize<List<HutDto>>(jsonString, _jsonSerialisationOptions);

        return huts ?? [];
    }
    
    public async Task<List<HutDto>> GetHutsByRegionAsync(string regionCode)
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Huts, new Dictionary<string, string>
        {
            { "region", regionCode }
        });

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var huts = JsonSerializer.Deserialize<List<HutDto>>(jsonString, _jsonSerialisationOptions);

        return huts ?? [];
    }

    public async Task<List<CampsiteDto>> GetCampsitesAsync()
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Campsites);
        
        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var campsites = JsonSerializer.Deserialize<List<HutDto>>(jsonString, _jsonSerialisationOptions);

        return campsites ?? [];
    }

    public async Task<List<CampsiteDto>> GetCampsitesByRegionAsync(string regionCode)
    {
               var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Huts, new Dictionary<string, string>
        {
            { "region", regionCode }
        });

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var huts = JsonSerializer.Deserialize<List<HutDto>>(jsonString, _jsonSerialisationOptions);

        return huts ?? [];
    }

    private string BuildRequestUrl(string endpoint, Dictionary<string, string>? queryParameters = null)
    {
        var path = endpoint.TrimStart('/');

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["coordinates"] = _docApiOptions.CoordinateSystem;

        if (queryParameters != null)
        {
            foreach (var param in queryParameters)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    query[param.Key] = param.Value;
                }
            }
        }

        var queryString = query.ToString();
        var requestUrl = !string.IsNullOrEmpty(queryString) ? $"{path}?{queryString}" : path;

        return requestUrl;
    }
}