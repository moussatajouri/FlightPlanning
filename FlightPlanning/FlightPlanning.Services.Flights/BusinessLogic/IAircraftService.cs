using FlightPlanning.Services.Flights.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public interface IAircraftService
    {
        IEnumerable<AircraftDto> GetAllAircrafts();

        AircraftDto GetAircraftById(int aircraftId);

        void InsertAircraft(AircraftDto aircraft);

        void UpdateAircraft(AircraftDto aircraft);

        void DeleteAircraft(int aircraftId);
    }
}
