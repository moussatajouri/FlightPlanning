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
    public class AircraftServiceTests
    {
        [Fact]
        public void Should_AircraftService_Throw_Exception_When_InjectedRepository_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AircraftService(null));
        }

        [Fact]
        public void Should_GetAircraftById_return_Null_When_AircraftIsNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.GetAircraftById(It.IsAny<int>())).Returns((Aircraft)null);

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            var aircraft = aircraftService.GetAircraftById(1);

            Assert.Null(aircraft);
            aircraftRepositoryMock.Verify(m => m.GetAircraftById(It.IsAny<int>()),Times.Once);
        }

        [Fact]
        public void Should_GetAircraftById_return_Dto_When_AircraftIsNotNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            var aircraft = new Aircraft
            {
                Id = 5,
                Name = "Name",
                Speed = 500,
                FuelCapacity = 1000,
                FuelConsumption = 60,
                TakeOffEffort = 10
            };

            aircraftRepositoryMock.Setup(m => m.GetAircraftById(It.IsAny<int>())).Returns(aircraft);

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            var aircraftDto = aircraftService.GetAircraftById(1);

            aircraftRepositoryMock.Verify(m => m.GetAircraftById(It.IsAny<int>()), Times.Once);

            Assert.Equal(aircraft.Id, aircraftDto.Id);
            Assert.Equal(aircraft.Name, aircraftDto.Name);
            Assert.Equal(aircraft.Speed, aircraftDto.Speed);
            Assert.Equal(aircraft.FuelCapacity, aircraftDto.FuelCapacity);
            Assert.Equal(aircraft.FuelConsumption, aircraftDto.FuelConsumption);
            Assert.Equal(aircraft.TakeOffEffort, aircraftDto.TakeOffEffort);
        }

        [Fact]
        public void Should_GetAllAircrafts_return_Null_When_AircraftsIsNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.GetAllAircrafts()).Returns((IEnumerable<Aircraft>)null);

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            var aircrafts = aircraftService.GetAllAircrafts();

            Assert.Null(aircrafts);
            aircraftRepositoryMock.Verify(m => m.GetAllAircrafts(), Times.Once);
        }

        [Fact]
        public void Should_GetAllAircrafts_return_EmptyList_When_AircraftsIsEmpty()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.GetAllAircrafts()).Returns((new List<Aircraft>()));

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            var aircrafts = aircraftService.GetAllAircrafts();

            Assert.NotNull(aircrafts);
            Assert.Empty(aircrafts);
            aircraftRepositoryMock.Verify(m => m.GetAllAircrafts(), Times.Once);
        }

        [Fact]
        public void Should_GetAllAircrafts_return_List_When_AircraftsIsNotEmpty()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.GetAllAircrafts()).Returns((new List<Aircraft> { new Aircraft(), new Aircraft()}));

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            var aircrafts = aircraftService.GetAllAircrafts();

            Assert.NotNull(aircrafts);
            Assert.Equal(2, aircrafts.Count());
            aircraftRepositoryMock.Verify(m => m.GetAllAircrafts(), Times.Once);
        }

        [Fact]
        public void Should_InsertAircraft_DoesNotCall_InsertAircraft_When_AircraftsIsNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.InsertAircraft(It.IsAny<Aircraft>())).Verifiable();

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            aircraftService.InsertAircraft(null);

            aircraftRepositoryMock.Verify(m => m.InsertAircraft(It.IsAny<Aircraft>()), Times.Never);
        }

        [Fact]
        public void Should_InsertAircraft_Call_InsertAircraft_When_AircraftsIsNotNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.InsertAircraft(It.IsAny<Aircraft>())).Verifiable();

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            aircraftService.InsertAircraft(new AircraftDto());

            aircraftRepositoryMock.Verify(m => m.InsertAircraft(It.IsAny<Aircraft>()), Times.Once);
        }

        [Fact]
        public void Should_UpdateAircraft_DoesNotCallUpdateAircraft_When_AircraftsIsNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.UpdateAircraft(It.IsAny<Aircraft>())).Verifiable();

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            aircraftService.UpdateAircraft(null);

            aircraftRepositoryMock.Verify(m => m.UpdateAircraft(It.IsAny<Aircraft>()), Times.Never);
        }

        [Fact]
        public void Should_UpdateAircraft_Call_UpdateAircraft_When_AircraftsIsNotNull()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.UpdateAircraft(It.IsAny<Aircraft>())).Verifiable();

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            aircraftService.UpdateAircraft(new AircraftDto());

            aircraftRepositoryMock.Verify(m => m.UpdateAircraft(It.IsAny<Aircraft>()), Times.Once);
        }

        [Fact]
        public void Should_DeleteAircraft_Call_DeleteAircraft()
        {
            var aircraftRepositoryMock = new Mock<IAircraftRepository>();

            aircraftRepositoryMock.Setup(m => m.DeleteAircraft(It.IsAny<int>())).Verifiable();

            var aircraftService = new AircraftService(aircraftRepositoryMock.Object);

            aircraftService.DeleteAircraft(2);

            aircraftRepositoryMock.Verify(m => m.DeleteAircraft(It.IsAny<int>()), Times.Once);
        }
    }
}
