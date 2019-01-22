using FlightPlanning.Services.Flights.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public interface ICalculator
    {
        double CalculateDistanceBetweenAirports(AirportDto airportDeparture, AirportDto airportDestination);

        double CalculateNeededFuel(double distance, AircraftDto aircraft);        
    }
}
