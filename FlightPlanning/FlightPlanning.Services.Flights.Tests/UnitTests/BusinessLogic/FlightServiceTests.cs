using FlightPlanning.Services.Flights.BusinessLogic;
using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.BusinessLogic
{
    public class FlightServiceTests
    {
        [Fact]
        public void Should_FlightService_Throw_Exception_When_InjectedRepository_IsNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            Assert.Throws<ArgumentNullException>(() => new FlightService(null, calculatorMock.Object));
        }

        [Fact]
        public void Should_FlightService_Throw_Exception_When_InjectedCalculator_IsNull()
        {
            var flightRepositoryMock = new Mock<IFlightRepository>();
            Assert.Throws<ArgumentNullException>(() => new FlightService(flightRepositoryMock.Object, null));
        }

        [Fact]
        public void Should_GetFlightById_return_Null_When_FlightIsNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.GetFlightById(It.IsAny<int>())).Returns((Flight)null);

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var flight = flightService.GetFlightById(1);

            Assert.Null(flight);
            flightRepositoryMock.Verify(m => m.GetFlightById(It.IsAny<int>()),Times.Once);
        }

        [Fact]
        public void Should_GetFlightById_return_Dto_When_FlightIsNotNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            var date = new DateTime(2017, 10, 12);
            var flight = new Flight
            {
                Id = 1,
                Aircraft = new Aircraft { Id=2},
                AirportDeparture = new Airport { Id = 3},
                AirportDestination= new Airport { Id = 4 },
                UpdateDate = date
            };

            flightRepositoryMock.Setup(m => m.GetFlightById(It.IsAny<int>())).Returns(flight);

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var flightDto = flightService.GetFlightById(1);

            flightRepositoryMock.Verify(m => m.GetFlightById(It.IsAny<int>()), Times.Once);

            Assert.Equal(flight.Id, flightDto.Id);
            Assert.NotNull(flight.Aircraft);
            Assert.Equal(flight.Aircraft.Id, flightDto.Aircraft.Id);
            Assert.NotNull(flight.AirportDeparture);
            Assert.Equal(flight.AirportDeparture.Id, flightDto.AirportDeparture.Id);
            Assert.NotNull(flight.AirportDestination);
            Assert.Equal(flight.AirportDestination.Id, flightDto.AirportDestination.Id);
            Assert.Equal(date, flightDto.UpdateDate);
        }

        [Fact]
        public void Should_GetAllFlights_return_Null_When_FlightsIsNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.GetAllFlights()).Returns((IEnumerable<Flight>)null);

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var flights = flightService.GetAllFlights();

            Assert.Null(flights);
            flightRepositoryMock.Verify(m => m.GetAllFlights(), Times.Once);
        }

        [Fact]
        public void Should_GetAllFlights_return_EmptyList_When_FlightsIsEmpty()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.GetAllFlights()).Returns((new List<Flight>()));

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var flights = flightService.GetAllFlights();

            Assert.NotNull(flights);
            Assert.Empty(flights);
            flightRepositoryMock.Verify(m => m.GetAllFlights(), Times.Once);
        }

        [Fact]
        public void Should_GetAllFlights_return_List_When_FlightsIsNotEmpty()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.GetAllFlights()).Returns((new List<Flight> { new Flight(), new Flight()}));

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var flights = flightService.GetAllFlights();

            Assert.NotNull(flights);
            Assert.Equal(2, flights.Count());
            flightRepositoryMock.Verify(m => m.GetAllFlights(), Times.Once);
        }

        [Fact]
        public void Should_InsertFlight_DoesNotCall_InsertFlight_When_FlightsIsNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.InsertFlight(It.IsAny<Flight>())).Verifiable();

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            flightService.InsertFlight(null);

            flightRepositoryMock.Verify(m => m.InsertFlight(It.IsAny<Flight>()), Times.Never);
        }

        [Fact]
        public void Should_InsertFlight_Call_InsertFlight_When_FlightsIsNotNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.InsertFlight(It.IsAny<Flight>())).Verifiable();

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            flightService.InsertFlight(new FlightDto());

            flightRepositoryMock.Verify(m => m.InsertFlight(It.IsAny<Flight>()), Times.Once);
        }

        [Fact]
        public void Should_UpdateFlight_DoesNotCallUpdateFlight_When_FlightsIsNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.UpdateFlight(It.IsAny<Flight>())).Verifiable();

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            flightService.UpdateFlight(null);

            flightRepositoryMock.Verify(m => m.UpdateFlight(It.IsAny<Flight>()), Times.Never);
        }

        [Fact]
        public void Should_UpdateFlight_Call_UpdateFlight_When_FlightsIsNotNull()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.UpdateFlight(It.IsAny<Flight>())).Verifiable();

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            flightService.UpdateFlight(new FlightDto());

            flightRepositoryMock.Verify(m => m.UpdateFlight(It.IsAny<Flight>()), Times.Once);
        }

        [Fact]
        public void Should_DeleteFlight_Call_DeleteFlight()
        {
            var calculatorMock = new Mock<ICalculator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();

            flightRepositoryMock.Setup(m => m.DeleteFlight(It.IsAny<int>())).Verifiable();

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            flightService.DeleteFlight(2);

            flightRepositoryMock.Verify(m => m.DeleteFlight(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_GetDetailedFlightDtoById_Return_Null_When_FlightIsNotFound()
        {
            var calculatorMock = new Mock<ICalculator>();
            calculatorMock.Setup(m => m.CalculateDistanceBetweenAirports(It.IsAny<AirportDto>(), It.IsAny<AirportDto>())).Verifiable();
            calculatorMock.Setup(m => m.CalculateNeededFuel(It.IsAny<double>(), It.IsAny<AircraftDto>())).Verifiable();

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(m => m.GetFlightById(It.IsAny<int>())).Returns((Flight)null);

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var detailedFlightDtoResult = flightService.GetDetailedFlightDtoById(10);

            Assert.Null(detailedFlightDtoResult);

            flightRepositoryMock.Verify(m => m.GetFlightById(It.IsAny<int>()), Times.Once);
            calculatorMock.Verify(m => m.CalculateDistanceBetweenAirports(It.IsAny<AirportDto>(), It.IsAny<AirportDto>()), Times.Never);
            calculatorMock.Verify(m => m.CalculateNeededFuel(It.IsAny<double>(), It.IsAny<AircraftDto>()), Times.Never);
        }

        [Fact]
        public void Should_GetDetailedFlightDtoById_Return_DetailedFlightDto_When_FlightIsFound()
        {
            var expectedFlightId = 77;
            double expectedDistance = 999.99;
            var expectedNeededFuel = 111.11;

            var calculatorMock = new Mock<ICalculator>();
            calculatorMock.Setup(m => m.CalculateDistanceBetweenAirports(It.IsAny<AirportDto>(), It.IsAny<AirportDto>())).Returns(expectedDistance);
            calculatorMock.Setup(m => m.CalculateNeededFuel(It.IsAny<double>(), It.IsAny<AircraftDto>())).Returns(expectedNeededFuel);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(m => m.GetFlightById(It.IsAny<int>())).Returns(new Flight { Id = expectedFlightId });

            var flightService = new FlightService(flightRepositoryMock.Object, calculatorMock.Object);

            var detailedFlightDtoResult = flightService.GetDetailedFlightDtoById(10);

            Assert.NotNull(detailedFlightDtoResult);

            Assert.NotNull(detailedFlightDtoResult.Flight);
            Assert.Equal(expectedFlightId, detailedFlightDtoResult.Flight.Id);

            Assert.NotNull(detailedFlightDtoResult.FlightPlan);
            Assert.Equal(expectedDistance, detailedFlightDtoResult.FlightPlan.Distance);
            Assert.Equal(expectedNeededFuel, detailedFlightDtoResult.FlightPlan.NeededFuel);

            flightRepositoryMock.Verify(m => m.GetFlightById(It.IsAny<int>()), Times.Once);
            calculatorMock.Verify(m => m.CalculateDistanceBetweenAirports(It.IsAny<AirportDto>(), It.IsAny<AirportDto>()), Times.Once);
            calculatorMock.Verify(m => m.CalculateNeededFuel(It.IsAny<double>(), It.IsAny<AircraftDto>()), Times.Once);
        }
    }
}
