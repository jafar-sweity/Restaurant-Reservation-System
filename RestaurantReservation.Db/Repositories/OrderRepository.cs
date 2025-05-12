using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderRepository(RestaurantReservationDbContext context) : base(context) { }

        public async Task<List<Order>> ListOrdersAndMenuItems(int ReservationId)
        {
            return  await _context.Orders
                                .Include(o => o.OrderItems)
                                .ThenInclude(oi => oi.Item)
                                .Where(o => o.ReservationId == ReservationId).ToListAsync();

        }
    }
}
