using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public Aircraft Aircraft { get; set; }
        public Airport AirportDeparture { get; set; }
        public Airport AirportDestination { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
