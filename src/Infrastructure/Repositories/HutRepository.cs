using System.ComponentModel.DataAnnotations;
using AdventureArchive.Api.Domain.Entities.Hut;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Domain.ValueObjects;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Serilog;

namespace AdventureArchive.Api.Infrastructure.Repositories;

public class HutRepository(IDocService docService) : IHutProvider
{
    private readonly IDocService _docService = docService ?? throw new ArgumentNullException(nameof(docService));

    public async Task<IEnumerable<Hut>> GetHutsAsync(Region? regionCode)
    {
        var hutsResponse = await _docService.GetHutsAsync(regionCode.ToString());
        return hutsResponse
            .Select(hutDto =>
            {
                try
                {
                    var hut = new Hut
                    {
                        AssetId = hutDto.AssetId,
                        Name = hutDto.Name,
                        Status = Enum.Parse<StatusEnum>(hutDto.Status),
                        Region = regionCode,
                        Location = Location.CreateLocation(hutDto.Lat, hutDto.Lon)
                    };
                    hut.Validate();
                    return hut;
                }
                catch (ValidationException ex)
                {
                    Log.Error(ex, "Validation error for hut with AssetId {AssetId}: {Message}", hutDto.AssetId,
                        ex.Message);
                    return null;
                }
            })
            .Where(hut => hut != null)!;
    }
}