using FlightPlanning.Services.Flights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Table { get; }

        T GetById(int id);
        
        int Insert(T entity);

        int Update(T entity);

        int Delete(T entity);

    }
}
