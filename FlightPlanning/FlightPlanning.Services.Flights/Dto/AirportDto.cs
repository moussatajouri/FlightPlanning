using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Dto
{
    public class AirportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
