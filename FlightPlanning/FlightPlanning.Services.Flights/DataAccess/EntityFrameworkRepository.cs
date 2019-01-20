using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using FlightPlanning.Services.Flights.Transverse.Exception;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public abstract class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly FlightsDbContext _dbContext;

        public EntityFrameworkRepository(FlightsDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public TEntity GetById(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return _dbContext.Set<TEntity>()
               .AsNoTracking()
               .FirstOrDefault(e => e.Id == id);
        }

        public int Insert(TEntity entity)
        {
            return TryDbAction(entity, () =>
            {
                _dbContext.Set<TEntity>().Add(entity);
                return _dbContext.SaveChanges();
            });
        }

        public int Update(TEntity entity)
        {
            return TryDbAction(entity, () =>
            {
                _dbContext.Set<TEntity>().Update(entity);
                return _dbContext.SaveChanges();
            });
        }

        public int Delete(TEntity entity)
        {
            return TryDbAction(entity, () =>
            {
                _dbContext.Set<TEntity>().Remove(entity);
                return _dbContext.SaveChanges();
            });
        }

        internal int TryDbAction(TEntity entity, Func<int> acquire)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                return acquire();
            }
            catch (DbUpdateException dbUpdateException)
            {
                var errorMessage = $"Exception.Message : { dbUpdateException.Message }" +
                    dbUpdateException.InnerException ?? $"{Environment.NewLine}InnerException.Message : { dbUpdateException.InnerException.Message }";

                throw new FlightPlanningFunctionalException(ExceptionCodes.InvalidEntityCode, errorMessage);
            }
            catch (Exception exp)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.EntityFrameworkRepository, exp);
            }
        }
    }
}
