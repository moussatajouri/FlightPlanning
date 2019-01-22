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
    }
}
