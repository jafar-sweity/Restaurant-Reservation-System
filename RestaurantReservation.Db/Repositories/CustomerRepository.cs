using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        private readonly RestaurantReservationDbContext _context;

        public CustomerRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<List<Customer>> GetCustomersWithReservationsAbovePartySizeAsync(int PartySize)
        {
            return await _context.Customers.FromSql($"EXEC sp_FindCustomersWithPartySizeLargerThan {PartySize}").ToListAsync();
        }
    }
}
