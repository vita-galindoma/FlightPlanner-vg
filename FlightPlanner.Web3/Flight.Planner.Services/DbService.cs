﻿using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace Flight.Planner.Services
{
    public class DbService : IDbService
    {
        protected readonly IFlightPlannerDbContext _context;

        public DbService(IFlightPlannerDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            return _context.Set<T>();
        }

        public void Create<T>(T entity) where T : Entity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : Entity
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public T GetById<T>(int id) where T : Entity
        {
            return _context.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public void Update<T>(T entity) where T : Entity
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
