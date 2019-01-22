using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse.Exception;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.DataAccess
{
    public class FlightRepositoryTests
    {
        private void InsertSampleData(DbContextOptions<FlightsDbContext> options)
        {
            using (var context = new FlightsDbContext(options))
            {
                var departure = new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15, Longitude = 60.1546, Name = "Name_1" };
                var destination_1 = new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BB2", Icao = "BBB2", Latitude = 22.15, Longitude = 70.9146, Name = "Name_2" };
                var destination_2 = new Airport { City = "city_3", CountryName = "CountryName_3", Iata = "BB3", Icao = "BBB3", Latitude = 22.15, Longitude = 70.9146, Name = "Name_3" };
                context.Airport.Add(departure);
                context.Airport.Add(destination_1);
                context.Airport.Add(destination_2);

                var aircraft = new Aircraft { Name = "Name_1", FuelCapacity = 1000, FuelConsumption = 1, Speed = 800, TakeOffEffort = 0.2 };
                context.Aircraft.Add(aircraft);

                context.Flight.Add(new Flight { UpdateDate = new DateTime(2019, 10, 02, 10, 12, 36), Aircraft = aircraft, AirportDeparture = departure, AirportDestination = destination_1 });
                context.Flight.Add(new Flight { UpdateDate = new DateTime(2019, 10, 02, 10, 12, 36), Aircraft = aircraft, AirportDeparture = departure, AirportDestination = destination_2 });

                var insertCount = context.SaveChanges();

                Assert.Equal(6, insertCount);
            }
        }

        [Fact]
        public void Should_FlightRepository_Throw_Exception_When_InjectedContext_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FlightRepository(null));
        }

        #region GetAllFlights

        [Fact]
        public void Should_GetAllFlights_Return_Null_When_Db_Is_Empty()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAllFlights_Return_Null_When_Db_Is_Empty")
                .Options;

            using (var context = new FlightsDbContext(options))
            {
                var flightRepositoy = new FlightRepository(context);
                var flights = flightRepositoy.GetAllFlights();

                Assert.NotNull(flights);
                Assert.Empty(flights);
            }
        }

        [Fact]
        public void Should_GetAllFlights_Return_All_Flights_When_Db_Is_Not_Empty()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetAllFlights_Return_All_Flights_When_Db_Is_Not_Empty")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var flightRepositoy = new FlightRepository(context);
                var flights = flightRepositoy.GetAllFlights();

                Assert.NotNull(flights);
                Assert.Equal(2, flights.Count());

                var expectedDateTime = new DateTime(2019, 10, 02, 10, 12, 36);

                foreach (var flight in flights)
                {
                    Assert.NotNull(flight);
                    Assert.Equal(expectedDateTime, flight.UpdateDate);
                    var aircraft = flight.Aircraft;
                    Assert.NotNull(aircraft);
                    Assert.NotEqual(0, aircraft.Id);
                    Assert.Equal("Name_1", aircraft.Name);
                    Assert.Equal(1000, aircraft.FuelCapacity);
                    Assert.Equal(1, aircraft.FuelConsumption);
                    Assert.Equal(800, aircraft.Speed);
                    Assert.Equal(0.2, aircraft.TakeOffEffort);

                    var airportDeparture = flight.AirportDeparture;
                    Assert.NotNull(airportDeparture);
                    Assert.NotEqual(0, airportDeparture.Id);
                    Assert.Equal("city_1", airportDeparture.City);
                    Assert.Equal("CountryName_1", airportDeparture.CountryName);
                    Assert.Equal("AAA", airportDeparture.Iata);
                    Assert.Equal("AAAA", airportDeparture.Icao);
                    Assert.Equal(12.15, airportDeparture.Latitude);
                    Assert.Equal(60.1546, airportDeparture.Longitude);
                    Assert.Equal("Name_1", airportDeparture.Name);

                    var airportDestination = flight.AirportDestination;
                    Assert.NotNull(airportDestination);
                    Assert.NotEqual(0, airportDestination.Id);
                    Assert.StartsWith("city_", airportDestination.City);
                    Assert.StartsWith("CountryName_", airportDestination.CountryName);
                    Assert.StartsWith("BB", airportDestination.Iata);
                    Assert.StartsWith("BBB", airportDestination.Icao);
                    Assert.Equal(22.15, airportDestination.Latitude);
                    Assert.Equal(70.9146, airportDestination.Longitude);
                    Assert.StartsWith("Name_", airportDestination.Name);
                }
            }
        }

        #endregion GetAllFlights

        #region GetFlightById

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99)]
        public void Should_GetFlightById_return_Null_When_FlightId_IsNegativeOrZero(int flightId)
        {
            var flightRepositoy = new FlightRepository(new FlightsDbContext());

            var flight = flightRepositoy.GetFlightById(flightId);

            Assert.Null(flight);
        }

        [Fact]
        public void Should_GetFlightById_return_Null_When_Flight_IsNoutFound()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetFlightById_return_Null_When_Flight_IsNoutFound")
                .Options;

            using (var context = new FlightsDbContext(options))
            {
                var flightRepositoy = new FlightRepository(context);

                var flight = flightRepositoy.GetFlightById(5);

                Assert.Null(flight);
            }
        }

        [Fact]
        public void Should_GetFlightById_Return_Flight_When_FlightExist()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_GetFlightById_Return_Flight_When_FlightExist")
                .Options;

            InsertSampleData(options);

            using (var context = new FlightsDbContext(options))
            {
                var expectedFlightId = context.Flight.FirstOrDefault().Id;

                var flightRepositoy = new FlightRepository(context);

                var flight = flightRepositoy.GetFlightById(expectedFlightId);

                Assert.NotNull(flight);
                Assert.Equal(new DateTime(2019, 10, 02, 10, 12, 36), flight.UpdateDate);

                var aircraft = flight.Aircraft;
                Assert.NotNull(aircraft);
                Assert.NotEqual(0, aircraft.Id);
                Assert.Equal("Name_1", aircraft.Name);
                Assert.Equal(1000, aircraft.FuelCapacity);
                Assert.Equal(1, aircraft.FuelConsumption);
                Assert.Equal(800, aircraft.Speed);
                Assert.Equal(0.2, aircraft.TakeOffEffort);

                var airportDeparture = flight.AirportDeparture;
                Assert.NotNull(airportDeparture);
                Assert.NotEqual(0, airportDeparture.Id);
                Assert.Equal("city_1", airportDeparture.City);
                Assert.Equal("CountryName_1", airportDeparture.CountryName);
                Assert.Equal("AAA", airportDeparture.Iata);
                Assert.Equal("AAAA", airportDeparture.Icao);
                Assert.Equal(12.15, airportDeparture.Latitude);
                Assert.Equal(60.1546, airportDeparture.Longitude);
                Assert.Equal("Name_1", airportDeparture.Name);

                var airportDestination = flight.AirportDestination;
                Assert.NotNull(airportDestination);
                Assert.NotEqual(0, airportDestination.Id);
                Assert.Equal("city_2", airportDestination.City);
                Assert.Equal("CountryName_2", airportDestination.CountryName);
                Assert.Equal("BB2", airportDestination.Iata);
                Assert.Equal("BBB2", airportDestination.Icao);
                Assert.Equal(22.15, airportDestination.Latitude);
                Assert.Equal(70.9146, airportDestination.Longitude);
                Assert.Equal("Name_2", airportDestination.Name);
            }
        }

        #endregion

        #region InsertFlight

        [Fact]
        public void Should_InsertFlight_ThrowsException_When_Flight_IsNull()
        {
            var flightRepositoy = new FlightRepository(new FlightsDbContext());

            Assert.Throws<ArgumentNullException>(() => flightRepositoy.InsertFlight(null));
        }

        [Theory]
        [InlineData(null, 1, 2)]
        [InlineData(1, null, 2)]
        [InlineData(1, 1, null)]
        public void Should_InsertFlight_ThrowsFunctionalException_When_PrinciplePropertiesMissing(int? aircraftId, int? airportDepartureId, int? airportDestinationId)
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
                    var flightRepositoy = new FlightRepository(context);

                    var flight = new Flight();

                    if (aircraftId.HasValue)
                    {
                        flight.AircraftId = aircraftId.Value;
                    }
                    if (airportDepartureId.HasValue)
                    {
                        flight.AirportDepartureId = airportDepartureId.Value;
                    }
                    if (airportDestinationId.HasValue)
                    {
                        flight.AirportDestinationId = airportDestinationId.Value;
                    }

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.InsertFlight(flight));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains("FOREIGN KEY constraint failed", exception.Message);
                    Assert.Empty(context.Flight);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(99, 99)]
        public void Should_InsertFlight_ThrowsFunctionalException_When_SameDepartureAndDestinationAirport(int airportDepartureId, int airportDestinationId)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Flight>().Add(It.IsAny<Flight>())).Verifiable();
            flightsContextMock.Setup(m => m.SaveChanges()).Verifiable();

            var flightRepositoy = new FlightRepository(flightsContextMock.Object);

            var flight = new Flight
            {
                AirportDepartureId = airportDepartureId,
                AirportDestinationId = airportDestinationId,
            };

            var exception = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.InsertFlight(flight));

            Assert.Equal(ExceptionCodes.SameDepartureAndDestinationAirportCode, exception.Code);
            Assert.Contains(string.Format(ExceptionCodes.SameDepartureAndDestinationAirportFormatMessage, airportDepartureId, airportDestinationId), exception.Message);
            flightsContextMock.Verify(m => m.Set<Flight>().Add(It.IsAny<Flight>()), Times.Never);
            flightsContextMock.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_InsertFlight_ThrowsException_When_FlightRepositoyReturnNegativeOrZeroValue(int insertCount)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Flight>().Add(It.IsAny<Flight>())).Verifiable();
            flightsContextMock.Setup(m => m.SaveChanges()).Returns(insertCount);

            var flightRepositoy = new FlightRepository(flightsContextMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(
                () => flightRepositoy.InsertFlight(new Flight { AirportDepartureId = 1, AirportDestinationId = 2 }));

            Assert.Equal(ExceptionCodes.NoChangeMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            flightsContextMock.Verify(m => m.Set<Flight>().Add(It.IsAny<Flight>()), Times.Once);
            flightsContextMock.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void Should_InsertFlight_ThrowsFunctionalException_When_AirportOrAircraft_IsNotFound(bool insertAircraft, bool insertAirportDeparture, bool insertAirportDestination)
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


                int aircraftId = 0, airportDepartureId = 0, airportDestinationId = 0;

                using (var context = new FlightsDbContext(options))
                {
                    var expectedInsertCount = 0;
                    if (insertAirportDeparture)
                    {
                        var departure = new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15, Longitude = 60.1546, Name = "Name_1" };
                        context.Airport.Add(departure);
                        airportDepartureId = departure.Id;
                        expectedInsertCount++;
                    }
                    if (insertAirportDestination)
                    {
                        var destination = new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BB2", Icao = "BBB2", Latitude = 22.15, Longitude = 70.9146, Name = "Name_2" };
                        context.Airport.Add(destination);
                        airportDestinationId = destination.Id;
                        expectedInsertCount++;
                    }
                    if (insertAircraft)
                    {
                        var aircraft = new Aircraft { Name = "Name_1", FuelCapacity = 1000, FuelConsumption = 1, Speed = 800, TakeOffEffort = 0.2 };
                        context.Aircraft.Add(aircraft);
                        aircraftId = aircraft.Id;
                        expectedInsertCount++;
                    }

                    var insertCount = context.SaveChanges();

                    Assert.Equal(expectedInsertCount, insertCount);
                }

                using (var context = new FlightsDbContext(options))
                {
                    var flightRepositoy = new FlightRepository(context);

                    var flight = new Flight
                    {
                        AirportDepartureId = airportDepartureId,
                        AirportDestinationId = airportDestinationId,
                        AircraftId = aircraftId
                    };

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.InsertFlight(flight));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains("FOREIGN KEY constraint failed", exception.Message);
                }

                using (var context = new FlightsDbContext(options))
                {
                    Assert.Empty(context.Flight);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void Should_InsertFlight_Add_NewFlight()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_InsertFlight_Add_NewFlight")
                .Options;

            int aircraftId, airportDepartureId, airportDestinationId;

            using (var context = new FlightsDbContext(options))
            {
                var departure = new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15, Longitude = 60.1546, Name = "Name_1" };
                var destination = new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BB2", Icao = "BBB2", Latitude = 22.15, Longitude = 70.9146, Name = "Name_2" };
                context.Airport.Add(departure);
                context.Airport.Add(destination);

                var aircraft = new Aircraft { Name = "Name_1", FuelCapacity = 1000, FuelConsumption = 1, Speed = 800, TakeOffEffort = 0.2 };
                context.Aircraft.Add(aircraft);

                var insertCount = context.SaveChanges();

                Assert.Equal(3, insertCount);
                aircraftId = aircraft.Id;
                airportDepartureId = departure.Id;
                airportDestinationId= destination.Id;
            }

            using (var context = new FlightsDbContext(options))
            {
                var flightRepositoy = new FlightRepository(context);

                var flight = new Flight
                {
                    AirportDepartureId = airportDepartureId,
                    AirportDestinationId = airportDestinationId,
                    AircraftId = aircraftId
                };

                flightRepositoy.InsertFlight(flight);
            }

            using (var context = new FlightsDbContext(options))
            {
                var flight = context.Flight.Include(f => f.AirportDeparture).Include(f => f.AirportDestination).Include(f => f.Aircraft).SingleOrDefault();

                Assert.NotNull(flight);
                Assert.NotNull(flight.UpdateDate);
                Assert.Equal(DateTime.Now, flight.UpdateDate.Value, TimeSpan.FromMinutes(2));
                var aircraft = flight.Aircraft;
                Assert.NotNull(aircraft);
                Assert.NotEqual(0, aircraft.Id);
                Assert.Equal("Name_1", aircraft.Name);
                Assert.Equal(1000, aircraft.FuelCapacity);
                Assert.Equal(1, aircraft.FuelConsumption);
                Assert.Equal(800, aircraft.Speed);
                Assert.Equal(0.2, aircraft.TakeOffEffort);

                var airportDeparture = flight.AirportDeparture;
                Assert.NotNull(airportDeparture);
                Assert.NotEqual(0, airportDeparture.Id);
                Assert.Equal("city_1", airportDeparture.City);
                Assert.Equal("CountryName_1", airportDeparture.CountryName);
                Assert.Equal("AAA", airportDeparture.Iata);
                Assert.Equal("AAAA", airportDeparture.Icao);
                Assert.Equal(12.15, airportDeparture.Latitude);
                Assert.Equal(60.1546, airportDeparture.Longitude);
                Assert.Equal("Name_1", airportDeparture.Name);

                var airportDestination = flight.AirportDestination;
                Assert.NotNull(airportDestination);
                Assert.NotEqual(0, airportDestination.Id);
                Assert.Equal("city_2", airportDestination.City);
                Assert.Equal("CountryName_2", airportDestination.CountryName);
                Assert.Equal("BB2", airportDestination.Iata);
                Assert.Equal("BBB2", airportDestination.Icao);
                Assert.Equal(22.15, airportDestination.Latitude);
                Assert.Equal(70.9146, airportDestination.Longitude);
                Assert.Equal("Name_2", airportDestination.Name);
            }
        }

        #endregion

        #region UpdateFlight

        [Fact]
        public void Should_UpdateFlight_ThrowsException_When_Flight_IsNull()
        {
            var flightRepositoy = new FlightRepository(new FlightsDbContext());

            Assert.Throws<ArgumentNullException>(() => flightRepositoy.UpdateFlight(null));
        }

        [Theory]
        [InlineData(null, 1, 2)]
        [InlineData(1, null, 2)]
        [InlineData(1, 1, null)]
        public void Should_UpdateFlight_ThrowsFunctionalException_When_PrinciplePropertiesMissing(int? aircraftId, int? airportDepartureId, int? airportDestinationId)
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
                    var flightRepositoy = new FlightRepository(context);

                    var flight = new Flight();

                    if (aircraftId.HasValue)
                    {
                        flight.AircraftId = aircraftId.Value;
                    }
                    if (airportDepartureId.HasValue)
                    {
                        flight.AirportDepartureId = airportDepartureId.Value;
                    }
                    if (airportDestinationId.HasValue)
                    {
                        flight.AirportDestinationId = airportDestinationId.Value;
                    }

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.UpdateFlight(flight));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains("FOREIGN KEY constraint failed", exception.Message);
                    Assert.Empty(context.Flight);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(99, 99)]
        public void Should_UpdateFlight_ThrowsFunctionalException_When_SameDepartureAndDestinationAirport(int airportDepartureId, int airportDestinationId)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Flight>().Update(It.IsAny<Flight>())).Verifiable();
            flightsContextMock.Setup(m => m.SaveChanges()).Verifiable();

            var flightRepositoy = new FlightRepository(flightsContextMock.Object);

            var flight = new Flight
            {
                AirportDepartureId = airportDepartureId,
                AirportDestinationId = airportDestinationId,
            };

            var exception = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.UpdateFlight(flight));

            Assert.Equal(ExceptionCodes.SameDepartureAndDestinationAirportCode, exception.Code);
            Assert.Contains(string.Format(ExceptionCodes.SameDepartureAndDestinationAirportFormatMessage, airportDepartureId, airportDestinationId), exception.Message);
            flightsContextMock.Verify(m => m.Set<Flight>().Update(It.IsAny<Flight>()), Times.Never);
            flightsContextMock.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-4)]
        public void Should_UpdateFlight_ThrowsException_When_FlightRepositoyReturnNegativeOrZeroValue(int updateCount)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Flight>().Update(It.IsAny<Flight>())).Verifiable();
            flightsContextMock.Setup(m => m.SaveChanges()).Returns(updateCount);

            var flightRepositoy = new FlightRepository(flightsContextMock.Object);

            var resultException = Assert.Throws<FlightPlanningTechnicalException>(
                () => flightRepositoy.UpdateFlight(new Flight { AirportDepartureId = 1, AirportDestinationId = 2 }));

            Assert.Equal(ExceptionCodes.NoChangeMessage, resultException.Message);
            Assert.Equal(ExceptionCodes.NoChangeCode, resultException.Code);

            flightsContextMock.Verify(m => m.Set<Flight>().Update(It.IsAny<Flight>()), Times.Once);
            flightsContextMock.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void Should_UpdateFlight_ThrowsFunctionalException_When_AirportOrAircraft_IsNotFound(bool updateAircraft, bool updateAirportDeparture, bool updateAirportDestination)
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


                int aircraftId = 0, airportDepartureId = 0, airportDestinationId = 0;

                using (var context = new FlightsDbContext(options))
                {
                    var expectedUpdateCount = 0;
                    if (updateAirportDeparture)
                    {
                        var departure = new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15, Longitude = 60.1546, Name = "Name_1" };
                        context.Airport.Add(departure);
                        airportDepartureId = departure.Id;
                        expectedUpdateCount++;
                    }
                    if (updateAirportDestination)
                    {
                        var destination = new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BB2", Icao = "BBB2", Latitude = 22.15, Longitude = 70.9146, Name = "Name_2" };
                        context.Airport.Add(destination);
                        airportDestinationId = destination.Id;
                        expectedUpdateCount++;
                    }
                    if (updateAircraft)
                    {
                        var aircraft = new Aircraft { Name = "Name_1", FuelCapacity = 1000, FuelConsumption = 1, Speed = 800, TakeOffEffort = 0.2 };
                        context.Aircraft.Add(aircraft);
                        aircraftId = aircraft.Id;
                        expectedUpdateCount++;
                    }

                    var updateCount = context.SaveChanges();

                    Assert.Equal(expectedUpdateCount, updateCount);
                }

                using (var context = new FlightsDbContext(options))
                {
                    var flightRepositoy = new FlightRepository(context);

                    var flight = new Flight
                    {
                        AirportDepartureId = airportDepartureId,
                        AirportDestinationId = airportDestinationId,
                        AircraftId = aircraftId
                    };

                    var exception = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.UpdateFlight(flight));

                    Assert.Equal(ExceptionCodes.InvalidEntityCode, exception.Code);
                    Assert.Contains("FOREIGN KEY constraint failed", exception.Message);
                }

                using (var context = new FlightsDbContext(options))
                {
                    Assert.Empty(context.Flight);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void Should_UpdateFlight_Update_OldFlight()
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

                int aircraftId_2, airportDepartureId_2, airportDestinationId_2, flightToUpdateId;

                using (var context = new FlightsDbContext(options))
                {
                    var departure = new Airport { City = "city_1", CountryName = "CountryName_1", Iata = "AAA", Icao = "AAAA", Latitude = 12.15, Longitude = 60.1546, Name = "Name_1" };
                    var destination = new Airport { City = "city_2", CountryName = "CountryName_2", Iata = "BB2", Icao = "BBB2", Latitude = 22.15, Longitude = 70.9146, Name = "Name_2" };

                    var departure_2 = new Airport { City = "departure_2", CountryName = "departure_2", Iata = "111", Icao = "1111", Name = "departure_2" };
                    var destination_2 = new Airport { City = "destination_2", CountryName = "destination_2", Iata = "222", Icao = "2222", Name = "destination_2" };

                    context.Airport.Add(departure);
                    context.Airport.Add(destination);
                    context.Airport.Add(departure_2);
                    context.Airport.Add(destination_2);

                    var aircraft = new Aircraft { Name = "Name_1", FuelCapacity = 1000, FuelConsumption = 1, Speed = 800, TakeOffEffort = 0.2 };
                    context.Aircraft.Add(aircraft);

                    var aircraft_2 = new Aircraft { Name = "aircraft_2", FuelCapacity = 800, FuelConsumption = 2, Speed = 500, TakeOffEffort = 0.3 };
                    context.Aircraft.Add(aircraft_2);

                    var flightToUpdate = new Flight { UpdateDate = new DateTime(2019, 10, 02, 10, 12, 36), Aircraft = aircraft, AirportDeparture = departure, AirportDestination = destination };
                    context.Flight.Add(flightToUpdate);

                    var changeCount = context.SaveChanges();

                    Assert.Equal(7, changeCount);
                    aircraftId_2 = aircraft_2.Id;
                    airportDepartureId_2 = departure_2.Id;
                    airportDestinationId_2 = destination_2.Id;
                    flightToUpdateId = flightToUpdate.Id;
                }

                using (var context = new FlightsDbContext(options))
                {
                    var flightRepositoy = new FlightRepository(context);

                    var flight = new Flight
                    {
                        Id = flightToUpdateId,
                        AirportDepartureId = airportDepartureId_2,
                        AirportDestinationId = airportDestinationId_2,
                        AircraftId = aircraftId_2
                    };

                    flightRepositoy.UpdateFlight(flight);
                }

                using (var context = new FlightsDbContext(options))
                {
                    var flight = new FlightRepository(context).GetFlightById(flightToUpdateId);

                    Assert.NotNull(flight);
                    Assert.NotNull(flight.UpdateDate);
                    Assert.Equal(DateTime.Now, flight.UpdateDate.Value, TimeSpan.FromMinutes(2));
                    
                    var aircraft = flight.Aircraft;
                    Assert.NotNull(aircraft);
                    Assert.NotEqual(0, aircraft.Id);
                    Assert.Equal("aircraft_2", aircraft.Name);
                    Assert.Equal(800, aircraft.FuelCapacity);
                    Assert.Equal(2, aircraft.FuelConsumption);
                    Assert.Equal(500, aircraft.Speed);
                    Assert.Equal(0.3, aircraft.TakeOffEffort);

                    var airportDeparture = flight.AirportDeparture;
                    Assert.NotNull(airportDeparture);
                    Assert.NotEqual(0, airportDeparture.Id);
                    Assert.Equal("departure_2", airportDeparture.City);
                    Assert.Equal("departure_2", airportDeparture.CountryName);
                    Assert.Equal("111", airportDeparture.Iata);
                    Assert.Equal("1111", airportDeparture.Icao);
                    Assert.Equal("departure_2", airportDeparture.Name);

                    var airportDestination = flight.AirportDestination;
                    Assert.NotNull(airportDestination);
                    Assert.NotEqual(0, airportDestination.Id);
                    Assert.Equal("destination_2", airportDestination.City);
                    Assert.Equal("destination_2", airportDestination.CountryName);
                    Assert.Equal("222", airportDestination.Iata);
                    Assert.Equal("2222", airportDestination.Icao);
                    Assert.Equal("destination_2", airportDestination.Name);

                    Assert.Single(context.Flight);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region DeleteFlight

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-33)]
        public void Should_DeleteFlight_ReturnWithoutCallingRepository_When_FlightId_IsNegativeOrZero(int flightId)
        {
            var flightsContextMock = new Mock<FlightsDbContext>();

            flightsContextMock.Setup(m => m.Set<Flight>().Remove(It.IsAny<Flight>())).Verifiable();

            var flightRepositoy = new FlightRepository(flightsContextMock.Object);

            flightRepositoy.DeleteFlight(flightId);

            flightsContextMock.Verify(m => m.Set<Flight>().Remove(It.IsAny<Flight>()), Times.Never);
        }

        [Fact]
        public void Should_DeleteFlight_ThrowsException_When_FlightIsNotFound()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
              .UseInMemoryDatabase(databaseName: "Should_DeleteFlight_ThrowsException_When_FlightIsNotFound")
              .Options;

            using (var context = new FlightsDbContext(options))
            {
                var flightRepositoy = new FlightRepository(context);

                var flightId = 10;

                var resultException = Assert.Throws<FlightPlanningFunctionalException>(() => flightRepositoy.DeleteFlight(flightId));

                Assert.Equal(string.Format(ExceptionCodes.EntityToDeleteNotFoundFormatMessage, "Flight", flightId), resultException.Message);
                Assert.Equal(ExceptionCodes.EntityToDeleteNotFoundCode, resultException.Code);
            }
        }

        [Fact]
        public void Should_DeleteFlight_DeleteFlightFromDatabase()
        {
            var options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_DeleteFlight_DeleteFlightFromDatabase")
                .Options;

            InsertSampleData(options);

            int flightToDeleteId;

            using (var context = new FlightsDbContext(options))
            {
                var flight = context.Flight.FirstOrDefault();
                Assert.NotNull(flight);
                flightToDeleteId = flight.Id;
            }

            using (var context = new FlightsDbContext(options))
            {
                var flightRepositoy = new FlightRepository(context);
                flightRepositoy.DeleteFlight(flightToDeleteId);
            }

            using (var context = new FlightsDbContext(options))
            {
                var flight = context.Flight.SingleOrDefault(a => a.Id == flightToDeleteId);

                Assert.Null(flight);
                Assert.Equal(1, context.Flight.Count());
            }
        }

        #endregion
    }
}
