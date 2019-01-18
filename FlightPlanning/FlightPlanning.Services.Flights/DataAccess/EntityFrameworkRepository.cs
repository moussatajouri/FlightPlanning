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
    public class EntityFrameworkRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly FlightsDbContext _context;

        private DbSet<T> _entities { get; set; }

        public virtual IQueryable<T> Table => Entities;

        public virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        public EntityFrameworkRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return Entities.FirstOrDefault(e => e.Id == id);
        }

        public int Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Add(entity);

                return _context.SaveChanges();
            }
            catch (Exception exp)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.EntityFrameworkRepository, exp);
            }
        }             
        
        public int Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
                
            try
            {
                Entities.Update(entity);
                return _context.SaveChanges();
            }
            catch (Exception exp)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.EntityFrameworkRepository, exp);
            }
        }

        public int Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Remove(entity);

                return _context.SaveChanges();
            }
            catch (Exception exp)
            {
                throw new FlightPlanningTechnicalException(ExceptionCodes.EntityFrameworkRepository, exp);
            }
        }
    }
}
