using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Mapper
{
    public class FlightMapper
    {
        public static FlightDto MapToDto(Flight flight)
        {
            if (flight == null)
            {
                return null;
            }

            return new FlightDto
            {
                Id = flight.Id,
                Aircraft = AircraftMapper.MapToDto(flight.Aircraft),
                AirportDeparture = AirportMapper.MapToDto(flight.AirportDeparture),
                AirportDestination = AirportMapper.MapToDto(flight.AirportDestination),
                UpdateDate = flight.UpdateDate
            };
        }

        public static Flight MapFromDto(FlightDto flightDto)
        {
            if (flightDto == null)
            {
                return null;
            }

            return new Flight
            {
                Id = flightDto.Id,
                Aircraft = AircraftMapper.MapFromDto(flightDto.Aircraft),
                AirportDeparture = AirportMapper.MapFromDto(flightDto.AirportDeparture),
                AirportDestination = AirportMapper.MapFromDto(flightDto.AirportDestination),
                UpdateDate = flightDto.UpdateDate
            };
        }
    }
}
