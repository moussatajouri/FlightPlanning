using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Mapper
{
    public class AircraftMapper
    {
        public static AircraftDto MapToDto(Aircraft aircraft)
        {
            if (aircraft == null)
            {
                return null;
            }

            return new AircraftDto
            {
                Id = aircraft.Id,
                Name = aircraft.Name,
                Speed = aircraft.Speed,
                FuelCapacity = aircraft.FuelCapacity,
                FuelConsumption = aircraft.FuelConsumption,
                TakeOffEffort = aircraft.TakeOffEffort
            };
        }

        public static Aircraft MapFromDto(AircraftDto aircraftDto)
        {
            if (aircraftDto == null)
            {
                return null;
            }

            return new Aircraft
            {
                Id = aircraftDto.Id,
                Name = aircraftDto.Name,
                Speed = aircraftDto.Speed,
                FuelCapacity = aircraftDto.FuelCapacity,
                FuelConsumption = aircraftDto.FuelConsumption,
                TakeOffEffort = aircraftDto.TakeOffEffort
            };
        }
    }
}
