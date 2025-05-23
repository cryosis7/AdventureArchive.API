using AdventureArchive.Api.Api.Models.Doc;
using AdventureArchive.Api.Api.Models.Doc.Huts;
using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AdventureArchive.Api.Api.Controllers;

[ApiController]
[Route("api/doc")]
public class DocController(IHutRepository hutRepository, ICampsiteRepository campsiteRepository) : ControllerBase
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
    // [HttpGet("tracks")]
    // [ProducesResponseType(typeof(GetTracksResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult<GetTracksResponse>> GetTracks([FromQuery] string? regionCode)
    // {
    //     if (regionCode != null && regionCode.ToRegionEnum() == null)
    //     {
    //         Log.Information("Invalid region code {RegionCode} provided", regionCode);
    //         return BadRequest("Invalid region code.");
    //     }
    //
    //     var tracksData = await trackProvider.GetAllAsync(regionCode.ToRegionEnum());
    //     return Ok(new GetTracksResponse(tracksData));
    // }
    [HttpGet("huts")]
    [ProducesResponseType(typeof(GetHutsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetHutsResponse>> GetHuts([FromQuery] string? regionCode)
    {
        var regionEnum = regionCode?.ToRegionEnum();
        if (regionCode != null && regionEnum == null)
        {
            Log.Information("Invalid region code {RegionCode} provided", regionCode);
            return BadRequest("Invalid region code.");
        }

        var hutsData = regionEnum == null
            ? await hutRepository.GetAllAsync()
            : await hutRepository.GetByRegion(regionEnum.Value);
        return Ok(new GetHutsResponse(hutsData.Cast<HutLandmark>()));
    }
    
    [HttpGet]
    [Route("campsites")]
    [ProducesResponseType(typeof(GetCampsitesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetCampsitesResponse>> GetCampsites([FromQuery] string? regionCode)
    {
        var regionEnum = regionCode?.ToRegionEnum();
        if (regionCode != null && regionEnum == null)
        {
            Log.Information("Invalid region code {RegionCode} provided", regionCode);
            return BadRequest("Invalid region code.");
        }

        var campsitesData = regionEnum == null
            ? await campsiteRepository.GetAllAsync()
            : await campsiteRepository.GetByRegion(regionEnum.Value);
        return Ok(new GetCampsitesResponse(campsitesData.Cast<CampsiteLandmark>()));
    }
}