using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace AdventureArchive.Api.Models.Hut;

[TestFixture]
public class HutModelTests
{
    [Test]
    public void ValidData_ShouldPassValidation()
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "Test Hut",
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Test]
    public void MissingRegion_ShouldPassValidation()
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "Test Hut",
            Status = "OPEN",
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Test]
    public void InvalidAssetId_ShouldFailValidation()
    {
        var model = new HutModel
        {
            AssetId = 0,
            Name = "Test Hut",
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "Asset ID must be a positive number");
    }

    [Test]
    public void MissingAssetId_ShouldFailValidation()
    {
        var model = new HutModel
        {
            Name = "Test Hut",
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
    }

    [Test]
    public void EmptyName_ShouldFailValidation()
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "",
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "The Name field is required.");
    }

    [Test]
    public void NameExceedsMaxLength_ShouldFailValidation()
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = new string('A', 256),
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "The field Name must be a string with a maximum length of 255.");
    }

    [Test]
    public void InvalidStatus_ShouldFailValidation()
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "Test Hut",
            Status = "INVALID",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "Status must be either OPEN or CLSD");
    }

    [Test]
    public void InvalidRegion_ShouldFailValidation()
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "Test Hut",
            Status = "OPEN",
            Region = "INVALID",
            Lat = -36.8485,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "Region must be a valid region code");
    }

    [TestCase(-91)]
    [TestCase(91)]
    public void InvalidLatitude_ShouldFailValidation(double lat)
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "Test Hut",
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = lat,
            Lon = 174.7633
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "Latitude must be between -90 and 90");
    }

    [TestCase(-181)]
    [TestCase(181)]
    public void InvalidLongitude_ShouldFailValidation(double lon)
    {
        var model = new HutModel
        {
            AssetId = 1,
            Name = "Test Hut",
            Status = "OPEN",
            Region = Regions.Northland,
            Lat = -36.8485,
            Lon = lon
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().HaveCount(1);
        validationResults.Should().ContainSingle(vr => vr.ErrorMessage == "Longitude must be between -180 and 180");
    }
}