using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface ICampsiteRepository : ILandmarkRepository
{
    Task<IEnumerable<ILandmark>> GetByRegion(RegionEnum region);
}
