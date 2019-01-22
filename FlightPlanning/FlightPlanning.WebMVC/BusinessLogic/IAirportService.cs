using FlightPlanning.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.BusinessLogic
{
    public interface IAirportService
    {
        Task<IEnumerable<Airport>> GetAllAirports();

        //Airport GetAirportById(int airportId);

        void InsertAirport(Airport airport);

        void UpdateAirport(Airport airport);

        void DeleteAirport(int airportId);
    }
}
