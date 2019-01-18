using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Exception
{
    public class ExceptionCodes
    {
        public static readonly string EntityFrameworkRepository = "DataAccess_Exception";

        public static readonly string InvalidAirportCode = "invalid_airport_data";
        public static readonly string InvalidAirportMessage = "Already exists airport with same Iata, Icao or Name proprieties. They must be unique";

        public static readonly string NoChangeCode = "invalid_airport_data";
        public static readonly string NoChangeMessage = "No change was persisted";

        public static readonly string EntityToDeleteNotFoundCode = "Entity_To_Delete_Is_Not_Found";
        public static readonly string EntityToDeleteNotFoundFormatMessage = "The {0} entity with identifier {1} is not found";

    }
}
