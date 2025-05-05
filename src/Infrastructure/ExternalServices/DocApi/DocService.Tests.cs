using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Track;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;

[TestFixture]
public class DocServiceTests
{
    private Mock<IDocHttpClient> _docHttpClientMock;
    private DocService _docService;

    [SetUp]
    public void SetUp()
    {
        _docHttpClientMock = new Mock<IDocHttpClient>();
        _docService = new DocService(_docHttpClientMock.Object);
    }

    [Test]
    public async Task GetTracksAsync_ValidRegionCode_ReturnsTracksData()
    {
        // Arrange
        List<TrackModel> responseContent =
        [
            new() { Name = "Track1", Line = [] },
            new() { Name = "Track2", Line = [] }
        ];
        _docHttpClientMock
            .Setup(client => client.GetTracksByRegionAsync("NZ-NTL"))
            .ReturnsAsync(responseContent);

        // Act
        var result = await _docService.GetTracksAsync("NZ-NTL");

        // Assert
        result.Should().BeEquivalentTo(responseContent);
    }

    [Test]
    public void GetTracksAsync_InvalidRegionCode_ThrowsHttpRequestException()
    {
        // Arrange
        _docHttpClientMock
            .Setup(client => client.GetTracksByRegionAsync("INVALID"))
            .ThrowsAsync(new HttpRequestException());

        // Act & Assert
        Assert.That(() => _docService.GetTracksAsync("INVALID"), Throws.InstanceOf<HttpRequestException>());
    }

    [Test]
    public async Task GetHutsAsync_ValidResponse_ReturnsHutsData()
    {
        // Arrange
        var hutsResponse = new List<HutDto>
        {
            new() { AssetId = 1, Name = "Hut1", Status = "OPEN", Lat = -40.0, Lon = 175.0 }
        };
        _docHttpClientMock.Setup(client => client.GetHutsAsync())
            .ReturnsAsync(hutsResponse);

        // Act
        var result = await _docService.GetHutsAsync();

        // Assert
        result.Should().BeEquivalentTo(hutsResponse);
    }

    [Test]
    public void GetHutsAsync_HttpRequestException_ThrowsHttpRequestException()
    {
        // Arrange
        _docHttpClientMock.Setup(client => client.GetHutsAsync())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act & Assert
        Assert.That(() => _docService.GetHutsAsync(), Throws.InstanceOf<HttpRequestException>());
    }

    [Test]
    public void GetHutsAsync_GeneralException_ThrowsException()
    {
        // Arrange
        _docHttpClientMock.Setup(client => client.GetHutsAsync())
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act & Assert
        Assert.That(() => _docService.GetHutsAsync(), Throws.InstanceOf<Exception>().With.Message.EqualTo("Unexpected error"));
    }
}
