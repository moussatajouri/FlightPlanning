using FlightPlanning.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.BusinessLogic
{
    public interface IAircraftService
    {
        Task<BasicResponse<IEnumerable<Aircraft>>> GetAllAircrafts();
        
        Task<BasicResponse<string>> InsertAircraft(Aircraft aircraft);

        Task<BasicResponse<string>> UpdateAircraft(Aircraft aircraft);

        Task<BasicResponse<string>> DeleteAircraft(int aircraftId);
    }
}
