using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Infrastructure
{
    public class ApiConfiguration
    {
        public string AirportApiBasePath { get; set; }
        public string AircraftApiBasePath { get; set; }
        public string FlightApiBasePath { get; set; }
    }
}
