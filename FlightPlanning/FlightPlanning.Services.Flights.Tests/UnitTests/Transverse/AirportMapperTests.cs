using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.Transverse
{
    public class AirportMapperTests
    {
        #region MapToDto

        [Fact]
        public void Should_MapToDto_Return_Null_When_EntityIsNull()
        {
            Assert.Null(AirportMapper.MapToDto(null));
        }

        [Fact]
        public void Should_MapToDto_Map_EntityCorrectly()
        {
            var airport = new Airport
            {
                Id = 99,
                Name = "Name",
                City = "City",
                CountryName = "CountryName",
                Iata = "Iata",
                Icao = "Icao",
                Latitude = 5,
                Longitude = 9
            };

            var airportDto = AirportMapper.MapToDto(airport);

            Assert.Equal(airport.Id, airportDto.Id);
            Assert.Equal(airport.Name, airportDto.Name);
            Assert.Equal(airport.City, airportDto.City);
            Assert.Equal(airport.CountryName, airportDto.CountryName);
            Assert.Equal(airport.Iata, airportDto.Iata);
            Assert.Equal(airport.Icao, airportDto.Icao);
            Assert.Equal(airport.Latitude, airportDto.Latitude);
            Assert.Equal(airport.Longitude, airportDto.Longitude);

        }

        #endregion MapToDto

        #region MapFromDto

        [Fact]
        public void Should_MapFromDto_Return_Null_When_EntityIsNull()
        {
            Assert.Null(AirportMapper.MapFromDto(null));
        }

        [Fact]
        public void Should_MapFromDto_Map_DtoCorrectly()
        {
            var airportDto = new AirportDto
            {
                Id = 99,
                Name = "Name",
                City = "City",
                CountryName = "CountryName",
                Iata = "Iata",
                Icao = "Icao",
                Latitude = 5,
                Longitude = 9
            };

            var airport = AirportMapper.MapFromDto(airportDto);

            Assert.Equal(airportDto.Id, airport.Id);
            Assert.Equal(airportDto.Name, airport.Name);
            Assert.Equal(airportDto.City, airport.City);
            Assert.Equal(airportDto.CountryName, airport.CountryName);
            Assert.Equal(airportDto.Iata, airport.Iata);
            Assert.Equal(airportDto.Icao, airport.Icao);
            Assert.Equal(airportDto.Latitude, airport.Latitude);
            Assert.Equal(airportDto.Longitude, airport.Longitude);

        }

        #endregion MapFromDto
    }
}
