using AdventureArchive.Api.Api.Models.Doc.Huts;
using AdventureArchive.Api.Api.Models.Doc.Tracks;
using AdventureArchive.Api.Constants;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AdventureArchive.Api.Api.Controllers;

[ApiController]
[Route("api/doc")]
public class DocController(IDocService docService) : ControllerBase
{
    /// <summary>
    /// Gets tracks for a region from the DOC api
    /// </summary>
    /// <param name="regionCode">The region code to get tracks for.</param>
    /// <returns>
    /// 200 OK with track data if successful.
    /// </returns>
    /// <remarks>
    /// **Valid Region Codes:**
    /// - NZ-NTL: Northland
    /// - NZ-AUK: Auckland
    /// - NZ-WKO: Waikato + Coromandel
    /// - DOC-COR: Coromandel
    /// - NZ-BOP: Bay of Plenty
    /// - NZ-GIS: East Coast
    /// - NZ-TKI: Taranaki
    /// - NZ-MWT: Manawatu/Whanganui + Central North Island
    /// - DOC-CNI: Central North Island
    /// - NZ-HKB: Hawke's Bay
    /// - NZ-WGN: Wellington/Kapiti
    /// - DOC-WPA: Wairarapa
    /// - NZ-CIT: Chatham Islands
    /// - NZ-NSN: Nelson/Tasman
    /// - NZ-MBH: Marlborough
    /// - NZ-WTC: West Coast
    /// - NZ-CAN: Canterbury
    /// - NZ-OTA: Otago
    /// - NZ-STL: Southland + Fiordland
    /// - DOC-FIL: Fiordland
    /// </remarks>
    [HttpGet("tracks")]
    [ProducesResponseType(typeof(GetTracksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetTracksResponse>> GetTracks([FromQuery] string? regionCode)
    {
        if (regionCode != null && !Regions.ValidRegionCodes.Contains(regionCode))
        {
            Log.Information("Invalid region code {RegionCode} provided", regionCode);
            return BadRequest("Invalid region code.");
        }

        var tracksData = await docService.GetTracksAsync(regionCode);
        return Ok(new GetTracksResponse(tracksData));
    }

    /// <summary>
    /// Gets all huts from the DOC API
    /// </summary>
    /// <returns>
    /// 200 OK with hut data if successful.
    /// 500 Internal Server Error
    /// </returns>
    [HttpGet("huts")]
    [ProducesResponseType(typeof(GetHutsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetHutsResponse>> GetHuts()
    {
        var hutsData = await docService.GetHutsAsync();
        return Ok(new GetHutsResponse(hutsData));
    }
}