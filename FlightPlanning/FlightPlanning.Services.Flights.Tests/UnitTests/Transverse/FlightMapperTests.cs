using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.Transverse
{
    public class FlightMapperTests
    {
        #region MapToDto

        [Fact]
        public void Should_MapToDto_Return_Null_When_EntityIsNull()
        {
            Assert.Null(FlightMapper.MapToDto(null));
        }

        [Fact]
        public void Should_MapToDto_Map_EntityCorrectly_With_Null_Proprities()
        {
            var flight = new Flight
            {
                Id = 99,
                Aircraft = null,
                AirportDeparture = null,
                AirportDestination = null,
                UpdateDate = null
            };

            var flightDto = FlightMapper.MapToDto(flight);

            Assert.Equal(flight.Id, flightDto.Id);
            Assert.Null(flight.Aircraft);
            Assert.Null(flight.AirportDeparture);
            Assert.Null(flight.AirportDestination);
            Assert.Null(flight.UpdateDate);
        }

        [Fact]
        public void Should_MapToDto_Map_EntityCorrectly_With_Proprities()
        {
            var aircraft = new Aircraft
            {
                Id = 5,
                Name = "Name",
                Speed = 500,
                FuelCapacity = 1000,
                FuelConsumption = 60,
                TakeOffEffort = 10
            };

            var airportDeparture = new Airport
            {
                Id = 1,
                Name = "airportDeparture",
                City = "airportDeparture",
                CountryName = "airportDeparture",
                Iata = "airportDeparture",
                Icao = "airportDeparture",
                Latitude = 5,
                Longitude = 9
            };

            var airportDestination = new Airport
            {
                Id = 2,
                Name = "airportDestination",
                City = "airportDestination",
                CountryName = "airportDestination",
                Iata = "airportDestination",
                Icao = "airportDestination",
                Latitude = 16,
                Longitude = 10
            };

            var date = new DateTime(2018, 3, 2, 1, 1, 1);

            var flight = new Flight
            {
                Id = 99,
                Aircraft = aircraft,
                AirportDeparture = airportDeparture,
                AirportDestination = airportDestination,
                UpdateDate = date
            };

            var flightDto = FlightMapper.MapToDto(flight);

            Assert.Equal(flight.Id, flightDto.Id);
            Assert.NotNull(flight.UpdateDate);
            Assert.Equal(date,flight.UpdateDate);

            Assert.NotNull(flight.Aircraft);
            Assert.Equal(aircraft.Id, flight.Aircraft.Id);
            Assert.Equal(aircraft.Name, flight.Aircraft.Name);
            Assert.Equal(aircraft.Speed, flight.Aircraft.Speed);
            Assert.Equal(aircraft.FuelCapacity, flight.Aircraft.FuelCapacity);
            Assert.Equal(aircraft.FuelConsumption, flight.Aircraft.FuelConsumption);
            Assert.Equal(aircraft.TakeOffEffort, flight.Aircraft.TakeOffEffort);

            Assert.NotNull(flight.AirportDeparture);
            Assert.Equal(airportDeparture.Id, flight.AirportDeparture.Id);
            Assert.Equal(airportDeparture.Name, flight.AirportDeparture.Name);
            Assert.Equal(airportDeparture.City, flight.AirportDeparture.City);
            Assert.Equal(airportDeparture.CountryName, flight.AirportDeparture.CountryName);
            Assert.Equal(airportDeparture.Iata, flight.AirportDeparture.Iata);
            Assert.Equal(airportDeparture.Icao, flight.AirportDeparture.Icao);
            Assert.Equal(airportDeparture.Latitude, flight.AirportDeparture.Latitude);
            Assert.Equal(airportDeparture.Longitude, flight.AirportDeparture.Longitude);

            Assert.NotNull(flight.AirportDestination);
            Assert.Equal(airportDestination.Id, flight.AirportDestination.Id);
            Assert.Equal(airportDestination.Name, flight.AirportDestination.Name);
            Assert.Equal(airportDestination.City, flight.AirportDestination.City);
            Assert.Equal(airportDestination.CountryName, flight.AirportDestination.CountryName);
            Assert.Equal(airportDestination.Iata, flight.AirportDestination.Iata);
            Assert.Equal(airportDestination.Icao, flight.AirportDestination.Icao);
            Assert.Equal(airportDestination.Latitude, flight.AirportDestination.Latitude);
            Assert.Equal(airportDestination.Longitude, flight.AirportDestination.Longitude);
        }

        #endregion MapToDto

        #region MapFromDto

        [Fact]
        public void Should_MapFromDto_Return_Null_When_EntityIsNull()
        {
            Assert.Null(FlightMapper.MapFromDto(null));
        }

        [Fact]
        public void Should_MapFromDto_Map_EntityCorrectly_With_Null_Proprities()
        {
            var flightDto = new FlightDto
            {
                Id = 99,
                Aircraft = null,
                AirportDeparture = null,
                AirportDestination = null,
                UpdateDate = null
            };

            var flight = FlightMapper.MapFromDto(flightDto);

            Assert.Equal(flight.Id, flightDto.Id);
            Assert.Null(flight.Aircraft);
            Assert.Null(flight.AirportDeparture);
            Assert.Null(flight.AirportDestination);
            Assert.Null(flight.UpdateDate);
        }

        [Fact]
        public void Should_MapFromDto_Map_DtoCorrectly_With_Proprities()
        {
            var aircraftDto = new AircraftDto
            {
                Id = 5
            };

            var airportDepartureDto = new AirportDto
            {
                Id = 1
            };

            var airportDestinationDto = new AirportDto
            {
                Id = 2
            };

            var date = new DateTime(2018, 3, 2, 1, 1, 1);

            var flightDto = new FlightDto
            {
                Id = 99,
                Aircraft = aircraftDto,
                AirportDeparture = airportDepartureDto,
                AirportDestination = airportDestinationDto,
                UpdateDate = date
            };

            var flight = FlightMapper.MapFromDto(flightDto);

            Assert.Equal(flightDto.Id, flight.Id);
            Assert.NotNull(flight.UpdateDate);
            Assert.Equal(date, flight.UpdateDate);

            Assert.Equal(aircraftDto.Id, flight.AircraftId);
            Assert.Equal(airportDepartureDto.Id, flight.AirportDepartureId);
            Assert.Equal(airportDestinationDto.Id, flight.AirportDestinationId);
        }

        #endregion MapFromDto
    }
}
