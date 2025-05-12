using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
