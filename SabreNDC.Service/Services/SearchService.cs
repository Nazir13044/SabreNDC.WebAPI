﻿using SabreNDC.Application.Dtos;
using SabreNDC.Application.Services;
using System.Numerics;
using TripLover.AirCommonModels;

namespace SabreNDC.Service.Services
{
    public class SearchService : ISearchService
    {
        public async Task<string> Search(ACMSearchReq searchRequest)
        {
            //Making SabreNDC Request
            #region NDC Request

            BFMRequest bFMRequest = new BFMRequest();
            bFMRequest = await ACMSearchReqToBFMRequest(searchRequest);

            #endregion


            return await Task.FromResult("searchText");
        }
        private async Task<BFMRequest> ACMSearchReqToBFMRequest(ACMSearchReq searchRequest)
        {
            BFMRequest bFMRequest = new BFMRequest();
            try
            {
                #region SabreNDC request

                #region CabinClass
                string? cabinClass;
                switch (searchRequest.CabinClass.ToLower())
                {
                    case "economy":
                        cabinClass = "Y";
                        break;

                    case "business":
                        cabinClass = "C";
                        break;

                    case "premium economy":
                        cabinClass = "S";
                        break;

                    case "premiumeconomy":
                        cabinClass = "S";
                        break;

                    case "premium business":
                        cabinClass = "J";
                        break;
                    case "premiumbusiness":
                        cabinClass = "J";
                        break;

                    case "first":
                        cabinClass = "F";
                        break;

                    case "premium first":
                        cabinClass = "P";
                        break;
                    case "premiumfirst":
                        cabinClass = "P";
                        break;

                    default:
                        cabinClass = "Y";
                        break;
                }
                #endregion

                #region OriginDestination

                List<OriginDestinationInformation> OriginDestinationInformations = new List<OriginDestinationInformation>();
                int countRoute = 0;

                foreach (var route in searchRequest.Routes)
                {
                    OriginDestinationInformations.Add(new OriginDestinationInformation()
                    {
                        DepartureDateTime = Convert.ToDateTime(route.DepartureDate).ToString("yyyy-MM-ddT00:00:00"),
                        OriginLocation = new OriginLocation
                        {
                            LocationCode = route.Origin
                        },
                        DestinationLocation = new DestinationLocation
                        {
                            LocationCode = route.Destination,
                        },
                        RPH = countRoute.ToString(),
                    });
                    countRoute++;
                }

                #endregion

                #region TravelPreferences
                TravelPreferences travelPreferences = new TravelPreferences()
                {
                    CabinPref = new List<CabinPref>()
                {
                    new CabinPref()
                    {
                        Cabin = cabinClass,
                        PreferLevel = searchRequest.CabinClass.ToLower().Equals("all") ? "Preferred" : "Only",  //[Only, Unacceptable, Preferred]
                    }
                },
                    TPA_Extensions = new TPAExtensions()
                    {
                        NumTrips = new NumTrips()
                        {
                            //Number = 10,
                        },
                        DataSources = new DataSources()
                        {
                            NDC = "Enable",
                            ATPCO = "Disable",
                            LCC = "Disable"
                        },
                        PreferNDCSourceOnTie = new PreferNDCSourceOnTie()
                        {
                            Value = true
                        },
                        NDCIndicators = new NDCIndicators()
                        {
                            MultipleBrandedFares = new MultipleBrandedFares()
                            {
                                Value = false
                            },
                            MaxNumberOfUpsells = new MaxNumberOfUpsells()
                            {
                                Value = 6
                            }
                        }
                    }
                };
                #endregion

                #region Passenger Quantity

                List<PassengerTypeQuantity> paxTypeQuantity = new List<PassengerTypeQuantity>();

                if (searchRequest.Adults > 0)
                    paxTypeQuantity.Add(new PassengerTypeQuantity()
                    {
                        Code = "ADT",
                        Quantity = searchRequest.Adults,
                    });
                if (searchRequest.Childs > 0)
                {
                    if (searchRequest.ChildrenAges != null && searchRequest.ChildrenAges.Count == searchRequest.Childs)
                    {
                        int cnnCount = searchRequest.ChildrenAges.Where(x => x < 5).Count();

                        if (cnnCount > 0)
                        {
                            paxTypeQuantity.Add(new PassengerTypeQuantity()
                            {
                                Code = "C03",
                                Quantity = cnnCount,
                            });
                        }
                        if (searchRequest.Childs - cnnCount > 0)
                        {

                            paxTypeQuantity.Add(new PassengerTypeQuantity()
                            {
                                Code = "C06",
                                Quantity = searchRequest.Childs - cnnCount,
                            });
                        }
                    }
                }
                if (searchRequest.Infants > 0)
                {
                    paxTypeQuantity.Add(new PassengerTypeQuantity()
                    {
                        Code = "INF",
                        Quantity = searchRequest.Infants,
                    });
                }

                #endregion

                #region AddPasenger

                TravelerInfoSummary travelerInfoSummary = new TravelerInfoSummary()
                {
                    AirTravelerAvail = new List<AirTravelerAvail>()
                {
                    new AirTravelerAvail()
                    {
                        PassengerTypeQuantity=paxTypeQuantity
                    }
                }
                };

                #endregion

                #region making bfm search request
                bFMRequest = new BFMRequest()
                {
                    OTA_AirLowFareSearchRQ = new OTAAirLowFareSearchRQ()
                    {
                        Version = "4",

                        POS = new POS()
                        {
                            Source = new List<Source>()
                            {
                                new Source()
                                {
                                    PseudoCityCode = "0U5I",//searchRequest.ApiCredential.PCC,
                                    RequestorID = new RequestorID()
                                    {
                                        ID = "1",
                                        Type = "1",
                                        CompanyName = new CompanyName()
                                        {
                                            Code = "TN"
                                        }
                                    }
                                }
                            }
                        },
                        OriginDestinationInformation = new List<OriginDestinationInformation>(OriginDestinationInformations),
                        TravelPreferences = travelPreferences,
                        TravelerInfoSummary = travelerInfoSummary,
                        TPA_Extensions = new TPAExtensions()
                        {
                            IntelliSellTransaction = new IntelliSellTransaction()
                            {
                                RequestType = new RequestType()
                                {
                                    Name = searchRequest.ApiCredential.PCC
                                }
                            }
                        }
                    }
                };
                #endregion

                #endregion
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return bFMRequest;
        }
    }
}
