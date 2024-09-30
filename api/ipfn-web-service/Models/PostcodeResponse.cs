using System.Text.Json.Serialization;

namespace ipfn_web_service.Models;

public class PostcodeResponse
{
    public int Status { get; set; }
    public Result Result { get; set; }
}

public class Result
{
    public string Postcode { get; set; }
    public int Quality { get; set; }
    public int Eastings { get; set; }
    public int Northings { get; set; }
    public string Country { get; set; }
    public string NhsHa { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    [JsonPropertyName("european_electoral_region")]
    public string EuropeanElectoralRegion { get; set; }
    [JsonPropertyName("primary_care_trust")]
    public string PrimaryCareTrust { get; set; }
    public string Region { get; set; }
    public string Lsoa { get; set; }
    public string Msoa { get; set; }
    public string Incode { get; set; }
    public string Outcode { get; set; }
    public string ParliamentaryConstituency { get; set; }
    public string ParliamentaryConstituency2024 { get; set; }
    public string AdminDistrict { get; set; }
    public string Parish { get; set; }
    public string AdminCounty { get; set; }
    public string DateOfIntroduction { get; set; }
    public string AdminWard { get; set; }
    public string Ced { get; set; }
    public string Ccg { get; set; }
    public string Nuts { get; set; }
    public string Pfa { get; set; }
    public Codes Codes { get; set; }
}

public class Codes
{
    [JsonPropertyName("admin_district")]
    public string AdminDistrict { get; set; }
    public string AdminCounty { get; set; }
    public string AdminWard { get; set; }
    public string Parish { get; set; }
    [JsonPropertyName("parliamentary_constituency")]
    public string ParliamentaryConstituency { get; set; }
    public string ParliamentaryConstituency2024 { get; set; }
    public string Ccg { get; set; }
    public string CcgId { get; set; }
    public string Ced { get; set; }
    public string Nuts { get; set; }
    public string Lsoa { get; set; }
    public string Msoa { get; set; }
    public string Lau2 { get; set; }
    public string Pfa { get; set; }
}