using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Factories;

public class LandmarkFactory : ILandmarkFactory
{
    public ILandmark CreateLandmark(LandmarkCreationParameters parameters)
    {
        return parameters.LandmarkType switch
        {
            LandmarkType.Hut => new HutLandmark(parameters.DocId ?? "", parameters.Name,
                new Location(parameters.Latitude, parameters.Longitude),
                parameters.Description),
            LandmarkType.Campsite => new CampsiteLandmark(parameters.DocId ?? "", parameters.Name,
                new Location(parameters.Latitude, parameters.Longitude),
                parameters.Description),
            _ => throw new ArgumentOutOfRangeException(
                nameof(parameters.LandmarkType), parameters.LandmarkType, null)
        };
    }
}