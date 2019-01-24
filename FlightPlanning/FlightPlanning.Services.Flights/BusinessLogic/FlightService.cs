using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
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
        private readonly ICalculator _calculator;

        public FlightService(IFlightRepository flightRepository, ICalculator calculator)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
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

        public DetailedFlightDto GetDetailedFlightDtoById(int flightId)
        {
            var flightDb = _flightRepository.GetFlightById(flightId);
            return DetailFlight(flightDb);
        }

        public IEnumerable<DetailedFlightDto> GetAllDetailedFlights()
        {
            var flights = _flightRepository.GetAllFlights();
            return flights?.Select(flight => DetailFlight(flight)).ToList(); ;
        }

        private DetailedFlightDto DetailFlight(Flight flightDb)
        {
            var flight = FlightMapper.MapToDto(flightDb);

            if (flight == null)
            {
                return null;
            }

            double distance = _calculator.CalculateDistanceBetweenAirports(flight.AirportDeparture, flight.AirportDestination);
            double neededFuel = _calculator.CalculateNeededFuel(distance, flight.Aircraft);

            return new DetailedFlightDto
            {
                Flight = flight,
                FlightPlan = new FlightPlanDto
                {
                    Distance = distance,
                    NeededFuel = neededFuel
                }
            };
        }

        
    }
}