using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public interface IAirportRepositoy
    {
        IEnumerable<Airport> GetAllAirports();

        Airport GetAirportById(int airportId);

        void InsertAirport(Airport airport);

        void UpdateAirport(Airport airport);

        void DeleteAirport(int airportId);
    }
}
