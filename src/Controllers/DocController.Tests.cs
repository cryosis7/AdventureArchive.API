using AdventureArchive.Api.Models;
using AdventureArchive.Api.Models.Hut;
using AdventureArchive.Api.Models.Track;
using Moq;
using AdventureArchive.Api.Services;
using AdventureArchive.Api.Services.Doc;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace AdventureArchive.Api.Controllers;

[TestFixture]
public class DocControllerTests
{
    private Mock<IDocService> _mockDocService;
    private DocController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockDocService = new Mock<IDocService>();
        _controller = new DocController(_mockDocService.Object);
    }

    [Test]
    public async Task GetTracks_NoRegionCode_ReturnsOk()
    {
        var trackModel = new TrackModel() { Name = "Track 1", Line = [] };
        _mockDocService
            .Setup(service => service.GetTracksAsync(null))
            .ReturnsAsync([trackModel]);

        var result = await _controller.GetTracks(null);
        
        result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(new TracksContract
        {
            Tracks = [trackModel.ToContract()]
        });
    }

    [Test]
    public async Task GetTracks_ValidRegionCode_ReturnsOk()
    {
        var trackModel = new TrackModel() { Name = "Track 1", Line = [] };
        _mockDocService
            .Setup(service => service.GetTracksAsync("NZ-NTL"))
            .ReturnsAsync([trackModel]);

        var result = await _controller.GetTracks("NZ-NTL");
        
        result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(new TracksContract
        {
            Tracks = [trackModel.ToContract()]
        });
    }

    [Test]
    public async Task GetTracks_InvalidRegionCode_ReturnsBadRequest()
    {
        var result = await _controller.GetTracks("INVALID");

        result.Result.Should().BeOfType<BadRequestObjectResult>().Which
            .StatusCode.Should().Be(400);
        result.Result.As<ObjectResult>().Value.Should().Be("Invalid region code.");
    }

    [Test]
    public async Task GetTracks_HttpRequestException_ReturnsInternalServerError()
    {
        var regionCode = "NZ-NTL";
        _mockDocService
            .Setup(service => service.GetTracksAsync(regionCode))
            .ThrowsAsync(new HttpRequestException("Error calling DOC API."));

        var result = await _controller.GetTracks(regionCode);

        result.Result.Should().BeOfType<ObjectResult>().Which
            .StatusCode.Should().Be(500);
        // result.Value.Should().Be("Error calling DOC API.");
        // ((dynamic)result.Value).message.Should().Be("Error calling DOC API.");
    }

    [Test]
    public async Task GetHuts_ReturnsOk()
    {
        var hut = new HutModel
        {
            Name = "Hut 1",
            AssetId = 12345,
            Status = "OPEN",
            Region = "NZ-NTL"
        };

        _mockDocService
            .Setup(service => service.GetHutsAsync())
            .ReturnsAsync([hut]);

        var result = await _controller.GetHuts();

        result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(new HutsContract
        {
            Huts = [hut.ToContract()]
        });
    }

    [Test]
    public async Task GetHuts_HttpRequestException_ReturnsInternalServerError()
    {
        _mockDocService
            .Setup(service => service.GetHutsAsync())
            .ThrowsAsync(new HttpRequestException("Error calling DOC API."));

        var result = await _controller.GetHuts();

        result.Result.Should().BeOfType<ObjectResult>().Which
            .StatusCode.Should().Be(500);
    }
}