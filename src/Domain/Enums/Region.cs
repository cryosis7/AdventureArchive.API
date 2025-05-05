using System.Diagnostics.CodeAnalysis;

namespace AdventureArchive.Api.Domain.Enums;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Region
{
    NZ_NTL, // "Northland",
    NZ_AUK, // "Auckland",
    NZ_WKO, // "Waikato",
    DOC_COR, // "Coromandel",
    NZ_BOP, // "Bay of Plenty",
    NZ_GIS, // "East Coast",
    NZ_TKI, // "Taranaki",
    NZ_MWT, // "Manawatu/Whanganui",
    DOC_CNI, // "Central North Island",
    NZ_HKB, // "Hawke's Bay",
    NZ_WGN, // "Wellington/Kapiti",
    DOC_WPA, // "Wairarapa",
    NZ_CIT, // "Chatham Islands",
    NZ_NSN, // "Nelson/Tasman",
    NZ_MBH, // "Marlborough",
    NZ_WTC, // "West Coast",
    NZ_CAN, // "Canterbury",
    NZ_OTA, // "Otago",
    NZ_STL, // "Southland",
    DOC_FIL // "Fiordland",
}

public static class RegionExtensions
{
    public static string? GetName(this Region? region)
    {
        return region switch
        {
            Region.NZ_NTL => "Northland",
            Region.NZ_AUK => "Auckland",
            Region.NZ_WKO => "Waikato",
            Region.DOC_COR => "Coromandel",
            Region.NZ_BOP => "Bay of Plenty",
            Region.NZ_GIS => "East Coast",
            Region.NZ_TKI => "Taranaki",
            Region.NZ_MWT => "Manawatu/Whanganui",
            Region.DOC_CNI => "Central North Island",
            Region.NZ_HKB => "Hawke's Bay",
            Region.NZ_WGN => "Wellington/Kapiti",
            Region.DOC_WPA => "Wairarapa",
            Region.NZ_CIT => "Chatham Islands",
            Region.NZ_NSN => "Nelson/Tasman",
            Region.NZ_MBH => "Marlborough",
            Region.NZ_WTC => "West Coast",
            Region.NZ_CAN => "Canterbury",
            Region.NZ_OTA => "Otago",
            Region.NZ_STL => "Southland",
            Region.DOC_FIL => "Fiordland",
            _ => null
        };
    }
}