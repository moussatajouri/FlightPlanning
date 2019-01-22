using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Transverse.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }

        public FlightDto GetFlightById(int flightId)
        {
            var flight = _flightRepository.GetFlightById(flightId);
            return FlightMapper.MapToDto(flight);
        }

        public IEnumerable<FlightDto> GetAllFlights()
        {
            var flights = _flightRepository.GetAllFlights();
            return flights?.Select(flight => FlightMapper.MapToDto(flight)); ;
        }

        public void InsertFlight(FlightDto flight)
        {
            if (flight == null)
            {
                return;
            }

            _flightRepository.InsertFlight(FlightMapper.MapFromDto(flight));
        }

        public void UpdateFlight(FlightDto flight)
        {
            if (flight == null)
            {
                return;
            }

            _flightRepository.UpdateFlight(FlightMapper.MapFromDto(flight));
        }

        public void DeleteFlight(int flightId)
        {
            _flightRepository.DeleteFlight(flightId);
        }
    }
}