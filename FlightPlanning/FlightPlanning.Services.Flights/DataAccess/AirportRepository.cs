using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using FlightPlanning.Services.Flights.Transverse.Exception;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public class AirportRepository : IAirportRepository
    {
        private readonly IRepository<Airport> _airportDbContext;

        public AirportRepository(IRepository<Airport> airportDbContext)
        {
            _airportDbContext = airportDbContext;
        }

        public IEnumerable<Airport> GetAllAirports()
        {
            return _airportDbContext.Table.AsEnumerable();
        }

        public Airport GetAirportById(int airportId)
        {
            if (airportId <= 0)
            {
                return null;
            }

            return _airportDbContext.GetById(airportId);
        }

        public void InsertAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            if (_airportDbContext.Table.Where(a => a.Iata == airport.Iata || a.Icao == airport.Icao || a.Name == airport.Name).Any())
            {
                throw new FlightPlanningFunctionalException(
                    string.Format(ExceptionCodes.InvalidEntityFormatCode, nameof(airport)),
                    ExceptionCodes.InvalidAirportMessage);
            }

            var insertCount = _airportDbContext.Insert(airport);

            if (insertCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.InvalidAirportMessage);
            }
        }

        public void UpdateAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            if (_airportDbContext.Table.Where(a => a.Id != airport.Id && (a.Iata == airport.Iata || a.Icao == airport.Icao || a.Name == airport.Name)).Any())
            {
                throw new FlightPlanningFunctionalException(
                    string.Format(ExceptionCodes.InvalidEntityFormatCode, nameof(airport)),
                    ExceptionCodes.InvalidAirportMessage);
            }

            var updateCount = _airportDbContext.Update(airport);

            if (updateCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.InvalidAirportMessage);
            }
        }

        public void DeleteAirport(int airportId)
        {
            if (airportId <= 0)
            {
                return;
            }

            var airportToDelete = _airportDbContext.GetById(airportId);

            if (airportToDelete == null)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.EntityToDeleteNotFoundCode,
                    string.Format(ExceptionCodes.EntityToDeleteNotFoundCode, nameof(Airport), airportId));
            }

            var deleteCount = _airportDbContext.Delete(airportToDelete);

            if (deleteCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.InvalidAirportMessage);
            }
        }
    }
}
