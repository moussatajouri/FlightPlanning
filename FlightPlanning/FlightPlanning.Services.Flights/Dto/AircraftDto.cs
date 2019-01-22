using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Dto
{
    public class AircraftDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Speed { get; set; }
        public double FuelCapacity { get; set; }
        public double FuelConsumption { get; set; }
        public double TakeOffEffort { get; set; }
    }
}
