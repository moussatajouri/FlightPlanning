using FlightPlanning.Services.Flights.BusinessLogic;
using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.BusinessLogic
{
    public class AirportServiceTests
    {
        [Fact]
        public void Should_AirportService_Throw_Exception_When_InjectedRepository_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AirportService(null));
        }

        [Fact]
        public void Should_GetAirportById_return_Null_When_AirportIsNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.GetAirportById(It.IsAny<int>())).Returns((Airport)null);

            var airportService = new AirportService(airportRepositoryMock.Object);

            var airport = airportService.GetAirportById(1);

            Assert.Null(airport);
            airportRepositoryMock.Verify(m => m.GetAirportById(It.IsAny<int>()),Times.Once);
        }

        [Fact]
        public void Should_GetAirportById_return_Dto_When_AirportIsNotNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

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

            airportRepositoryMock.Setup(m => m.GetAirportById(It.IsAny<int>())).Returns(airport);

            var airportService = new AirportService(airportRepositoryMock.Object);

            var airportDto = airportService.GetAirportById(1);

            airportRepositoryMock.Verify(m => m.GetAirportById(It.IsAny<int>()), Times.Once);

            Assert.Equal(airport.Id, airportDto.Id);
            Assert.Equal(airport.Name, airportDto.Name);
            Assert.Equal(airport.City, airportDto.City);
            Assert.Equal(airport.CountryName, airportDto.CountryName);
            Assert.Equal(airport.Iata, airportDto.Iata);
            Assert.Equal(airport.Icao, airportDto.Icao);
            Assert.Equal(airport.Latitude, airportDto.Latitude);
            Assert.Equal(airport.Longitude, airportDto.Longitude);
        }

        [Fact]
        public void Should_GetAllAirports_return_Null_When_AirportsIsNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.GetAllAirports()).Returns((IEnumerable<Airport>)null);

            var airportService = new AirportService(airportRepositoryMock.Object);

            var airports = airportService.GetAllAirports();

            Assert.Null(airports);
            airportRepositoryMock.Verify(m => m.GetAllAirports(), Times.Once);
        }

        [Fact]
        public void Should_GetAllAirports_return_EmptyList_When_AirportsIsEmpty()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.GetAllAirports()).Returns((new List<Airport>()));

            var airportService = new AirportService(airportRepositoryMock.Object);

            var airports = airportService.GetAllAirports();

            Assert.NotNull(airports);
            Assert.Empty(airports);
            airportRepositoryMock.Verify(m => m.GetAllAirports(), Times.Once);
        }

        [Fact]
        public void Should_GetAllAirports_return_List_When_AirportsIsNotEmpty()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.GetAllAirports()).Returns((new List<Airport> { new Airport(), new Airport()}));

            var airportService = new AirportService(airportRepositoryMock.Object);

            var airports = airportService.GetAllAirports();

            Assert.NotNull(airports);
            Assert.Equal(2, airports.Count());
            airportRepositoryMock.Verify(m => m.GetAllAirports(), Times.Once);
        }

        [Fact]
        public void Should_InsertAirport_DoesNotCall_InsertAirport_When_AirportsIsNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.InsertAirport(It.IsAny<Airport>())).Verifiable();

            var airportService = new AirportService(airportRepositoryMock.Object);

            airportService.InsertAirport(null);

            airportRepositoryMock.Verify(m => m.InsertAirport(It.IsAny<Airport>()), Times.Never);
        }

        [Fact]
        public void Should_InsertAirport_Call_InsertAirport_When_AirportsIsNotNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.InsertAirport(It.IsAny<Airport>())).Verifiable();

            var airportService = new AirportService(airportRepositoryMock.Object);

            airportService.InsertAirport(new AirportDto());

            airportRepositoryMock.Verify(m => m.InsertAirport(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void Should_UpdateAirport_DoesNotCallUpdateAirport_When_AirportsIsNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.UpdateAirport(It.IsAny<Airport>())).Verifiable();

            var airportService = new AirportService(airportRepositoryMock.Object);

            airportService.UpdateAirport(null);

            airportRepositoryMock.Verify(m => m.UpdateAirport(It.IsAny<Airport>()), Times.Never);
        }

        [Fact]
        public void Should_UpdateAirport_Call_UpdateAirport_When_AirportsIsNotNull()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.UpdateAirport(It.IsAny<Airport>())).Verifiable();

            var airportService = new AirportService(airportRepositoryMock.Object);

            airportService.UpdateAirport(new AirportDto());

            airportRepositoryMock.Verify(m => m.UpdateAirport(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void Should_DeleteAirport_Call_DeleteAirport()
        {
            var airportRepositoryMock = new Mock<IAirportRepository>();

            airportRepositoryMock.Setup(m => m.DeleteAirport(It.IsAny<int>())).Verifiable();

            var airportService = new AirportService(airportRepositoryMock.Object);

            airportService.DeleteAirport(2);

            airportRepositoryMock.Verify(m => m.DeleteAirport(It.IsAny<int>()), Times.Once);
        }
    }
}
