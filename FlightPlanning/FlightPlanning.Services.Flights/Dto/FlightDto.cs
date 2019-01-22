using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Dto
{
    public class FlightDto
    {
        public int Id { get; set; }
        public AircraftDto Aircraft { get; set; }
        public AirportDto AirportDeparture { get; set; }
        public AirportDto AirportDestination { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
