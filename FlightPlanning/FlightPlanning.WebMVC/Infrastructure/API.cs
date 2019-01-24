using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Infrastructure
{
    public class API
    {
        public static class Airport
        {
            public static string GetAllAirport(string baseUri) => $"{baseUri}/airport/all";
            public static string CrudAirport(string baseUri) => $"{baseUri}/airport";
        }

        public static class Aircraft
        {
            public static string GetAllAircraft(string baseUri) => $"{baseUri}/aircraft/all";
            public static string CrudAircraft(string baseUri) => $"{baseUri}/aircraft";
        }

        public static class Flight
        {
            public static string GetAllFlight(string baseUri) => $"{baseUri}/flight/all";

            public static string GetAllFlightsDetails(string baseUri) => $"{baseUri}/flight/detail/all";

            public static string CrudFlight(string baseUri) => $"{baseUri}/flight";
        }
    }
}
