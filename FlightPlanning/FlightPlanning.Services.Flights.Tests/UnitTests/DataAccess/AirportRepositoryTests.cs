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
using Microsoft.Data.Sqlite;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.DataAccess
{
    public class AirportRepositoryTests
    {
        private void InsertSampleData(DbContextOptions<FlightsDbContext> options)
        {
            using (var context = new FlightsDbContext(options))
            {
                context.Airport.Add(new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15, Longitude = 60.1546, Name = "Name_1" });
                context.Airport.Add(new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BBB", Icao = "BBBB", Latitude = 22.15, Longitude = 70.9146, Name = "Name_2" });
                context.Airport.Add(new Airport { City = "city_3", CountryName = "CountryName_3", Iata = "CCC", Icao = "CCCC", Latitude = 32.15, Longitude = 80.0021, Name = "Name_3" });

                var insertCount = context.SaveChanges();

                Assert.Equal(3, insertCount);
            }
        }

        [Fact]
        public void Should_AirportRepository_Throw_Exception_When_InjectedContext_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AirportRepository(null));
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
                var airportRepositoy = new AirportRepository(context);
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
                var airportRepositoy = new AirportRepository(context);
                var airports = airportRepositoy.GetAllAirports();

                Assert.NotNull(airports);
                Assert.Equal(3, airports.Count());

                var airport = airports.SingleOrDefault(a => a.Iata == "BBB");
                Assert.NotNull(airport);
                Assert.Equal("city_2", airport.City);
                Assert.Equal("CountryName_2", airport.CountryName);
                Assert.Equal("BBB", airport.Iata);
                Assert.Equal("BBBB", airport.Icao);
                Assert.Equal(22.15, airport.Latitude);
                Assert.Equal(70.9146, airport.Longitude);
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
            var airportRepositoy = new AirportRepository(new FlightsDbContext());

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
                var airportRepositoy = new AirportRepository(context);

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

                var airportRepositoy = new AirportRepository(context);

                var airport = airportRepositoy.GetAirportById(expectedAirportId);

                Assert.NotNull(airport);
                Assert.Equal("city_2", airport.City);
                Assert.Equal("CountryName_2", airport.CountryName);
                Assert.Equal("BBB", airport.Iata);
                Assert.Equal("BBBB", airport.Icao);
                Assert.Equal(22.15, airport.Latitude);
                Assert.Equal(70.9146, airport.Longitude);
                Assert.Equal("Name_2", airport.Name);
            }
        }

        #endregion

        #region InsertAirport

        [Fact]
        public void Should_InsertAirport_ThrowsException_When_Airport_IsNull()
        {
            var airportRepositoy = new AirportRepository(new FlightsDbContext());

            Assert.Throws<ArgumentNullException>(() => airportRepositoy.InsertAirport(null));
        }

        [Theory]
        [InlineData(null, "city", "countryName", "Name")]
        [InlineData("name", null, "countryName", "City")]
        [InlineData("name", "city", null, "CountryName")]
        public void Should_InsertAirport_ThrowsFunctionalException_When_PrinciplePropertiesMissing(string name, string city, string countryName, string propertie)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<FlightsDbContext>()
                        .UseSqlite(connection)
                        .Options;

                using (var context = new FlightsDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new FlightsDbContext(options))
                {
                    var airportRepositoy = new AirportRepository(context);

                    var airport = new Airport
                    {
                        Name = name,
                        City = city,
                        CountryName = countryName
                    };

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.InsertAirport(airport));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains($"'NOT NULL constraint failed: Airport.{propertie}", exception.Message);
                    Assert.Empty(context.Airport);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Theory]
        [InlineData("Name_1", "xxx", "xxxx", "Name")]
        [InlineData("xxxxxx", "AAA", "xxxx", "IATA")]
        [InlineData("xxxxxx", "xxx", "AAAA", "ICAO")]
        public void Should_InsertAirport_ThrowsFunctionalException_When_AlreadyUniquePropertiesExist(string name, string iata, string icao, string propertie)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<FlightsDbContext>()
                        .UseSqlite(connection)
                        .Options;

                using (var context = new FlightsDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                InsertSampleData(options);

                using (var context = new FlightsDbContext(options))
                {
                    var airportRepositoy = new AirportRepository(context);

                    var airport = new Airport
                    {
                        Name = name,
                        Iata = iata,
                        Icao = icao,
                        City = "City",
                        CountryName = "CountryName"
                    };

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.InsertAirport(airport));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains($"UNIQUE constraint failed: Airport.{propertie}", exception.Message);
                    Assert.Equal(3, context.Airport.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_InsertAirport_ThrowsException_When_AirportRepositoyReturnNegativeOrZeroValue(int insertCount)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Airport>().Add(It.IsAny<Airport>())).Verifiable();
            flightsContextMock.Setup(m => m.SaveChanges()).Returns(insertCount);

            var airportRepositoy = new AirportRepository(flightsContextMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => airportRepositoy.InsertAirport(new Airport()));

            Assert.Equal(ExceptionCodes.NoChangeMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            flightsContextMock.Verify(m => m.Set<Airport>().Add(It.IsAny<Airport>()), Times.Once);
            flightsContextMock.Verify(m => m.SaveChanges(), Times.Once);
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
                var airportRepositoy = new AirportRepository(context);

                var airport = new Airport
                {
                    City = "city_X",
                    CountryName = "CountryName_X",
                    Iata = "XXX",
                    Icao = "XXXX",
                    Latitude = 22.15,
                    Longitude = 70.9146,
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
                Assert.Equal(22.15, airport.Latitude);
                Assert.Equal(70.9146, airport.Longitude);
                Assert.Equal("Name_X", airport.Name);
            }
        }

        #endregion

        #region UpdateAirport

        [Fact]
        public void Should_UpdateAirport_ThrowsException_When_Airport_IsNull()
        {
            var airportRepositoy = new AirportRepository(new FlightsDbContext());

            Assert.Throws<ArgumentNullException>(() => airportRepositoy.UpdateAirport(null));
        }


        [Theory]
        [InlineData("Name_2", "xxx", "xxxx", "Name")]
        [InlineData("xxxxxx", "BBB", "xxxx", "IATA")]
        [InlineData("xxxxxx", "xxx", "BBBB", "ICAO")]
        public void Should_UpdateAirport_ThrowsFunctionalException_When_AlreadyUniquePropertiesExist(string name, string iata, string icao, string propertie)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<FlightsDbContext>()
                        .UseSqlite(connection)
                        .Options;

                using (var context = new FlightsDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                InsertSampleData(options);

                int airportIdToupdate;

                using (var context = new FlightsDbContext(options))
                {
                    var airportRepositoy = new AirportRepository(context);

                    var airport = context.Airport.FirstOrDefault(a => a.Name == "Name_1");

                    airportIdToupdate = airport.Id;
                    airport.Name = name;
                    airport.Iata = iata;
                    airport.Icao = icao;

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.UpdateAirport(airport));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains($"UNIQUE constraint failed: Airport.{propertie}", exception.Message);
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
            finally
            {
                connection.Close();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_UpdateAirport_ThrowsException_When_AirportRepositoyReturnNegativeOrZeroValue(int updateCount)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Airport>().Update(It.IsAny<Airport>())).Verifiable();
            flightsContextMock.Setup(m => m.SaveChanges()).Returns(updateCount);

            var airportRepositoy = new AirportRepository(flightsContextMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(() => airportRepositoy.UpdateAirport(new Airport()));

            Assert.Equal(ExceptionCodes.NoChangeMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            flightsContextMock.Verify(m => m.Set<Airport>().Update(It.IsAny<Airport>()), Times.Once);
            flightsContextMock.Verify(m => m.SaveChanges(), Times.Once);
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
                expectedAirport.Latitude = 10.33;
                expectedAirport.Longitude = 44.444;
                expectedAirport.Name = "Name_1_upd";
            }

            using (var context = new FlightsDbContext(options))
            {
                var airportRepositoy = new AirportRepository(context);

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
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Airport>().Remove(It.IsAny<Airport>())).Verifiable();

            var airportRepositoy = new AirportRepository(flightsContextMock.Object);

            airportRepositoy.DeleteAirport(airportId);

            flightsContextMock.Verify(m => m.Set<Airport>().Remove(It.IsAny<Airport>()), Times.Never);
        }

        [Fact]
        public void Should_DeleteAirport_ThrowsException_When_AirportIsNotFound()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
               .UseInMemoryDatabase(databaseName: "Should_DeleteAirport_ThrowsException_When_AirportIsNotFound")
               .Options;

            using (var context = new FlightsDbContext(options))
            {
                var airportRepositoy = new AirportRepository(context);

                var airportId = 10;

                var resultException = Assert.Throws<FlightPlanningFunctionalException>(() => airportRepositoy.DeleteAirport(airportId));

                Assert.Equal(string.Format(ExceptionCodes.EntityToDeleteNotFoundFormatMessage, "Airport", airportId), resultException.Message);
                Assert.Equal(ExceptionCodes.EntityToDeleteNotFoundCode, resultException.Code);
            }
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
                Assert.NotNull(airport);
                airportToDeleteId = airport.Id;
            }

            using (var context = new FlightsDbContext(options))
            {
                var airportRepositoy = new AirportRepository(context);
                airportRepositoy.DeleteAirport(airportToDeleteId);
            }

            using (var context = new FlightsDbContext(options))
            {
                var airport = context.Airport.SingleOrDefault(a => a.Id == airportToDeleteId);

                Assert.Null(airport);
                Assert.Equal(2, context.Airport.Count());
            }
        }

        #endregion
    }
}
