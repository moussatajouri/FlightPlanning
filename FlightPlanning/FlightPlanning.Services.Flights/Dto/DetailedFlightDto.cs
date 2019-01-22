using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Dto
{
    public class DetailedFlightDto
    {
        public FlightDto Flight { get; set; }
        public FlightPlanDto FlightPlan { get; set; }
    }
}
