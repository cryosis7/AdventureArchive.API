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

    [TestCase("NZ-NTL")]
    [TestCase(null)]
    public async Task GetTracks_ValidRegionCode_ReturnsOk(string regionCode)
    {
        List<TrackModel> response = [new() { Name = "Track 1", Line = [] }];
        _mockDocService
            .Setup(service => service.GetTracksAsync(regionCode))
            .ReturnsAsync(response);

        var result = await _controller.GetTracks(regionCode);

        result.Should().NotBeNull();
        result.Result.Should().Be(200);
        result.Value.Should().BeEquivalentTo(response);
    }

    [TestCase("")]
    [TestCase("INVALID")]
    public async Task GetTracks_InvalidRegionCode_ReturnsBadRequest(string regionCode)
    {
        var result = await _controller.GetTracks(regionCode);

        result.Should().NotBeNull();
        result.Result.Should().Be(400);
        result.Value.Should().Be("Invalid region code.");
    }

    [Test]
    public async Task GetTracks_HttpRequestException_ReturnsInternalServerError()
    {
        var regionCode = "NZ-NTL";
        _mockDocService
            .Setup(service => service.GetTracksAsync(regionCode))
            .ThrowsAsync(new HttpRequestException("Error calling DOC API."));

        var result = await _controller.GetTracks(regionCode);

        result.Should().NotBeNull();
        result.Result.Should().Be(500);
        // result.Value.Should().Be("Error calling DOC API.");
        // ((dynamic)result.Value).message.Should().Be("Error calling DOC API.");
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetHuts_ReturnsOk(bool validResponse)
    {
        string[] validationErrors = validResponse ? [] : ["Invalid data"];
        var hutsResponse = new HutsContract
        {
            IsValid = validResponse,
            ValidationErrors = validationErrors.ToList(),
            Huts = []
        };
        _mockDocService
            .Setup(service => service.GetHutsAsync())
            .ReturnsAsync([]);

        var result = await _controller.GetHuts();

        result.Should().NotBeNull();
        result.Result.Should().Be(200);
        result.Value.Should().BeEquivalentTo(hutsResponse);
    }

    [Test]
    public async Task GetHuts_HttpRequestException_ReturnsInternalServerError()
    {
        _mockDocService
            .Setup(service => service.GetHutsAsync())
            .ThrowsAsync(new HttpRequestException("Error calling DOC API."));

        var result = await _controller.GetHuts();

        result.Should().NotBeNull();
        result.Result.Should().Be(500);
        // result.Value.Should().Be("Error calling DOC API.");
    }
}
