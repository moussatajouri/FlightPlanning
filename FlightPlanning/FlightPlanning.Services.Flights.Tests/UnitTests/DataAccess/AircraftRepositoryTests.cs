using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using FlightPlanning.Services.Flights.Transverse.Exception;
using Moq;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.DataAccess
{
    public class AircraftRepositoryTests
    {
        private void InsertSampleData(DbContextOptions<FlightsDbContext> options)
        {
            using (var context = new FlightsDbContext(options))
            {
                context.Aircraft.Add(new Aircraft { Name = "Name_1", FuelCapacity = 1000, FuelConsumption= 1, Speed= 800, TakeOffEffort= 0.2m });
                context.Aircraft.Add(new Aircraft { Name = "Name_2", FuelCapacity = 1500, FuelConsumption = 0.6m, Speed = 750, TakeOffEffort = 0.3m });
                context.Aircraft.Add(new Aircraft { Name = "Name_3", FuelCapacity = 1200, FuelConsumption = 0.4m, Speed = 900, TakeOffEffort = 0.4m });

                var insertCount = context.SaveChanges();

                Assert.Equal(3, insertCount);
            }
        }

        #region GetAllAircrafts

        [Fact]
        public void Should_GetAllAircrafts_Return_Null_When_Db_Is_Empty()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAllAircrafts_Return_Null_When_Db_Is_Empty")
                .Options;

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);
                var aircrafts = aircraftRepositoy.GetAllAircrafts();

                Assert.NotNull(aircrafts);
                Assert.Empty(aircrafts);
            }
        }

        [Fact]
        public void Should_GetAllAircrafts_Return_All_Aircrafts_When_Db_Is_Not_Empty()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAllAircrafts_Return_All_Aircrafts_When_Db_Is_Not_Empty")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);
                var aircrafts = aircraftRepositoy.GetAllAircrafts();

                Assert.NotNull(aircrafts);
                Assert.Equal(3, aircrafts.Count());

                var aircraft = aircrafts.SingleOrDefault(a => a.Name == "Name_2");
                Assert.NotNull(aircraft);
                Assert.Equal(1500, aircraft.FuelCapacity);
                Assert.Equal(0.6m, aircraft.FuelConsumption);
                Assert.Equal(750, aircraft.Speed);
                Assert.Equal(0.3m, aircraft.TakeOffEffort);
            }
        }

        #endregion GetAllAircrafts

        #region GetAircraftById

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99)]
        public void Should_GetAircraftById_return_Null_When_AircraftId_IsNegativeOrZero(int aircraftId)
        {
            var aircraftRepositoy = new AircraftRepository(null);

            var aircraft = aircraftRepositoy.GetAircraftById(aircraftId);

            Assert.Null(aircraft);
        }

        [Fact]
        public void Should_GetAircraftById_return_Null_When_Aircraft_IsNoutFound()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAircraftById_return_Null_When_Aircraft_IsNoutFound")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                var aircraft = aircraftRepositoy.GetAircraftById(5);

                Assert.Null(aircraft);
            }
        }

        [Fact]
        public void Should_GetAircraftById_Return_Aircraft_When_AircraftExist()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAircraftById_Return_Aircraft_When_AircraftExist")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var expectedAircraftId = context.Aircraft.Single(a => a.Name == "Name_2").Id;

                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                var aircraft = aircraftRepositoy.GetAircraftById(expectedAircraftId);

                Assert.NotNull(aircraft);
                Assert.Equal(1500, aircraft.FuelCapacity);
                Assert.Equal(0.6m, aircraft.FuelConsumption);
                Assert.Equal(750, aircraft.Speed);
                Assert.Equal(0.3m, aircraft.TakeOffEffort);
            }
        }

        #endregion

        #region InsertAircraft

        [Fact]
        public void Should_InsertAircraft_ThrowsException_When_Aircraft_IsNull()
        {
            var aircraftRepositoy = new AircraftRepository(null);

            Assert.Throws<ArgumentNullException>(() => aircraftRepositoy.InsertAircraft(null));
        }

        [Theory]
        [InlineData("Name_1")]
        [InlineData("Name_2")]
        [InlineData("Name_3")]
        public void Should_InsertAircraft_ThrowsFunctionalException_When_AlreadyNameExist(string name)
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: $"Should_InsertAircraft_ThrowsFunctionalException_When_AlreadyNameExist_n_{name}")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                var aircraft = new Aircraft
                {
                    Name = name,
                };

                var exception = Assert.Throws<FlightPlanningFunctionalException>(() => aircraftRepositoy.InsertAircraft(aircraft));

                Assert.Equal(string.Format(ExceptionCodes.InvalidEntityFormatCode, "aircraft"), exception.Code);
                Assert.Equal(ExceptionCodes.InvalidAircraftMessage, exception.Message);
                Assert.Equal(3, context.Aircraft.Count());
            }
        }

        [Fact]
        public void Should_InsertAircraft_ThrowsException_When_AircraftRepositoyThrowsException()
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            var expectedException = new Exception("test exception");
            repositoryMock.Setup(m => m.Insert(It.IsAny<Aircraft>())).Throws(expectedException);

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var resultException = Assert.Throws<Exception>(() => aircraftRepositoy.InsertAircraft(new Aircraft()));

            Assert.Equal(expectedException.Message, resultException.Message);
            Assert.Equal(expectedException.StackTrace, resultException.StackTrace);

            repositoryMock.Verify(m => m.Insert(It.IsAny<Aircraft>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_InsertAircraft_ThrowsException_When_AircraftRepositoyReturnNegativeOrZeroValue(int insertCount)
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            repositoryMock.Setup(m => m.Insert(It.IsAny<Aircraft>())).Returns(insertCount);

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => aircraftRepositoy.InsertAircraft(new Aircraft()));

            Assert.Equal(ExceptionCodes.InvalidAircraftMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            repositoryMock.Verify(m => m.Insert(It.IsAny<Aircraft>()), Times.Once);
        }

        [Fact]
        public void Should_InsertAircraft_Add_NewAircraft()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_InsertAircraft_Add_NewAircraft")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                var aircraft = new Aircraft
                {
                    Name = "aircraft_X",
                    FuelCapacity= 99999,
                    FuelConsumption= 1.12m,
                    Speed = 545,
                    TakeOffEffort= 45.36m
                };

                aircraftRepositoy.InsertAircraft(aircraft);
            }

            using (var context = new FlightsDbContext(options))
            {
                var aircraft = context.Aircraft.SingleOrDefault(a => a.Name == "aircraft_X");

                Assert.NotNull(aircraft);
                Assert.NotEqual(0, aircraft.Id);
                Assert.Equal("aircraft_X", aircraft.Name);
                Assert.Equal(99999, aircraft.FuelCapacity);
                Assert.Equal(1.12m, aircraft.FuelConsumption);
                Assert.Equal(545, aircraft.Speed);
                Assert.Equal(45.36m, aircraft.TakeOffEffort);
            }
        }

        #endregion

        #region UpdateAircraft

        [Fact]
        public void Should_UpdateAircraft_ThrowsException_When_Aircraft_IsNull()
        {
            var aircraftRepositoy = new AircraftRepository(null);

            Assert.Throws<ArgumentNullException>(() => aircraftRepositoy.UpdateAircraft(null));
        }

        [Theory]
        [InlineData("Name_2")]
        [InlineData("Name_3")]
        public void Should_UpdateAircraft_ThrowsFunctionalException_When_AlreadyNameExist(string name)
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: $"Should_UpdateAircraft_ThrowsFunctionalException_When_AlreadyNameExist_n_{name}")
                .Options;

            InsertSampleData(options);

            int aircraftIdToupdate;

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                var aircraft = context.Aircraft.FirstOrDefault(a => a.Name == "Name_1");

                aircraftIdToupdate = aircraft.Id;
                aircraft.Name = name;

                var exception = Assert.Throws<FlightPlanningFunctionalException>(() => aircraftRepositoy.UpdateAircraft(aircraft));

                Assert.Equal(string.Format(ExceptionCodes.InvalidEntityFormatCode, "aircraft"), exception.Code);
                Assert.Equal(ExceptionCodes.InvalidAircraftMessage, exception.Message);
                Assert.Equal(3, context.Aircraft.Count());
            }
            using (var context = new FlightsDbContext(options))
            {
                var aircraftAtDb = context.Aircraft.Single(a => a.Id == aircraftIdToupdate);
                Assert.Equal("Name_1", aircraftAtDb.Name);
            }
        }

        [Fact]
        public void Should_UpdateAircraft_ThrowsException_When_AircraftRepositoyThrowsException()
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            var expectedException = new Exception("test exception");
            repositoryMock.Setup(m => m.Update(It.IsAny<Aircraft>())).Throws(expectedException);

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var resultException = Assert.Throws<Exception>(() => aircraftRepositoy.UpdateAircraft(new Aircraft()));

            Assert.Equal(expectedException.Message, resultException.Message);
            Assert.Equal(expectedException.StackTrace, resultException.StackTrace);

            repositoryMock.Verify(m => m.Update(It.IsAny<Aircraft>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_UpdateAircraft_ThrowsException_When_AircraftRepositoyReturnNegativeOrZeroValue(int updateCount)
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            repositoryMock.Setup(m => m.Update(It.IsAny<Aircraft>())).Returns(updateCount);

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => aircraftRepositoy.UpdateAircraft(new Aircraft()));

            Assert.Equal(ExceptionCodes.InvalidAircraftMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            repositoryMock.Verify(m => m.Update(It.IsAny<Aircraft>()), Times.Once);
        }

        [Fact]
        public void Should_UpdateAircraft_Update_Aircraft()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_UpdateAircraft_Update_Aircraft")
                .Options;

            InsertSampleData(options);

            Aircraft expectedAircraft;

            using (var context = new FlightsDbContext(options))
            {
                expectedAircraft = context.Aircraft.Single(a => a.Name == "Name_2");
                expectedAircraft.Name = "aircraft_X";
                expectedAircraft.FuelCapacity = 5555;
                expectedAircraft.FuelConsumption = 0.95m;
                expectedAircraft.Speed = 566;
                expectedAircraft.TakeOffEffort = 13.99m;
            }

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                aircraftRepositoy.UpdateAircraft(expectedAircraft);
            }

            using (var context = new FlightsDbContext(options))
            {
                var aircraft = context.Aircraft.Single(a => a.Id == expectedAircraft.Id);

                Assert.NotNull(aircraft);
                Assert.NotEqual(0, aircraft.Id);
                Assert.Equal(expectedAircraft.Name, expectedAircraft.Name);
                Assert.Equal(expectedAircraft.FuelCapacity, expectedAircraft.FuelCapacity);
                Assert.Equal(expectedAircraft.FuelConsumption, expectedAircraft.FuelConsumption);
                Assert.Equal(expectedAircraft.Speed, expectedAircraft.Speed);
                Assert.Equal(expectedAircraft.TakeOffEffort, expectedAircraft.TakeOffEffort);
            }
        }

        #endregion

        #region DeleteAircraft

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-33)]
        public void Should_DeleteAircraft_ReturnWithoutCallingRepository_When_AircraftId_IsNegativeOrZero(int aircraftId)
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            repositoryMock.Setup(m => m.Delete(It.IsAny<Aircraft>())).Verifiable();

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            aircraftRepositoy.DeleteAircraft(aircraftId);

            repositoryMock.Verify(m => m.Delete(It.IsAny<Aircraft>()), Times.Never);
        }

        [Fact]
        public void Should_DeleteAircraft_ThrowsException_When_AircraftIsNotFound()
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Aircraft)null);

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var aircraftId = 10;

            var resultException = Assert.Throws<FlightPlanningFunctionalException>(() => aircraftRepositoy.DeleteAircraft(aircraftId));

            Assert.Equal(string.Format(ExceptionCodes.EntityToDeleteNotFoundCode, "Aircraft", aircraftId), resultException.Message);
            Assert.Equal(ExceptionCodes.EntityToDeleteNotFoundCode, resultException.Code);

            repositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(m => m.Delete(It.IsAny<Aircraft>()), Times.Never);
        }

        [Fact]
        public void Should_DeleteAircraft_ThrowsException_When_AircraftRepositoyThrowsException()
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            var expectedException = new Exception("test exception");
            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Aircraft());
            repositoryMock.Setup(m => m.Delete(It.IsAny<Aircraft>())).Throws(expectedException);

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var resultException = Assert.Throws<Exception>(() => aircraftRepositoy.DeleteAircraft(10));

            Assert.Equal(expectedException.Message, resultException.Message);
            Assert.Equal(expectedException.StackTrace, resultException.StackTrace);

            repositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(m => m.Delete(It.IsAny<Aircraft>()), Times.Once);
        }

        [Fact]
        public void Should_DeleteAircraft_DeleteAircraftFromDatabase()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_DeleteAircraft_DeleteAircraftFromDatabase")
                .Options;

            InsertSampleData(options);

            int aircraftToDeleteId;

            using (var context = new FlightsDbContext(options))
            {
                var aircraft = context.Aircraft.FirstOrDefault();
                aircraftToDeleteId = aircraft.Id;
                Assert.NotNull(aircraft);
                var efRepository = new EntityFrameworkRepository<Aircraft>(context);
                var aircraftRepositoy = new AircraftRepository(efRepository);

                aircraftRepositoy.DeleteAircraft(aircraftToDeleteId);
            }

            using (var context = new FlightsDbContext(options))
            {
                var aircraft = context.Aircraft.SingleOrDefault(a => a.Id == aircraftToDeleteId);

                Assert.Null(aircraft);
                Assert.Equal(2, context.Aircraft.Count());
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_DeleteAircraft_ThrowsException_When_AircraftRepositoyReturnNegativeOrZeroValue(int updateCount)
        {
            var repositoryMock = new Mock<IRepository<Aircraft>>();

            repositoryMock.Setup(m => m.Delete(It.IsAny<Aircraft>())).Returns(updateCount);
            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Aircraft());

            var aircraftRepositoy = new AircraftRepository(repositoryMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => aircraftRepositoy.DeleteAircraft(10));

            Assert.Equal(ExceptionCodes.InvalidAircraftMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            repositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(m => m.Delete(It.IsAny<Aircraft>()), Times.Once);
        }

        #endregion
    }
}
