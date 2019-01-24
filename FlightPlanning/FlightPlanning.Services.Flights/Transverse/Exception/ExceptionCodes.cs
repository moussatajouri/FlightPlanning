using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Exception
{
    public class ExceptionCodes
    {
        public static readonly string EntityFrameworkRepository = "DataAccess_Exception";

        public static readonly string InvalidEntityCode = "invalid_entity_data";

        public static readonly string NoChangeCode = "invalid_airport_data";
        public static readonly string NoChangeMessage = "No change was persisted";

        public static readonly string EntityToDeleteNotFoundCode = "Entity_To_Delete_Is_Not_Found";
        public static readonly string EntityToDeleteNotFoundFormatMessage = "The {0} entity with identifier {1} is not found";

        public static readonly string SameDepartureAndDestinationAirportCode = "Same_Airport_Departure_And_Airport_Destination";
        public static readonly string SameDepartureAndDestinationAirportFormatMessage = "The airport of departure and destination must be different. Departure Id={0}. Destination Id={1}";

        public static readonly string FlightNullArgumentsCode = "Flight_Null_Arguments";
        public static readonly string FlightNullArgumentsMessage = "The departure and destination airport and aircraft can't be null.";

        public static string InvalidAircraftMessage = "Invalid Aircraft: Name can't be null or empty and FuelCapacity, FuelConsumption , Speed, TakeOffEffort must be positive values.";
        public static string InvalidAirportMessage = "Invalid Airport : - Name and CountryName can't be null or empty. - Iata = 3 characters - Icao 4 characters - Latitude valid range [-180,180]; - Longitude valid range [-90,90]";
    }
}
