using System.Net;
using System.Text.Json;
using AdventureArchive.Api.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace AdventureArchive.Api.Services;

[TestFixture]
public class DocServiceTests
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;
    private IConfiguration _configuration;
    private DocService _docService;

    [SetUp]
    public void SetUp()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var inMemorySettings = new Dictionary<string, string>
        {
            { "DocApi:BaseUrl", "https://example.com" },
            { "DocApi:ApiKey", "test-api-key" },
            { "DocApi:Tracks", "/tracks" },
            { "DocApi:Huts", "/huts" },
            { "DocApi:CoordinateSystem", "NZGD2000" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _docService = new DocService(_httpClient, _configuration);
    }

    [Test]
    public async Task GetTracksAsync_ValidRegionCode_ReturnsTracksData()
    {
        // Arrange
        var responseContent = "[{\"name\":\"Track1\"},{\"name\":\"Track2\"}]";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

        // Act
        var result = await _docService.GetTracksAsync("NZ-NTL");

        // Assert
        result.Should().Be(responseContent);
    }

    [Test]
    public void GetTracksAsync_InvalidRegionCode_ThrowsHttpRequestException()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        // Act & Assert
        Assert.That(() => _docService.GetTracksAsync("INVALID"), Throws.InstanceOf<HttpRequestException>());
    }

    [Test]
    public async Task GetHutsAsync_ValidResponse_ReturnsHutsData()
    {
        // Arrange
        var hut = new DocHutModel
        {
            AssetId = 1,
            Name = "Hut1",
            Status = "OPEN",
            Lat = -40.0,
            Lon = 175.0
        };
        var responseContent = JsonSerializer.Serialize(new List<DocHutModel>
        {
            hut
        });
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

        // Act
        var result = await _docService.GetHutsAsync();

        // Assert
        result.ValidationErrors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
        result.Huts.Should().HaveCount(1);
        result.Huts[0].Should().BeEquivalentTo(hut);
    }

    [Test]
    public async Task GetHutsAsync_InvalidResponse_ReturnsValidationErrors()
    {
        // Arrange
        var hut = new DocHutModel
        {
            AssetId = 1,
            Name = "Hut1",
            Status = "INVALID",
            Lat = -100.0,
            Lon = 200.0,
            Region = "INVALID"
        };
        var responseContent = JsonSerializer.Serialize(new List<DocHutModel>
        {
            hut
        });
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });
        var expectedValidationErrors = new[]
        {
            "Validation error for hut Hut1 (ID: 1): Status must be either OPEN or CLSD",
            "Validation error for hut Hut1 (ID: 1): Region must be a valid region code",
            "Validation error for hut Hut1 (ID: 1): Latitude must be between -90 and 90",
            "Validation error for hut Hut1 (ID: 1): Longitude must be between -180 and 180"
        };

        // Act
        var result = await _docService.GetHutsAsync();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Huts.Should().BeEmpty();
        result.ValidationErrors.Should()
        .BeEquivalentTo(expectedValidationErrors, options => options.WithoutStrictOrdering());
    }

    [Test]
    public async Task GetHutsAsync_HttpRequestException_ReturnsValidationErrors()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await _docService.GetHutsAsync();

        // Assert
        result.IsValid.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle("HTTP request failed: Request failed");
    }

    [Test]
    public async Task GetHutsAsync_GeneralException_ReturnsValidationErrors()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _docService.GetHutsAsync();

        // Assert
        result.IsValid.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle("Unexpected error: Unexpected error");
    }
}