using FlightPlanning.Services.Flights.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public interface IFlightService
    {
        IEnumerable<FlightDto> GetAllFlights();        

        FlightDto GetFlightById(int flightId);        

        void InsertFlight(FlightDto flight);

        void UpdateFlight(FlightDto flight);

        void DeleteFlight(int flightId);

        IEnumerable<DetailedFlightDto> GetAllDetailedFlights();

        DetailedFlightDto GetDetailedFlightDtoById(int flightId);
    }
}
