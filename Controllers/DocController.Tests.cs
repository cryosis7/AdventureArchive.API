using AdventureArchive.Api.Models;
using Moq;
using AdventureArchive.Api.Services;
using AdventureArchive.Api.Models.Response;
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
        _mockDocService
            .Setup(service => service.GetTracksAsync(regionCode))
            .ReturnsAsync("Tracks data");

        var result = await _controller.GetTracks(regionCode) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo("Tracks data"));
    }

    [TestCase("")]
    [TestCase("INVALID")]
    public async Task GetTracks_InvalidRegionCode_ReturnsBadRequest(string regionCode)
    {
        var result = await _controller.GetTracks(regionCode) as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));
        Assert.That(result.Value, Is.EqualTo("Invalid region code."));
    }

    [Test]
    public async Task GetTracks_HttpRequestException_ReturnsInternalServerError()
    {
        var regionCode = "NZ-NTL";
        _mockDocService
            .Setup(service => service.GetTracksAsync(regionCode))
            .ThrowsAsync(new HttpRequestException("Error calling DOC API."));

        var result = await _controller.GetTracks(regionCode) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(500));
        Assert.That(((dynamic)result.Value).message, Is.EqualTo("Error calling DOC API."));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetHuts_ReturnsOk(bool validResponse)
    {
        string[] validationErrors = validResponse ? [] : ["Invalid data"];
        var hutsResponse = new DocHutsResponse
            { IsValid = validResponse, ValidationErrors = [..validationErrors] };
        _mockDocService
            .Setup(service => service.GetHutsAsync())
            .ReturnsAsync(hutsResponse);

        var result = await _controller.GetHuts() as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo(hutsResponse));
    }

    [Test]
    public async Task GetHuts_HttpRequestException_ReturnsInternalServerError()
    {
        _mockDocService
            .Setup(service => service.GetHutsAsync())
            .ThrowsAsync(new HttpRequestException("Error calling DOC API."));

        var result = await _controller.GetHuts() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(500));
        Assert.That(((dynamic)result.Value).message, Is.EqualTo("Error calling DOC API."));
    }
}