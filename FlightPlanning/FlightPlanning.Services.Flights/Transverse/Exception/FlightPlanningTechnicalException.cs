using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Exception
{
    public class FlightPlanningTechnicalException : FlightPlanningException
    {

        public FlightPlanningTechnicalException(string code, System.Exception innerException)
           : this(code, string.Empty, innerException)
        {
        }

        public FlightPlanningTechnicalException(string code, string message, System.Exception innerException)
           : base(code, message, innerException)
        {
        }

        public FlightPlanningTechnicalException(string code, string message)
          : base(code, message)
        {
        }
    }
}
