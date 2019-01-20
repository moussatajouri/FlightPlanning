﻿using System;
using System.Collections.Generic;

namespace FlightPlanning.Services.Flights.Models
{
    public partial class Aircraft : BaseEntity
    {
        public string Name { get; set; }
        public decimal Speed { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal FuelConsumption { get; set; }
        public decimal TakeOffEffort { get; set; }
    }
}
