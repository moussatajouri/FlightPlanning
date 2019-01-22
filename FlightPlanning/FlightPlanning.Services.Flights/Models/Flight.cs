using System;
using System.Collections.Generic;

namespace FlightPlanning.Services.Flights.Models
{
    public partial class Flight : BaseEntity
    {
        public int AirportDepartureId { get; set; }
        public int AirportDestinationId { get; set; }
        public int AircraftId { get; set; }
        public DateTime? UpdateDate { get; set; }

        public Aircraft Aircraft { get; set; }
        public Airport AirportDeparture { get; set; }
        public Airport AirportDestination { get; set; }
    }
}
