using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Transverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.BusinessLogic
{
    public class Calculator : ICalculator
    {
        private readonly IDistanceCalculator _distanceCalculator;
        private IDistanceCalculator distanceCalculator;

        public Calculator(IDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator)); ;
        }

        public double CalculateDistanceBetweenAirports(AirportDto airportDeparture, AirportDto airportDestination)
        {
            if (airportDeparture == null || airportDestination == null)
            {
                throw new ArgumentNullException($"{nameof(airportDeparture)} OR {nameof(airportDestination)}");
            }

            if (!airportDeparture.Latitude.HasValue ||
                !airportDeparture.Longitude.HasValue ||
                !airportDestination.Latitude.HasValue ||
                !airportDestination.Longitude.HasValue)
            {
                throw new ArgumentNullException($"airportDeparture.Latitude: {airportDeparture.Latitude}, airportDeparture.Longitude: {airportDeparture.Longitude}, airportDestination.Latitude: {airportDestination.Latitude}, airportDestination.Longitude: {airportDestination.Longitude}");
            }

            return _distanceCalculator.HaversineInKM(
                airportDeparture.Latitude.Value,
                airportDeparture.Longitude.Value,
                airportDestination.Latitude.Value,
                airportDestination.Longitude.Value);
        }

        public double CalculateNeededFuel(double distance, AircraftDto aircraft)
        {
            if (aircraft == null)
            {
                throw new ArgumentNullException(nameof(aircraft));
            }

            if(distance == 0 || aircraft.Speed==0)
            {
                return 0;
            }

            var flightDuration = distance / aircraft.Speed;

            return (aircraft.FuelConsumption * distance * flightDuration) + aircraft.TakeOffEffort;
        }

    }
}
