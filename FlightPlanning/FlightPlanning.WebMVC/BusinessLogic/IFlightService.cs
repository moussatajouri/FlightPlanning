using FlightPlanning.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.BusinessLogic
{
    public interface IFlightService
    {
        Task<BasicResponse<IEnumerable<Flight>>> GetAllFlights();

        Task<BasicResponse<IEnumerable<DetailedFlight>>> GetAllFlightsDetails();

        Task<BasicResponse<string>> InsertFlight(Flight flight);

        Task<BasicResponse<string>> UpdateFlight(Flight flight);

        Task<BasicResponse<string>> DeleteFlight(int flightId);
    }
}
