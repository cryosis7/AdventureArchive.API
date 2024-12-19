using AdventureArchive.Api.Constants;
using AdventureArchive.Api.Models;
using AdventureArchive.Api.Models.Response;
using AdventureArchive.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdventureArchive.Api.Controllers;

[ApiController]
[Route("api/doc")]
public class DocController(DocService docService) : ControllerBase
{
    /// <summary>
    /// Gets tracks for a region from the DOC api
    /// </summary>
    /// <param name="regionCode">The region code to get tracks for.</param>
    /// <returns>
    /// 200 OK if the region code is valid, otherwise 400 Bad Request.
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
    public async Task<IActionResult> GetTracks([FromQuery] string? regionCode)
    {
        try
        {
            if (regionCode != null && !Regions.ValidRegionCodes.Contains(regionCode))
            {
                return BadRequest("Invalid region code.");
            }
            var tracksData = await docService.GetTracksAsync(regionCode);
            return Ok(tracksData);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { message = "Error calling DOC API.", details = ex.Message });
        }
    }
    
    /// <summary>
    /// Gets all huts from the DOC API
    /// </summary>
    /// <returns>
    /// 200 OK with hut data if successful.
    /// 500 Internal Server Error if there is an error calling the DOC API.
    /// </returns>
    [HttpGet("huts")]
    public async Task<IActionResult> GetHuts()
    {
        try
        {
            return Ok(await docService.GetHutsAsync());
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { message = "Error calling DOC API.", details = ex.Message });
        }
    }
}