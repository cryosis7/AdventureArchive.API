using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Constants;
using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Attributes.Validation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RegionCodeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success!;
        }

        if (value is not string regionCode)
        {
            return new ValidationResult("Region code must be a string");
        }

        if (!Regions.ValidRegionCodes.Contains(regionCode))
        {
            return new ValidationResult("Region must be a valid region code");
        }

        return ValidationResult.Success!;
    }
}