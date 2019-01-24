using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Models
{
    public class DetailedFlight
    {
        public Flight Flight { get; set; }
        public FlightPlan FlightPlan { get; set; }
    }
}
