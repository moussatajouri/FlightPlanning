using FlightPlanning.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.BusinessLogic
{
    public interface IAirportService
    {
        Task<BasicResponse<IEnumerable<Airport>>> GetAllAirports();
        
        Task<BasicResponse<string>> InsertAirport(Airport airport);

        Task<BasicResponse<string>> UpdateAirport(Airport airport);

        Task<BasicResponse<string>> DeleteAirport(int airportId);
    }
}
