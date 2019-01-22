using FlightPlanning.Services.Flights.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public interface IAirportService
    {
        IEnumerable<AirportDto> GetAllAirports();

        AirportDto GetAirportById(int airportId);

        void InsertAirport(AirportDto airport);

        void UpdateAirport(AirportDto airport);

        void DeleteAirport(int airportId);
    }
}
