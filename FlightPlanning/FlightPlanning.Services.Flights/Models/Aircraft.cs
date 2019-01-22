using System;
using System.Collections.Generic;

namespace FlightPlanning.Services.Flights.Models
{
    public partial class Aircraft : BaseEntity
    {
        public Aircraft()
        {
            Flight = new HashSet<Flight>();
        }

        public string Name { get; set; }
        public double Speed { get; set; }
        public double FuelCapacity { get; set; }
        public double FuelConsumption { get; set; }
        public double TakeOffEffort { get; set; }

        public ICollection<Flight> Flight { get; set; }
    }
}
