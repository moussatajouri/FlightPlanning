using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public interface IAircraftRepository
    {
        IEnumerable<Aircraft> GetAllAircrafts();

        Aircraft GetAircraftById(int aircraftId);

        void InsertAircraft(Aircraft aircraft);

        void UpdateAircraft(Aircraft aircraft);

        void DeleteAircraft(int aircraftId);
    }
}
