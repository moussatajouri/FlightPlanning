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
    public class AirportRepositoyTests
    {

        private void InsertSampleData(DbContextOptions<FlightsDbContext> options)
        {
            using (var context = new FlightsDbContext(options))
            {
                context.Airport.Add(new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15m, Longitude = 60.1546m, Name = "Name_1" });
                context.Airport.Add(new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BBB", Icao = "BBBB", Latitude = 22.15m, Longitude = 70.9146m, Name = "Name_2" });
                context.Airport.Add(new Airport { City = "city_3", CountryName = "CountryName_3", Iata = "CCC", Icao = "CCCC", Latitude = 32.15m, Longitude = 80.0021m, Name = "Name_3" });

                var insertCount = context.SaveChanges();

                Assert.Equal(3, insertCount);
            }
        }

        #region GetAllAirports

        [Fact]
        public void Should_GetAllAirports_Return_Null_When_Db_Is_Empty()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAllAirports_Return_Null_When_Db_Is_Empty")
                .Options;

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);
                var airports = airportRepositoy.GetAllAirports();

                Assert.NotNull(airports);
                Assert.Empty(airports);
            }
        }

        [Fact]
        public void Should_GetAllAirports_Return_All_Airports_When_Db_Is_Not_Empty()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAllAirports_Return_All_Airports_When_Db_Is_Not_Empty")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);
                var airports = airportRepositoy.GetAllAirports();

                Assert.NotNull(airports);
                Assert.Equal(3, airports.Count());

                var airport = airports.SingleOrDefault(a => a.Iata == "BBB");
                Assert.NotNull(airport);
                Assert.Equal("city_2", airport.City);
                Assert.Equal("CountryName_2", airport.CountryName);
                Assert.Equal("BBB", airport.Iata);
                Assert.Equal("BBBB", airport.Icao);
                Assert.Equal(22.15m, airport.Latitude);
                Assert.Equal(70.9146m, airport.Longitude);
                Assert.Equal("Name_2", airport.Name);
            }
        }

        #endregion GetAllAirports

        #region GetAirportById

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99)]
        public void Should_GetAirportById_return_Null_When_AirportId_IsNegativeOrZero(int airportId)
        {
            var airportRepositoy = new AirportRepositoy(null);

            var airport = airportRepositoy.GetAirportById(airportId);

            Assert.Null(airport);
        }

        [Fact]
        public void Should_GetAirportById_return_Null_When_Airport_IsNoutFound()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAirportById_return_Null_When_Airport_IsNoutFound")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                var airport = airportRepositoy.GetAirportById(5);

                Assert.Null(airport);
            }
        }

        [Fact]
        public void Should_GetAirportById_Return_Airport_When_AirportExist()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAirportById_Return_Airport_When_AirportExist")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var expectedAirportId = context.Airport.Single(a => a.Name == "Name_2").Id;

                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                var airport = airportRepositoy.GetAirportById(expectedAirportId);

                Assert.NotNull(airport);
                Assert.Equal("city_2", airport.City);
                Assert.Equal("CountryName_2", airport.CountryName);
                Assert.Equal("BBB", airport.Iata);
                Assert.Equal("BBBB", airport.Icao);
                Assert.Equal(22.15m, airport.Latitude);
                Assert.Equal(70.9146m, airport.Longitude);
                Assert.Equal("Name_2", airport.Name);
            }
        }

        #endregion

        #region InsertAirport

        [Fact]
        public void Should_InsertAirport_ThrowsException_When_Airport_IsNull()
        {
            var airportRepositoy = new AirportRepositoy(null);

            Assert.Throws<ArgumentNullException>(() => airportRepositoy.InsertAirport(null));
        }

        [Theory]
        [InlineData("Name_1", "AAA", "AAAA")]
        [InlineData("Name_1", "xxx", "xxxx")]
        [InlineData("Name_1", "AAA", "xxxx")]
        [InlineData("Name_1", "xxx", "AAAA")]
        [InlineData("xxx", "AAA", "AAAA")]
        [InlineData("xxx", "AAA", "xxxx")]
        [InlineData("xxx", "xxx", "AAAA")]
        public void Should_InsertAirport_ThrowsFunctionalException_When_AlreadyUniquePropertiesExist(string name, string iata, string icao)
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: $"Should_InsertAirport_ThrowsFunctionalException_When_AlreadyUniquePropertiesExist_n_{name}_iata_{iata}_icao_{icao}")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                var airport = new Airport
                {
                    Name = name,
                    Iata = iata,
                    Icao = icao
                };

                var exception = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.InsertAirport(airport));

                Assert.Equal(ExceptionCodes.InvalidAirportCode, exception.Code);
                Assert.Equal(ExceptionCodes.InvalidAirportMessage, exception.Message);
                Assert.Equal(3, context.Airport.Count());
            }
        }

        [Fact]
        public void Should_InsertAirport_ThrowsException_When_AirportRepositoyThrowsException()
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            var expectedException = new Exception("test exception");
            repositoryMock.Setup(m => m.Insert(It.IsAny<Airport>())).Throws(expectedException);

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var resultException = Assert.Throws<Exception>(() => airportRepositoy.InsertAirport(new Airport()));

            Assert.Equal(expectedException.Message, resultException.Message);
            Assert.Equal(expectedException.StackTrace, resultException.StackTrace);

            repositoryMock.Verify(m => m.Insert(It.IsAny<Airport>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_InsertAirport_ThrowsException_When_AirportRepositoyReturnNegativeOrZeroValue(int insertCount)
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            repositoryMock.Setup(m => m.Insert(It.IsAny<Airport>())).Returns(insertCount);

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => airportRepositoy.InsertAirport(new Airport()));

            Assert.Equal(ExceptionCodes.InvalidAirportMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            repositoryMock.Verify(m => m.Insert(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void Should_InsertAirport_Add_NewAirport()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_InsertAirport_Add_NewAirport")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                var airport = new Airport
                {
                    City = "city_X",
                    CountryName = "CountryName_X",
                    Iata = "XXX",
                    Icao = "XXXX",
                    Latitude = 22.15m,
                    Longitude = 70.9146m,
                    Name = "Name_X"
                };

                airportRepositoy.InsertAirport(airport);
            }

            using (var context = new FlightsDbContext(options))
            {
                var airport = context.Airport.SingleOrDefault(a => a.Name == "Name_X");

                Assert.NotNull(airport);
                Assert.NotEqual(0, airport.Id);
                Assert.Equal("city_X", airport.City);
                Assert.Equal("CountryName_X", airport.CountryName);
                Assert.Equal("XXX", airport.Iata);
                Assert.Equal("XXXX", airport.Icao);
                Assert.Equal(22.15m, airport.Latitude);
                Assert.Equal(70.9146m, airport.Longitude);
                Assert.Equal("Name_X", airport.Name);
            }
        }

        #endregion

        #region UpdateAirport

        [Fact]
        public void Should_UpdateAirport_ThrowsException_When_Airport_IsNull()
        {
            var airportRepositoy = new AirportRepositoy(null);

            Assert.Throws<ArgumentNullException>(() => airportRepositoy.UpdateAirport(null));
        }

        [Theory]

        [InlineData("Name_2", "AAA", "AAAA")]
        [InlineData("Name_2", "xxx", "AAAA")]
        [InlineData("Name_2", "AAA", "xxxx")]
        [InlineData("Name_2", "xxx", "xxxx")]
        [InlineData("Name_1", "BBB", "AAAA")]
        [InlineData("xxxxxx", "BBB", "AAAA")]
        [InlineData("Name_1", "BBB", "xxxx")]
        [InlineData("xxxxxx", "BBB", "xxxx")]
        [InlineData("Name_1", "AAA", "BBBB")]
        [InlineData("Name_1", "xxx", "BBBB")]
        [InlineData("xxxxxx", "AAA", "BBBB")]
        [InlineData("xxxxxx", "xxx", "BBBB")]
        [InlineData("Name_2", "BBB", "BBBB")]
        [InlineData("Name_2", "BBB", "xxxx")]
        [InlineData("Name_2", "xxx", "BBBB")]
        [InlineData("xxxxxx", "BBB", "BBBB")]
        public void Should_UpdateAirport_ThrowsFunctionalException_When_AlreadyUniquePropertiesExist(string name, string iata, string icao)
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: $"Should_UpdateAirport_ThrowsFunctionalException_When_AlreadyUniquePropertiesExist_n_{name}_iata_{iata}_icao_{icao}")
                .Options;

            InsertSampleData(options);

            int airportIdToupdate;

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                var airport = context.Airport.FirstOrDefault(a => a.Name == "Name_1");

                airportIdToupdate = airport.Id;
                airport.Name = name;
                airport.Iata = iata;
                airport.Icao = icao;

                var exception = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.UpdateAirport(airport));

                Assert.Equal(ExceptionCodes.InvalidAirportCode, exception.Code);
                Assert.Equal(ExceptionCodes.InvalidAirportMessage, exception.Message);
                Assert.Equal(3, context.Airport.Count());
            }
            using (var context = new FlightsDbContext(options))
            {
                var airportAtDb = context.Airport.Single(a => a.Id == airportIdToupdate);
                Assert.Equal("Name_1", airportAtDb.Name);
                Assert.Equal("AAA", airportAtDb.Iata);
                Assert.Equal("AAAA", airportAtDb.Icao);
            }
        }

        [Fact]
        public void Should_UpdateAirport_ThrowsException_When_AirportRepositoyThrowsException()
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            var expectedException = new Exception("test exception");
            repositoryMock.Setup(m => m.Update(It.IsAny<Airport>())).Throws(expectedException);

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var resultException = Assert.Throws<Exception>(() => airportRepositoy.UpdateAirport(new Airport()));

            Assert.Equal(expectedException.Message, resultException.Message);
            Assert.Equal(expectedException.StackTrace, resultException.StackTrace);

            repositoryMock.Verify(m => m.Update(It.IsAny<Airport>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_UpdateAirport_ThrowsException_When_AirportRepositoyReturnNegativeOrZeroValue(int updateCount)
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            repositoryMock.Setup(m => m.Update(It.IsAny<Airport>())).Returns(updateCount);

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => airportRepositoy.UpdateAirport(new Airport()));

            Assert.Equal(ExceptionCodes.InvalidAirportMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            repositoryMock.Verify(m => m.Update(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void Should_UpdateAirport_Update_Airport()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_UpdateAirport_Update_Airport")
                .Options;

            InsertSampleData(options);

            Airport expectedAirport;

            using (var context = new FlightsDbContext(options))
            {
                expectedAirport = context.Airport.Single(a => a.Name == "Name_2");
                expectedAirport.City = "city_1_upd";
                expectedAirport.CountryName = "CountryName_1_upd";
                expectedAirport.Iata = "OOO";
                expectedAirport.Icao = "OOOO";
                expectedAirport.Latitude = 10.33m;
                expectedAirport.Longitude = 44.444m;
                expectedAirport.Name = "Name_1_upd";
            }

            using (var context = new FlightsDbContext(options))
            {
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                airportRepositoy.UpdateAirport(expectedAirport);
            }

            using (var context = new FlightsDbContext(options))
            {
                var airport = context.Airport.Single(a => a.Id == expectedAirport.Id);

                Assert.NotNull(airport);
                Assert.NotEqual(0, airport.Id);
                Assert.Equal(expectedAirport.City, airport.City);
                Assert.Equal(expectedAirport.CountryName, airport.CountryName);
                Assert.Equal(expectedAirport.Iata, airport.Iata);
                Assert.Equal(expectedAirport.Icao, airport.Icao);
                Assert.Equal(expectedAirport.Latitude, airport.Latitude);
                Assert.Equal(expectedAirport.Longitude, airport.Longitude);
                Assert.Equal(expectedAirport.Name, airport.Name);
            }
        }

        #endregion

        #region DeleteAirport

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-33)]
        public void Should_DeleteAirport_ReturnWithoutCallingRepository_When_AirportId_IsNegativeOrZero(int airportId)
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            repositoryMock.Setup(m => m.Delete(It.IsAny<Airport>())).Verifiable();

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            airportRepositoy.DeleteAirport(airportId);

            repositoryMock.Verify(m => m.Delete(It.IsAny<Airport>()), Times.Never);
        }

        [Fact]
        public void Should_DeleteAirport_ThrowsException_When_AirportIsNotFound()
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Airport)null);

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var airportId = 10;

            var resultException = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.DeleteAirport(airportId));

            Assert.Equal(string.Format(ExceptionCodes.EntityToDeleteNotFoundCode, "Airport", airportId), resultException.Message);
            Assert.Equal(ExceptionCodes.EntityToDeleteNotFoundCode, resultException.Code);

            repositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(m => m.Delete(It.IsAny<Airport>()), Times.Never);
        }

        [Fact]
        public void Should_DeleteAirport_ThrowsException_When_AirportRepositoyThrowsException()
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            var expectedException = new Exception("test exception");
            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Airport());
            repositoryMock.Setup(m => m.Delete(It.IsAny<Airport>())).Throws(expectedException);

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var resultException = Assert.Throws<Exception>(() => airportRepositoy.DeleteAirport(10));

            Assert.Equal(expectedException.Message, resultException.Message);
            Assert.Equal(expectedException.StackTrace, resultException.StackTrace);

            repositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(m => m.Delete(It.IsAny<Airport>()), Times.Once);
        }

        [Fact]
        public void Should_DeleteAirport_DeleteAirportFromDatabase()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_DeleteAirport_DeleteAirportFromDatabase")
                .Options;

            InsertSampleData(options);

            int airportToDeleteId;

            using (var context = new FlightsDbContext(options))
            {
                var airport = context.Airport.FirstOrDefault();
                airportToDeleteId = airport.Id;
                Assert.NotNull(airport);
                var efRepository = new EntityFrameworkRepository<Airport>(context);
                var airportRepositoy = new AirportRepositoy(efRepository);

                airportRepositoy.DeleteAirport(airportToDeleteId);
            }

            using (var context = new FlightsDbContext(options))
            {
                var airport = context.Airport.SingleOrDefault(a => a.Id == airportToDeleteId);

                Assert.Null(airport);
                Assert.Equal(2, context.Airport.Count());
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_DeleteAirport_ThrowsException_When_AirportRepositoyReturnNegativeOrZeroValue(int updateCount)
        {
            var repositoryMock = new Mock<IRepository<Airport>>();

            repositoryMock.Setup(m => m.Delete(It.IsAny<Airport>())).Returns(updateCount);
            repositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Airport());

            var airportRepositoy = new AirportRepositoy(repositoryMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => airportRepositoy.DeleteAirport(10));

            Assert.Equal(ExceptionCodes.InvalidAirportMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            repositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(m => m.Delete(It.IsAny<Airport>()), Times.Once);
        }

        #endregion
    }
}
