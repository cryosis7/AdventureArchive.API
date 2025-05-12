using AdventureArchive.Api.Domain.Entities.Landmark;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface ILandmarkRepository
{
    ILandmark GetById(Guid id);
    Task<IEnumerable<ILandmark>> GetAllAsync();
    // IEnumerable<ILandmark> GetLandmarksByType<T>() where T : class, ILandmark;
    // IEnumerable<ILandmark> GetByName(string name);
}