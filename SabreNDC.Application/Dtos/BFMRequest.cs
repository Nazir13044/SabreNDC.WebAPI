using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos;

// BFMRequest myDeserializedClass = JsonConvert.DeserializeObject<BFMRequest>(myJsonResponse);
public sealed class BFMRequest
{
    public OTAAirLowFareSearchRQ? OTA_AirLowFareSearchRQ { get; set; }
}
public class AirTravelerAvail
{
    public List<PassengerTypeQuantity>? PassengerTypeQuantity { get; set; }
}

public class CabinPref
{
    public string? Cabin { get; set; }
    public string? PreferLevel { get; set; }
}

public class CompanyName
{
    public string? Code { get; set; }
}

public class DataSources
{
    public string? NDC { get; set; }
    public string? ATPCO { get; set; }
    public string? LCC { get; set; }
}

public class DestinationLocation
{
    public string? LocationCode { get; set; }
}

public class IntelliSellTransaction
{
    public RequestType? RequestType { get; set; }
}

public class MaxNumberOfUpsells
{
    public int Value { get; set; }
}

public class MultipleBrandedFares
{
    public bool Value { get; set; }
}

public class NDCIndicators
{
    public MultipleBrandedFares? MultipleBrandedFares { get; set; }
    public MaxNumberOfUpsells? MaxNumberOfUpsells { get; set; }
}

public class NumTrips
{
    public int Number { get; set; }
}

public class OriginDestinationInformation
{
    public string? RPH { get; set; }
    public string? DepartureDateTime { get; set; }
    public OriginLocation? OriginLocation { get; set; }
    public DestinationLocation? DestinationLocation { get; set; }
}

public class OriginLocation
{
    public string? LocationCode { get; set; }
}

public class OTAAirLowFareSearchRQ
{
    public string? Version { get; set; }
    public POS? POS { get; set; }
    public List<OriginDestinationInformation>? OriginDestinationInformation { get; set; }
    public TravelPreferences? TravelPreferences { get; set; }
    public TravelerInfoSummary? TravelerInfoSummary { get; set; }
    public TPAExtensionsV2? TPA_Extensions { get; set; }
}

public class PassengerTypeQuantity
{
    public string? Code { get; set; }
    public int Quantity { get; set; }
}

public class POS
{
    public List<Source>? Source { get; set; }
}

public class PreferNDCSourceOnTie
{
    public bool Value { get; set; }
}

public class RequestorID
{
    public string? Type { get; set; }
    public string? ID { get; set; }
    public CompanyName? CompanyName { get; set; }
}

public class RequestType
{
    public string? Name { get; set; }
}

public class Source
{
    public string? PseudoCityCode { get; set; }
    public RequestorID? RequestorID { get; set; }
}

public class TPAExtensions
{
    public NumTrips? NumTrips { get; set; }
    public DataSources? DataSources { get; set; }
    public PreferNDCSourceOnTie? PreferNDCSourceOnTie { get; set; }
    public NDCIndicators? NDCIndicators { get; set; }
}
public class TPAExtensionsV2
{
    public IntelliSellTransaction? IntelliSellTransaction { get; set; }
}
public class TravelerInfoSummary
{
    public List<AirTravelerAvail>? AirTravelerAvail { get; set; }
}

public class TravelPreferences
{
    public List<CabinPref>? CabinPref { get; set; }
    public TPAExtensions? TPA_Extensions { get; set; }
}

