using System;
using System.Collections.Generic;

namespace FlightPlanning.Services.Flights.Models
{
    public partial class Airport : BaseEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
