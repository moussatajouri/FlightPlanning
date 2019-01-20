using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using FlightPlanning.Services.Flights.Transverse.Exception;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public class AirportRepository : EntityFrameworkRepository<Airport>, IAirportRepository
    {
        public AirportRepository(FlightsDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Airport> GetAllAirports()
        {
            return GetAll();
        }

        public Airport GetAirportById(int airportId)
        {
            if (airportId <= 0)
            {
                return null;
            }

            return GetById(airportId);
        }

        public void InsertAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            var insertCount = Insert(airport);

            if (insertCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }

        public void UpdateAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            var updateCount = Update(airport);

            if (updateCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }

        public void DeleteAirport(int airportId)
        {
            if (airportId <= 0)
            {
                return;
            }

            var airportToDelete = GetById(airportId);

            if (airportToDelete == null)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.EntityToDeleteNotFoundCode,
                    string.Format(ExceptionCodes.EntityToDeleteNotFoundFormatMessage, nameof(Airport), airportId));
            }

            var deleteCount = Delete(airportToDelete);

            if (deleteCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }
    }
}
