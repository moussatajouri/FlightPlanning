using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Exception
{
    public abstract class FlightPlanningException : System.Exception
    {
        public string Code { get; }

        public FlightPlanningException(string code, string message) 
            : base(message)
        {
            Code = code;
        }
        
        public FlightPlanningException(string code, string message, System.Exception innerException)
           : base(message, innerException)
        {
            Code = code;
        }
    }
}
