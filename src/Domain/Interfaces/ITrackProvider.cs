using AdventureArchive.Api.Domain.Entities;
using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface ITrackProvider
{
    public Task<IEnumerable<Track>> GetAllAsync(RegionEnum? regionCode);
}