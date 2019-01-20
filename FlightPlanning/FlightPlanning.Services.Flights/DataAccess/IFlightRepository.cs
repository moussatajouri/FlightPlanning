using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> GetAllFlights();

        Flight GetFlightById(int flightId);

        void InsertFlight(Flight flight);

        void UpdateFlight(Flight flight);

        void DeleteFlight(int flightId);
    }
}
