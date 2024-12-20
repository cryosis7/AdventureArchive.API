using FluentAssertions;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Constants;

namespace AdventureArchive.Api.Attributes.Validation;

[TestFixture]
public class RegionCodeAttributeTests
{
    [TestCase(Regions.Northland)]
    [TestCase(Regions.Auckland)]
    [TestCase(Regions.WaikatoCoromandel)]
    [TestCase(Regions.Coromandel)]
    [TestCase(Regions.BayOfPlenty)]
    [TestCase(Regions.EastCoast)]
    [TestCase(Regions.Taranaki)]
    [TestCase(Regions.ManawatuWhanganuiCentralNorthIsland)]
    [TestCase(Regions.CentralNorthIsland)]
    [TestCase(Regions.HawkesBay)]
    [TestCase(Regions.WellingtonKapiti)]
    [TestCase(Regions.Wairarapa)]
    [TestCase(Regions.ChathamIslands)]
    [TestCase(Regions.NelsonTasman)]
    [TestCase(Regions.Marlborough)]
    [TestCase(Regions.WestCoast)]
    [TestCase(Regions.Canterbury)]
    [TestCase(Regions.Otago)]
    [TestCase(Regions.SouthlandFiordland)]
    [TestCase(Regions.Fiordland)]
    public void IsValid_ReturnsSuccess_WhenRegionCodeIsValid(string regionCode)
    {
        var attribute = new RegionCodeAttribute();
        var context = new ValidationContext(new object());
        var result = attribute.GetValidationResult(regionCode, context);
        result.Should().Be(ValidationResult.Success);
    }

    [Test]
    public void IsValid_ReturnsError_WhenRegionCodeIsInvalid()
    {
        var attribute = new RegionCodeAttribute();
        var context = new ValidationContext(new object());
        var result = attribute.GetValidationResult("InvalidCode", context);
        result?.ErrorMessage.Should().Be("Region must be a valid region code");
    }

    [Test]
    public void IsValid_ReturnsSuccess_WhenValueIsNull()
    {
        var attribute = new RegionCodeAttribute();
        var context = new ValidationContext(new object());
        var result = attribute.GetValidationResult(null, context);
        result.Should().Be(ValidationResult.Success);
    }

    [Test]
    public void IsValid_ReturnsError_WhenValueIsNotString()
    {
        var attribute = new RegionCodeAttribute();
        var context = new ValidationContext(new object());
        var result = attribute.GetValidationResult(123, context);
        result?.ErrorMessage.Should().Be("Region code must be a string");
    }
    
    [Test]
    public void IsValid_ReturnsError_WhenRegionCodeIsEmptyString()
    {
        var attribute = new RegionCodeAttribute();
        var context = new ValidationContext(new object());
        var result = attribute.GetValidationResult(string.Empty, context);
        result?.ErrorMessage.Should().Be("Region must be a valid region code");
    }
}