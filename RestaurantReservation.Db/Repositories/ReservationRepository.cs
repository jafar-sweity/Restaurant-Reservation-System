using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Repositories
{
    public class ReservationRepository : Repository<Reservation>
    {
        private readonly RestaurantReservationDbContext _context;

        public ReservationRepository(RestaurantReservationDbContext context) : base(context) {
            _context = context;
        }

        public async Task<List<Reservation>> GetReservationsByCustomerIdAsync(int customerId)
        {
            return await _context.Reservations.Where(r => r.customerId == customerId).ToListAsync();
        }

        public async Task<List<ReservationDetailsView>> GetReservationDetailsAsync()
        {
            return await _context.ReservationDetailsView.ToListAsync();
        }
    }
}
