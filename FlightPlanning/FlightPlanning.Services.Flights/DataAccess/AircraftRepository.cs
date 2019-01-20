using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using FlightPlanning.Services.Flights.Transverse.Exception;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public class AircraftRepository : EntityFrameworkRepository<Aircraft>, IAircraftRepository
    {
        public AircraftRepository(FlightsDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Aircraft> GetAllAircrafts()
        {
            return GetAll();
        }

        public Aircraft GetAircraftById(int aircraftId)
        {
            if (aircraftId <= 0)
            {
                return null;
            }

            return GetById(aircraftId);
        }

        public void InsertAircraft(Aircraft aircraft)
        {
            if (aircraft == null)
            {
                throw new ArgumentNullException(nameof(aircraft));
            }

            var insertCount = Insert(aircraft);

            if (insertCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            if (aircraft == null)
            {
                throw new ArgumentNullException(nameof(aircraft));
            }

            var updateCount = Update(aircraft);

            if (updateCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }

        public void DeleteAircraft(int aircraftId)
        {
            if (aircraftId <= 0)
            {
                return;
            }

            var aircraftToDelete = GetById(aircraftId);

            if (aircraftToDelete == null)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.EntityToDeleteNotFoundCode,
                    string.Format(ExceptionCodes.EntityToDeleteNotFoundFormatMessage, nameof(Aircraft), aircraftId));
            }

            var deleteCount = Delete(aircraftToDelete);

            if (deleteCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.NoChangeMessage);
            }
        }
    }
}
