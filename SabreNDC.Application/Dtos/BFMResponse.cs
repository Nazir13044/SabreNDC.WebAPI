using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos;

// BFMResponse myDeserializedClass = JsonConvert.DeserializeObject<BFMResponse>(myJsonResponse);
public class BFMResponse
{
    public GroupedItineraryResponse groupedItineraryResponse { get; set; }
}
public class Allowance
{
    public int @ref { get; set; }
}

public class Arrival
{
    public string airport { get; set; }
    public string time { get; set; }
    public string terminal { get; set; }
    public int? dateAdjustment { get; set; }
}

public class BaggageAllowanceDesc
{
    public int id { get; set; }
    public int weight { get; set; }
    public string unit { get; set; }
}

public class BaggageInformation
{
    public string provisionType { get; set; }
    public string airlineCode { get; set; }
    public List<Segment> segments { get; set; }
    public Allowance allowance { get; set; }
}

public class Brand
{
    public string code { get; set; }
    public string brandName { get; set; }
    public int priceClassDescriptionRef { get; set; }
}

public class Carrier
{
    public string marketing { get; set; }
    public int marketingFlightNumber { get; set; }
    public string operating { get; set; }
    public int operatingFlightNumber { get; set; }
    public string codeShared { get; set; }
    public Equipment equipment { get; set; }
}

public class Departure
{
    public string airport { get; set; }
    public string time { get; set; }
    public string terminal { get; set; }
}

public class Description
{
    public string id { get; set; }
    public string text { get; set; }
}

public class Equipment
{
    public string code { get; set; }
}

public class Fare
{
    public string offerItemId { get; set; }
    public bool mandatoryInd { get; set; }
    public string serviceId { get; set; }
    public string validatingCarrierCode { get; set; }
    public bool eTicketable { get; set; }
    public List<PassengerInfoList> passengerInfoList { get; set; }
    public TotalFare totalFare { get; set; }
}

public class FareComponent
{
    public int @ref { get; set; }
    public string beginAirport { get; set; }
    public string endAirport { get; set; }
    public List<Segment> segments { get; set; }
}

public class FareComponentDesc
{
    public int id { get; set; }
    public string fareBasisCode { get; set; }
    public string farePassengerType { get; set; }
    public string fareDescription { get; set; }
    public List<Segment> segments { get; set; }
    public Brand brand { get; set; }
}

public class GroupDescription
{
    public List<LegDescription> legDescriptions { get; set; }
}

public class GroupedItineraryResponse
{
    public string version { get; set; }
    public List<Message> messages { get; set; }
    public Statistics statistics { get; set; }
    public List<ScheduleDesc> scheduleDescs { get; set; }
    public List<FareComponentDesc> fareComponentDescs { get; set; }
    public List<BaggageAllowanceDesc> baggageAllowanceDescs { get; set; }
    public List<LegDesc> legDescs { get; set; }
    public List<PassengerDesc> passengerDescs { get; set; }
    public List<PriceClassDescription> priceClassDescriptions { get; set; }
    public List<ItineraryGroup> itineraryGroups { get; set; }
}

public class Itinerary
{
    public int id { get; set; }
    public string pricingSource { get; set; }
    public List<Leg> legs { get; set; }
    public List<PricingInformation> pricingInformation { get; set; }
}

public class ItineraryGroup
{
    public GroupDescription groupDescription { get; set; }
    public List<Itinerary> itineraries { get; set; }
}

public class Leg
{
    public int @ref { get; set; }
}

public class LegDesc
{
    public int id { get; set; }
    public int elapsedTime { get; set; }
    public List<Schedule> schedules { get; set; }
}

public class LegDescription
{
    public string departureDate { get; set; }
    public string departureLocation { get; set; }
    public string arrivalLocation { get; set; }
}

public class Message
{
    public string severity { get; set; }
    public string type { get; set; }
    public string code { get; set; }
    public string text { get; set; }
    public string value { get; set; }
    public string shortCode { get; set; }
}

public class Offer
{
    public string offerId { get; set; }
    public int timeToLive { get; set; }
    public string source { get; set; }
}

public class Passenger
{
    public int @ref { get; set; }
}

public class PassengerDesc
{
    public int id { get; set; }
    public string passengerId { get; set; }
}

public class PassengerInfo
{
    public string passengerType { get; set; }
    public int passengerNumber { get; set; }
    public List<FareComponent> fareComponents { get; set; }
    public PassengerTotalFare passengerTotalFare { get; set; }
    public List<BaggageInformation> baggageInformation { get; set; }
    public List<Passenger> passengers { get; set; }
}

public class PassengerInfoList
{
    public PassengerInfo passengerInfo { get; set; }
}

public class PassengerTotalFare
{
    public double totalFare { get; set; }
    public double totalTaxAmount { get; set; }
    public string currency { get; set; }
    public double baseFareAmount { get; set; }
    public string baseFareCurrency { get; set; }
    public double equivalentAmount { get; set; }
    public string equivalentCurrency { get; set; }
}

public class PriceClassDescription
{
    public int id { get; set; }
    public List<Description> descriptions { get; set; }
}

public class PricingInformation
{
    public string pricingSubsource { get; set; }
    public Offer offer { get; set; }
    public Fare fare { get; set; }
}

public class Schedule
{
    public int @ref { get; set; }
    public int? departureDateAdjustment { get; set; }
}

public class ScheduleDesc
{
    public int id { get; set; }
    public int stopCount { get; set; }
    public bool eTicketable { get; set; }
    public int elapsedTime { get; set; }
    public Departure departure { get; set; }
    public Arrival arrival { get; set; }
    public Carrier carrier { get; set; }
}

public class Segment
{
    public Segment segment { get; set; }
    public int id { get; set; }
}

public class Segment2
{
    public string bookingCode { get; set; }
    public string cabinCode { get; set; }
    public int seatsAvailable { get; set; }
}

public class Statistics
{
    public int itineraryCount { get; set; }
}

public class TotalFare
{
    public double totalPrice { get; set; }
    public double totalTaxAmount { get; set; }
    public string currency { get; set; }
    public double baseFareAmount { get; set; }
    public string baseFareCurrency { get; set; }
    public double equivalentAmount { get; set; }
    public string equivalentCurrency { get; set; }
}


