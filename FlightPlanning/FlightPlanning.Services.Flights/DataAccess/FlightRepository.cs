using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse.Exception;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public class FlightRepository : EntityFrameworkRepository<Flight>, IFlightRepository
    {
        public FlightRepository(FlightsDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return GetAll().Include(f => f.AirportDeparture)
                .Include(f => f.AirportDestination)
                .Include(f => f.Aircraft);
        }

        public Flight GetFlightById(int flightId)
        {
            if (flightId <= 0)
            {
                return null;
            }

            return GetAllFlights()
               .FirstOrDefault(e => e.Id == flightId);
        }

        public void InsertFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }

            if (flight.AirportDepartureId == flight.AirportDestinationId)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.SameDepartureAndDestinationAirportCode,
                    string.Format(ExceptionCodes.SameDepartureAndDestinationAirportFormatMessage, flight.AirportDepartureId, flight.AirportDestinationId));
            }

            flight.UpdateDate = DateTime.Now;

            var insertCount = Insert(flight);

            if (insertCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }

        public void UpdateFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }
            if (flight.AirportDepartureId == flight.AirportDestinationId)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.SameDepartureAndDestinationAirportCode,
                    string.Format(ExceptionCodes.SameDepartureAndDestinationAirportFormatMessage, flight.AirportDepartureId, flight.AirportDestinationId));
            }

            flight.UpdateDate = DateTime.Now;

            var updateCount = Update(flight);

            if (updateCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }

        public void DeleteFlight(int flightId)
        {
            if (flightId <= 0)
            {
                return;
            }

            var flightToDelete = GetById(flightId);

            if (flightToDelete == null)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.EntityToDeleteNotFoundCode,
                    string.Format(ExceptionCodes.EntityToDeleteNotFoundFormatMessage, nameof(Flight), flightId));
            }

            var deleteCount = Delete(flightToDelete);

            if (deleteCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }
    }
}
