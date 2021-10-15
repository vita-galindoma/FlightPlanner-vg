using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace Flight.Planner.Services
{
    public class DbServiceExtended: DbService, IDbServiceExtended
    {
        public DbServiceExtended(IFlightPlannerDbContext context) : base(context)
        {
        }

        public void DeleteAll<T>() where T : Entity
        {
            _context.Set<T>().RemoveRange(_context.Set<T>());
            _context.SaveChanges();
        }
    }
}
