using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using AdventureArchive.Api.Models;
using AdventureArchive.Api.Models.Hut;
using AdventureArchive.Api.Models.Track;
using Microsoft.Extensions.Options;

namespace AdventureArchive.Api.Services.Doc;

public interface IDocHttpClient
{
    Task<List<TrackModel>> GetAllTracksAsync();
    Task<List<TrackModel>> GetTracksByRegionAsync(string regionCode);
    Task<List<HutModel>> GetHutsAsync();
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

    public async Task<List<TrackModel>> GetAllTracksAsync()
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Tracks);

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        
        var jsonString = await response.Content.ReadAsStringAsync();
        var tracks = JsonSerializer.Deserialize<List<TrackModel>>(jsonString, _jsonSerialisationOptions);
        return tracks ?? [];
    }

    public async Task<List<TrackModel>> GetTracksByRegionAsync(string regionCode) // TODO: Change to enum
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Tracks, new Dictionary<string, string>
        {
            { "region", regionCode }
        });

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        
        var jsonString = await response.Content.ReadAsStringAsync();
        var tracks = JsonSerializer.Deserialize<List<TrackModel>>(jsonString, _jsonSerialisationOptions);
        return tracks ?? [];
    }

    public async Task<List<HutModel>> GetHutsAsync()
    {
        var requestUrl = BuildRequestUrl(_docApiOptions.Endpoints.Huts);

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var huts = JsonSerializer.Deserialize<List<HutModel>>(jsonString, _jsonSerialisationOptions);

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
        return !string.IsNullOrEmpty(queryString) ? $"{path}?{queryString}" : path;
    }
}