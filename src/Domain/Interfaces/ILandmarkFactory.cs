using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Factories;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface ILandmarkFactory
{
    ILandmark CreateLandmark(LandmarkCreationParameters parameters);
}