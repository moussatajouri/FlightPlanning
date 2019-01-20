using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using FlightPlanning.Services.Flights.Transverse.Exception;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly IRepository<Aircraft> _aircraftDbContext;

        public AircraftRepository(IRepository<Aircraft> aircraftDbContext)
        {
            _aircraftDbContext = aircraftDbContext;
        }

        public IEnumerable<Aircraft> GetAllAircrafts()
        {
            return _aircraftDbContext.Table.AsEnumerable();
        }

        public Aircraft GetAircraftById(int aircraftId)
        {
            if (aircraftId <= 0)
            {
                return null;
            }

            return _aircraftDbContext.GetById(aircraftId);
        }

        public void InsertAircraft(Aircraft aircraft)
        {
            if (aircraft == null)
            {
                throw new ArgumentNullException(nameof(aircraft));
            }

            if (_aircraftDbContext.Table.Where(a => a.Name == aircraft.Name).Any())
            {
                throw new FlightPlanningFunctionalException(
                    string.Format(ExceptionCodes.InvalidEntityFormatCode, nameof(aircraft)),
                    ExceptionCodes.InvalidAircraftMessage);
            }

            var insertCount = _aircraftDbContext.Insert(aircraft);

            if (insertCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.InvalidAircraftMessage);
            }
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            if (aircraft == null)
            {
                throw new ArgumentNullException(nameof(aircraft));
            }

            if (_aircraftDbContext.Table.Where(a => a.Id != aircraft.Id && a.Name == aircraft.Name).Any())
            {
                throw new FlightPlanningFunctionalException(
                    string.Format(ExceptionCodes.InvalidEntityFormatCode, nameof(aircraft)),
                    ExceptionCodes.InvalidAircraftMessage);
            }

            var updateCount = _aircraftDbContext.Update(aircraft);

            if (updateCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.InvalidAircraftMessage);
            }
        }

        public void DeleteAircraft(int aircraftId)
        {
            if (aircraftId <= 0)
            {
                return;
            }

            var aircraftToDelete = _aircraftDbContext.GetById(aircraftId);

            if (aircraftToDelete == null)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.EntityToDeleteNotFoundCode,
                    string.Format(ExceptionCodes.EntityToDeleteNotFoundCode, nameof(Aircraft), aircraftId));
            }

            var deleteCount = _aircraftDbContext.Delete(aircraftToDelete);

            if (deleteCount <= 0)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.NoChangeCode, ExceptionCodes.InvalidAircraftMessage);
            }
        }
    }
}
