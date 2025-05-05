using System.Diagnostics.CodeAnalysis;

namespace AdventureArchive.Api.Domain.Enums;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum RegionEnum
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
    public static string GetName(this RegionEnum region)
    {
        return region switch
        {
            RegionEnum.NZ_NTL => "Northland",
            RegionEnum.NZ_AUK => "Auckland",
            RegionEnum.NZ_WKO => "Waikato",
            RegionEnum.DOC_COR => "Coromandel",
            RegionEnum.NZ_BOP => "Bay of Plenty",
            RegionEnum.NZ_GIS => "East Coast",
            RegionEnum.NZ_TKI => "Taranaki",
            RegionEnum.NZ_MWT => "Manawatu/Whanganui",
            RegionEnum.DOC_CNI => "Central North Island",
            RegionEnum.NZ_HKB => "Hawke's Bay",
            RegionEnum.NZ_WGN => "Wellington/Kapiti",
            RegionEnum.DOC_WPA => "Wairarapa",
            RegionEnum.NZ_CIT => "Chatham Islands",
            RegionEnum.NZ_NSN => "Nelson/Tasman",
            RegionEnum.NZ_MBH => "Marlborough",
            RegionEnum.NZ_WTC => "West Coast",
            RegionEnum.NZ_CAN => "Canterbury",
            RegionEnum.NZ_OTA => "Otago",
            RegionEnum.NZ_STL => "Southland",
            RegionEnum.DOC_FIL => "Fiordland",
            _ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
        };
    }
    
    public static string? GetName(this RegionEnum? region)
    {
        return region == null ? null : GetName(region.Value);
    }
    
    public static RegionEnum? ToRegionEnum(this string? region)
    {
        return region == null ? null : Enum.TryParse<RegionEnum>(region, out var regionEnum) ? regionEnum : null;
    }
}