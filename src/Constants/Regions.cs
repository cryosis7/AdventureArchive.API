using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Constants;

public static class Regions
{
    public const string Northland = "NZ-NTL";
    public const string Auckland = "NZ-AUK";
    public const string Waikato = "NZ-WKO";
    public const string Coromandel = "DOC-COR";
    public const string BayOfPlenty = "NZ-BOP";
    public const string EastCoast = "NZ-GIS";
    public const string Taranaki = "NZ-TKI";
    public const string ManawatuWhanganui = "NZ-MWT";
    public const string CentralNorthIsland = "DOC-CNI";
    public const string HawkesBay = "NZ-HKB";
    public const string WellingtonKapiti = "NZ-WGN";
    public const string Wairarapa = "DOC-WPA";
    public const string ChathamIslands = "NZ-CIT";
    public const string NelsonTasman = "NZ-NSN";
    public const string Marlborough = "NZ-MBH";
    public const string WestCoast = "NZ-WTC";
    public const string Canterbury = "NZ-CAN";
    public const string Otago = "NZ-OTA";
    public const string Southland = "NZ-STL";
    public const string Fiordland = "DOC-FIL";

    public static readonly HashSet<string> ValidRegionCodes =
    [
        Northland,
        Auckland,
        Waikato,
        Coromandel,
        BayOfPlenty,
        EastCoast,
        Taranaki,
        ManawatuWhanganui,
        CentralNorthIsland,
        HawkesBay,
        WellingtonKapiti,
        Wairarapa,
        ChathamIslands,
        NelsonTasman,
        Marlborough,
        WestCoast,
        Canterbury,
        Otago,
        Southland,
        Fiordland
    ];

    public static readonly Dictionary<string, string> RegionNames = new()
    {
        { NelsonTasman, "Nelson/Tasman" },
        { WestCoast, "West Coast" },
        { Otago, "Otago" },
        { Canterbury, "Canterbury" },
        { Wairarapa, "Wairarapa" },
        { Marlborough, "Marlborough" },
        { Fiordland, "Fiordland" },
        { Taranaki, "Taranaki" },
        { CentralNorthIsland, "Central North Island" },
        { BayOfPlenty, "Bay of Plenty" },
        { HawkesBay, "Hawke's Bay" },
        { EastCoast, "East Coast" },
        { ManawatuWhanganui, "Manawatu/Whanganui" },
        { Southland, "Southland" },
        { Waikato, "Waikato" },
        { WellingtonKapiti, "Wellington/Kapiti" },
        { Northland, "Northland" },
        { Auckland, "Auckland" },
        { Coromandel, "Coromandel" }
    };
}