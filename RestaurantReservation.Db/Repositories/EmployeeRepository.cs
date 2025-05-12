using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        private readonly RestaurantReservationDbContext _context;

        public EmployeeRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Employee>> ListManagersAsync()
        { 
            return _context.Employees.Where(e=>e.Position == Models.Enum.EmployeePosition.Manager)
                .ToListAsync();
        }
    }
}
