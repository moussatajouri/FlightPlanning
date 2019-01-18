﻿using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using FlightPlanning.Services.Flights.Transverse;
using FlightPlanning.Services.Flights.Transverse.Exception;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.DataAccess
{
    public class EntityFrameworkRepositoryTests
    {
        private static Mock<DbSet<BaseEntity>> CreateDbSetMock(IEnumerable<BaseEntity> elements)
        {
            var elementsAsQueryable = elements.AsQueryable();

            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.As<IQueryable<BaseEntity>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            mockSet.As<IQueryable<BaseEntity>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            mockSet.As<IQueryable<BaseEntity>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            mockSet.As<IQueryable<BaseEntity>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return mockSet;
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99)]
        public void Should_GetById_Return_Null_When_Negative_Id(int entityId)
        {
            var userContextMock = new Mock<FlightsDbContext>();
            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            var entity = efReository.GetById(entityId);

            Assert.Null(entity);
            userContextMock.Verify(b => b.Set<BaseEntity>(), Times.Never);
        }

        [Fact]
        public void Should_GetById_Return_Null_When_entityId_IsNot_Found()
        {
            var data = new List<BaseEntity>
            {
                new BaseEntity { Id = 1 },
                new BaseEntity { Id = 2},
                new BaseEntity { Id = 3 },
            }.AsQueryable();

            var mockSet = CreateDbSetMock(data);
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);
            var entity = efReository.GetById(5);

            Assert.Null(entity);

            userContextMock.Verify(b => b.Set<BaseEntity>(), Times.Once);
        }

        [Fact]
        public void Should_GetById_Return_Null_When_Db_Is_Empty()
        {
            var mockSet = CreateDbSetMock(new List<BaseEntity>());
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);
            var entity = efReository.GetById(2);

            Assert.Null(entity);

            userContextMock.Verify(b => b.Set<BaseEntity>(), Times.Once);
        }

        [Fact]
        public void Should_GetById_Return_Entity_When_entityId_Is_Found()
        {
            var data = new List<BaseEntity>
            {
                new BaseEntity { Id = 1 },
                new BaseEntity { Id = 2},
                new BaseEntity { Id = 3 },
            };

            var mockSet = CreateDbSetMock(data);
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);
            var entity = efReository.GetById(2);

            Assert.NotNull(entity);
            Assert.Equal(2, entity.Id);

            userContextMock.Verify(b => b.Set<BaseEntity>(), Times.Once);
        }

        [Fact]
        public void Should_Insert_Throw_Exception_When_Entity_IsNull()
        {
            var efReository = new EntityFrameworkRepository<BaseEntity>(null);

            Assert.Throws<ArgumentNullException>(()=> efReository.Insert(null));
        }

        [Fact]
        public void Should_Insert_AddEntityAndSaveChanges_When_Entity_IsNotNull()
        {
            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.Setup(m => m.Add(It.IsAny<BaseEntity>())).Verifiable();
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);
            userContextMock.Setup(x => x.SaveChanges()).Verifiable();

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            efReository.Insert(new BaseEntity());

            mockSet.Verify(m => m.Add(It.IsAny<BaseEntity>()), Times.Once);
            userContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Should_Insert_ThrowException_When_HasOccurred()
        {
            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.Setup(m => m.Add(It.IsAny<BaseEntity>())).Verifiable();
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);

            var expectedExceptionMessage = "exception insert test";
            var exception = new Exception(expectedExceptionMessage);
            userContextMock.Setup(x => x.SaveChanges()).Throws(exception);

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            var exceptionResult = Assert.Throws<FlightPlanningTechnicalException>(() => efReository.Insert(new BaseEntity()));

            Assert.NotNull(exceptionResult);
            Assert.Equal(ExceptionCodes.EntityFrameworkRepository, exceptionResult.Code);
            Assert.NotNull(exceptionResult.InnerException);
            Assert.Equal(expectedExceptionMessage, exceptionResult.InnerException.Message);
        }

        [Fact]
        public void Should_Update_Throw_Exception_When_Entity_IsNull()
        {
            var efReository = new EntityFrameworkRepository<BaseEntity>(null);

            Assert.Throws<ArgumentNullException>(() => efReository.Update(null));
        }

        [Fact]
        public void Should_Update_SaveChanges_When_Entity_IsNotNull()
        {
            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.Setup(m => m.Add(It.IsAny<BaseEntity>())).Verifiable();
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);
            userContextMock.Setup(x => x.SaveChanges()).Verifiable();

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            efReository.Update(new BaseEntity());

            userContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Should_Updtae_ThrowException_When_HasOccurred()
        {
            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.Setup(m => m.Add(It.IsAny<BaseEntity>())).Verifiable();
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);

            var expectedExceptionMessage = "exception update test";
            var exception = new Exception(expectedExceptionMessage);
            userContextMock.Setup(x => x.SaveChanges()).Throws(exception);

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            var exceptionResult = Assert.Throws<FlightPlanningTechnicalException>(() => efReository.Update(new BaseEntity()));

            Assert.NotNull(exceptionResult);
            Assert.Equal(ExceptionCodes.EntityFrameworkRepository, exceptionResult.Code);
            Assert.NotNull(exceptionResult.InnerException);
            Assert.Equal(expectedExceptionMessage, exceptionResult.InnerException.Message);
        }


        [Fact]
        public void Should_Delete_Throw_Exception_When_Entity_IsNull()
        {
            var efReository = new EntityFrameworkRepository<BaseEntity>(null);

            Assert.Throws<ArgumentNullException>(() => efReository.Delete(null));
        }

        [Fact]
        public void Should_Delete_RemoveEntityAndSaveChanges_When_Entity_IsNotNull()
        {
            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.Setup(m => m.Add(It.IsAny<BaseEntity>())).Verifiable();
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);
            userContextMock.Setup(x => x.SaveChanges()).Verifiable();

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            efReository.Delete(new BaseEntity());

            mockSet.Verify(m => m.Remove(It.IsAny<BaseEntity>()), Times.Once);
            userContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Should_Delete_ThrowException_When_HasOccurred()
        {
            var mockSet = new Mock<DbSet<BaseEntity>>();
            mockSet.Setup(m => m.Add(It.IsAny<BaseEntity>())).Verifiable();
            var userContextMock = new Mock<FlightsDbContext>();
            userContextMock.Setup(x => x.Set<BaseEntity>()).Returns(mockSet.Object);

            var expectedExceptionMessage = "exception delete test";
            var exception = new Exception(expectedExceptionMessage);
            userContextMock.Setup(x => x.SaveChanges()).Throws(exception);

            var efReository = new EntityFrameworkRepository<BaseEntity>(userContextMock.Object);

            var exceptionResult = Assert.Throws<FlightPlanningTechnicalException>(() => efReository.Delete(new BaseEntity()));

            Assert.NotNull(exceptionResult);
            Assert.Equal(ExceptionCodes.EntityFrameworkRepository, exceptionResult.Code);
            Assert.NotNull(exceptionResult.InnerException);
            Assert.Equal(expectedExceptionMessage, exceptionResult.InnerException.Message);
        }
    }
}
