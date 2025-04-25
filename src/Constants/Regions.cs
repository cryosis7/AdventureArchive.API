namespace AdventureArchive.Api.Constants;

public static class Regions
{
    public const string Northland = "NZ-NTL";
    public const string Auckland = "NZ-AUK";
    public const string WaikatoCoromandel = "NZ-WKO";
    public const string Coromandel = "DOC-COR";
    public const string BayOfPlenty = "NZ-BOP";
    public const string EastCoast = "NZ-GIS";
    public const string Taranaki = "NZ-TKI";
    public const string ManawatuWhanganuiCentralNorthIsland = "NZ-MWT";
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
    public const string SouthlandFiordland = "NZ-STL";
    public const string Fiordland = "DOC-FIL";
    
    public static readonly HashSet<string> ValidRegionCodes =
    [
        Northland,
        Auckland,
        WaikatoCoromandel,
        Coromandel,
        BayOfPlenty,
        EastCoast,
        Taranaki,
        ManawatuWhanganuiCentralNorthIsland,
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
        SouthlandFiordland,
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
        { ManawatuWhanganuiCentralNorthIsland, "Manawatu/Whanganui" },
        { SouthlandFiordland, "Southland" },
        { WaikatoCoromandel, "Waikato" },
        { WellingtonKapiti, "Wellington/Kapiti" },
        { Northland, "Northland" },
        { Auckland, "Auckland" },
        { Coromandel, "Coromandel" }
    };
}