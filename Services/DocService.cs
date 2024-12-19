using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Web;
using AdventureArchive.Api.Models;
using AdventureArchive.Api.Models.Response;

namespace AdventureArchive.Api.Services;

public class DocService
{
    private readonly HttpClient _httpClient;
    private readonly string _tracksEndpoint;
    private readonly string _hutsEndpoint;
    private readonly string _coordinateSystem;

    public DocService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        var baseUrl = configuration["DocApi:BaseUrl"];
        var apiKey = configuration["DocApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("DocApi:BaseUrl and DocApi:ApiKey must be configured.");
        }

        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _tracksEndpoint = configuration["DocApi:Tracks"] ?? "";
        _hutsEndpoint = configuration["DocApi:Huts"] ?? "";
        _coordinateSystem = configuration["DocApi:CoordinateSystem"] ?? "";
    }

    public async Task<string> GetTracksAsync(string? regionCode)
    {
        var requestUrl = regionCode != null
            ? BuildRequestUrl(_tracksEndpoint, new Dictionary<string, string>
            {
                { "region", regionCode }
            })
            : BuildRequestUrl(_tracksEndpoint);

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<DocHutsResponse> GetHutsAsync()
    {
        try
        {
            var requestUrl = BuildRequestUrl(_hutsEndpoint);

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var huts = JsonSerializer.Deserialize<List<DocHutModel>>(jsonString);

            if (huts == null)
            {
                return new DocHutsResponse
                {
                    IsValid = false,
                    ValidationErrors = ["Failed to deserialize huts data"]
                };
            }

            var validationErrors = new List<ValidationResult>();
            var allErrors = new List<string>();

            foreach (var hut in huts)
            {
                var validationContext = new ValidationContext(hut);
                var isValid = Validator.TryValidateObject(hut, validationContext, validationErrors, true);

                if (!isValid)
                {
                    allErrors.AddRange(validationErrors.Select(validationResult =>
                        $"Validation error for hut {hut.Name} (ID: {hut.AssetId}): {validationResult.ErrorMessage}"));
                }
            }

            return new DocHutsResponse
            {
                Huts = huts,
                IsValid = allErrors.Count == 0,
                ValidationErrors = allErrors
            };
        }
        catch (HttpRequestException ex)
        {
            return new DocHutsResponse
            {
                IsValid = false,
                ValidationErrors = [$"HTTP request failed: {ex.Message}"]
            };
        }
        catch (Exception ex)
        {
            return new DocHutsResponse
            {
                IsValid = false,
                ValidationErrors = [$"Unexpected error: {ex.Message}"]
            };
        }
    }

    private string BuildRequestUrl(string endpoint, Dictionary<string, string>? queryParameters = null)
    {
        var path = endpoint.TrimStart('/');

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["coordinates"] = _coordinateSystem;

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
        if (!string.IsNullOrEmpty(queryString))
        {
            return $"{path}?{queryString}";
        }

        return path;
    }
}