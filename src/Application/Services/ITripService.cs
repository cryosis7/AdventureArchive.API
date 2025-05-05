namespace AdventureArchive.Api.Application.Services;

public interface ITripService
{
    Task<Guid> CreateTripAsync(string name, DateTime startDate, DateTime? endDate, TimeSpan? duration);
}