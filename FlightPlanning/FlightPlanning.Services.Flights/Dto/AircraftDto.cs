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
        public decimal Speed { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal FuelConsumption { get; set; }
        public decimal TakeOffEffort { get; set; }
    }
}
