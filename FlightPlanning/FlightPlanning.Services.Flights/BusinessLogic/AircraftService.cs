using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Transverse.Exception;
using FlightPlanning.Services.Flights.Transverse.Mapper;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public class AircraftService : IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;

        public AircraftService(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
        }

        public AircraftDto GetAircraftById(int aircraftId)
        {
            var aircraft = _aircraftRepository.GetAircraftById(aircraftId);
            return AircraftMapper.MapToDto(aircraft);
        }
        
        public IEnumerable<AircraftDto> GetAllAircrafts()
        {
            var aircrafts = _aircraftRepository.GetAllAircrafts();
            return aircrafts?.Select(aircraft => AircraftMapper.MapToDto(aircraft)); ;
        }

        public void InsertAircraft(AircraftDto aircraft)
        {
            if(aircraft == null)
            {
                return;
            }

            ValidateAircraft(aircraft);

            _aircraftRepository.InsertAircraft(AircraftMapper.MapFromDto(aircraft));
        }

        public void UpdateAircraft(AircraftDto aircraft)
        {
            if (aircraft == null)
            {
                return;
            }

            ValidateAircraft(aircraft);

            _aircraftRepository.UpdateAircraft(AircraftMapper.MapFromDto(aircraft));
        }

        public void DeleteAircraft(int aircraftId)
        {
            _aircraftRepository.DeleteAircraft(aircraftId);
        }

        private void ValidateAircraft(AircraftDto aircraft)
        {
            if(string.IsNullOrEmpty(aircraft.Name) || aircraft.FuelCapacity <= 0 || aircraft.FuelConsumption <= 0 || aircraft.Speed <= 0 || aircraft.TakeOffEffort <= 0)
            {
                throw new FlightPlanningFunctionalException(ExceptionCodes.InvalidEntityCode, ExceptionCodes.InvalidAircraftMessage);
            }
        }
    }
}
