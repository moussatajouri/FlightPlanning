using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.Transverse
{
    public class AircraftMapperTests
    {
        #region MapToDto

        [Fact]
        public void Should_MapToDto_Return_Null_When_EntityIsNull()
        {
            Assert.Null(AircraftMapper.MapToDto(null));
        }

        [Fact]
        public void Should_MapToDto_Map_EntityCorrectly()
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

            var aircraftDto = AircraftMapper.MapToDto(aircraft);

            Assert.Equal(aircraft.Id, aircraftDto.Id);
            Assert.Equal(aircraft.Name, aircraftDto.Name);
            Assert.Equal(aircraft.Speed, aircraftDto.Speed);
            Assert.Equal(aircraft.FuelCapacity, aircraftDto.FuelCapacity);
            Assert.Equal(aircraft.FuelConsumption, aircraftDto.FuelConsumption);
            Assert.Equal(aircraft.TakeOffEffort, aircraftDto.TakeOffEffort);
        }

        #endregion MapToDto

        #region MapFromDto

        [Fact]
        public void Should_MapFromDto_Return_Null_When_EntityIsNull()
        {
            Assert.Null(AircraftMapper.MapFromDto(null));
        }

        [Fact]
        public void Should_MapFromDto_Map_DtoCorrectly()
        {
            var aircraftDto = new AircraftDto
            {
                Id = 5,
                Name = "Name",
                Speed = 500,
                FuelCapacity = 1000,
                FuelConsumption = 60,
                TakeOffEffort = 10
            };

            var aircraft = AircraftMapper.MapFromDto(aircraftDto);

            Assert.Equal(aircraftDto.Id, aircraft.Id);
            Assert.Equal(aircraftDto.Name, aircraft.Name);
            Assert.Equal(aircraftDto.Speed, aircraft.Speed);
            Assert.Equal(aircraftDto.FuelCapacity, aircraft.FuelCapacity);
            Assert.Equal(aircraftDto.FuelConsumption, aircraft.FuelConsumption);
            Assert.Equal(aircraftDto.TakeOffEffort, aircraft.TakeOffEffort);

        }

        #endregion MapFromDto
    }
}
