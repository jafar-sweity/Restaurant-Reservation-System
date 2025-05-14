using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Views;

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

        public async Task<List<EmployeeRestaurantDetailsView>> GetEmployeesWithRestaurantDetailsAsync()
        {
            return await _context.EmployeeRestaurantDetailsView.ToListAsync();
        }
    }
}
