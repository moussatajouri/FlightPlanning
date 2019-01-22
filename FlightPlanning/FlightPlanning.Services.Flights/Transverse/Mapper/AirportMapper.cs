using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Mapper
{
    public static class AirportMapper
    {
        public static AirportDto MapToDto(Airport airport)
        {
            if (airport == null)
            {
                return null;
            }

            return new AirportDto
            {
                Id = airport.Id,
                Name = airport.Name,
                City = airport.City,
                CountryName = airport.CountryName,
                Iata = airport.Iata,
                Icao = airport.Icao,
                Latitude = airport.Latitude,
                Longitude = airport.Longitude
            };
        }

        public static Airport MapFromDto(AirportDto airportDto)
        {
            if (airportDto == null)
            {
                return null;
            }

            return new Airport
            {
                Id = airportDto.Id,
                Name = airportDto.Name,
                City = airportDto.City,
                CountryName = airportDto.CountryName,
                Iata = airportDto.Iata,
                Icao = airportDto.Icao,
                Latitude = airportDto.Latitude,
                Longitude = airportDto.Longitude
            };
        }
    }
}
