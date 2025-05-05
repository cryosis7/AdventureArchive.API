using AdventureArchive.Api.Domain.Entities.Trip;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface ITripProvider //TODO: Finish implementing, create a database provider
{
    Task AddAsync(ITrip trip);
    Task<ITrip?> GetByIdAsync(Guid id);
    Task<IEnumerable<ITrip>> GetAllAsync();
}
