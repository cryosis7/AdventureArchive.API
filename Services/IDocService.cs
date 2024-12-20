using AdventureArchive.Api.Models.Response;

namespace AdventureArchive.Api.Services
{
    public interface IDocService
    {
        Task<string> GetTracksAsync(string? regionCode);
        Task<DocHutsResponse> GetHutsAsync();
    }
}