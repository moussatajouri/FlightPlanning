using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Transverse.Mapper;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;

        public AirportService(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository ?? throw new ArgumentNullException(nameof(airportRepository));
        }

        public AirportDto GetAirportById(int airportId)
        {
            var airport = _airportRepository.GetAirportById(airportId);
            return AirportMapper.MapToDto(airport);
        }
        
        public IEnumerable<AirportDto> GetAllAirports()
        {
            var airports = _airportRepository.GetAllAirports();
            return airports?.Select(airport => AirportMapper.MapToDto(airport)); ;
        }

        public void InsertAirport(AirportDto airport)
        {
            if(airport == null)
            {
                return;
            }

            _airportRepository.InsertAirport(AirportMapper.MapFromDto(airport));
        }

        public void UpdateAirport(AirportDto airport)
        {
            if (airport == null)
            {
                return;
            }

            _airportRepository.UpdateAirport(AirportMapper.MapFromDto(airport));
        }

        public void DeleteAirport(int airportId)
        {
            _airportRepository.DeleteAirport(airportId);
        }
    }
}
