using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface IDocLandmarkRepository : ILandmarkRepository
{
    Task<IEnumerable<IDocLandmark>> GetByRegion(RegionEnum region);
}