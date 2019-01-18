using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Exception
{
    public class FlightPlanningFunctionalException : FlightPlanningException
    {
        public FlightPlanningFunctionalException(string code, string message) 
            : base(code, message)
        {
        }
    }
}
