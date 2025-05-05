using AdventureArchive.Api.Domain.Entities.Hut;
using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface IHutProvider
{
    public Task<IEnumerable<Hut>> GetHutsAsync(Region? regionCode);
}