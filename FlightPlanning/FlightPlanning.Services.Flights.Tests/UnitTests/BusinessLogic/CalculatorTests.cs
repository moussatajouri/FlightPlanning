using FlightPlanning.Services.Flights.BusinessLogic;
using FlightPlanning.Services.Flights.Dto;
using FlightPlanning.Services.Flights.Transverse;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.BusinessLogic
{
    public class CalculatorTests
    {

        [Fact]
        public void Should_CalculateDistanceBetweenAirports_Throw_Exception_When_InjectedDistanceCalculator_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Calculator(null));
        }

        [Fact]
        public void Should_CalculateDistanceBetweenAirports_Throw_Exception_When_AirportDeparture_IsNull()
        {
            var distanceCalculatorMock = new Mock<IDistanceCalculator>();
            distanceCalculatorMock.Setup(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Verifiable();

            Assert.Throws<ArgumentNullException>(() => new Calculator(distanceCalculatorMock.Object).CalculateDistanceBetweenAirports(null, new AirportDto()));

            distanceCalculatorMock.Verify(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Should_CalculateDistanceBetweenAirports_Throw_Exception_When_AirportDestination_IsNull()
        {
            var distanceCalculatorMock = new Mock<IDistanceCalculator>();
            distanceCalculatorMock.Setup(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Verifiable();

            Assert.Throws<ArgumentNullException>(() => new Calculator(distanceCalculatorMock.Object).CalculateDistanceBetweenAirports(new AirportDto(), null));

            distanceCalculatorMock.Verify(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Theory]
        [InlineData(null, 1, 1, 1)]
        [InlineData(1, null, 1, 1)]
        [InlineData(1, 1, null, 1)]
        [InlineData(1, 1, 1, null)]
        public void Should_CalculateDistanceBetweenAirports_Throw_Exception_When_GpsCoordinate_AreMissing(double? latitude_1, double? longitude_1, double? latitude_2, double? longitude_2)
        {
            var distanceCalculatorMock = new Mock<IDistanceCalculator>();
            distanceCalculatorMock.Setup(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Verifiable();

            var airportDeparture = new AirportDto { Latitude = latitude_1, Longitude = longitude_1 };
            var airportDestination = new AirportDto { Latitude = latitude_2, Longitude = longitude_2 };

            Assert.Throws<ArgumentNullException>(() => new Calculator(distanceCalculatorMock.Object).CalculateDistanceBetweenAirports(airportDeparture, airportDestination));

            distanceCalculatorMock.Verify(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Should_CalculateDistanceBetweenAirports_CalculateDistance_One()
        {
            var distanceCalculatorMock = new Mock<IDistanceCalculator>();
            var expectedDistance = 10015.115;
            distanceCalculatorMock.Setup(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(expectedDistance);

            var airportDeparture = new AirportDto { Latitude = 1, Longitude = 1 };
            var airportDestination = new AirportDto { Latitude = 1, Longitude = 1 };

            var calculator = new Calculator(distanceCalculatorMock.Object);
            var resultDistance = calculator.CalculateDistanceBetweenAirports(airportDeparture, airportDestination);

            Assert.Equal(expectedDistance, resultDistance);
            distanceCalculatorMock.Verify(d => d.HaversineInKM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
        }

        [Fact]
        public void Should_CalculateNeededFuel_Throw_Exception_When_Aircraft_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Calculator(new DistanceCalculator()).CalculateNeededFuel(10, null));
        }

        [Fact]
        public void Should_CalculateNeededFuel_Return_Zero_When_DistanceIsZero()
        {
            Assert.Equal(0, new Calculator(new DistanceCalculator()).CalculateNeededFuel(0, new AircraftDto()));
        }

        [Fact]
        public void Should_CalculateNeededFuel_Return_Zero_When_AircraftSpeed()
        {
            Assert.Equal(0, new Calculator(new DistanceCalculator()).CalculateNeededFuel(1000, new AircraftDto { Speed = 0 }));
        }

        [Theory]
        [InlineData(1000,600,20,50, 33383.3333333333)]
        [InlineData(2000, 550, 25, 40, 181858.181818182)]
        [InlineData(3000, 500, 31, 35, 558035)]
        public void Should_CalculateNeededFuel_Return_NeededFuel(double distance, double speed, double fuelConsumption,double takeOffEffort, double expectedNeededFuel)
        {
            var aircraft = new AircraftDto
            {
                Speed = speed,
                FuelConsumption = fuelConsumption,
                TakeOffEffort = takeOffEffort
            };

            var calculator = new Calculator(new DistanceCalculator());
            var resultNeededFuel =calculator.CalculateNeededFuel(distance, aircraft);

            Assert.Equal(expectedNeededFuel, resultNeededFuel, 5);
        }
        
    }
}
