using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Domain.Entities;
using Serilog;

namespace AdventureArchive.Api.Domain.Extensions;

public interface IValidatableEntity
{
}

public static class ValidationExtensions
{
    public static void Validate(this IValidatableEntity entity)
    {
        var context = new ValidationContext(entity);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(entity, context, results, true))
        {
            return;
        }

        var errors = string.Join(", ", results.Select(r => r.ErrorMessage));
        Log.Error("Validation error: {Errors}", errors);
        throw new ValidationException($"Validation failed: {errors}");
    }
}