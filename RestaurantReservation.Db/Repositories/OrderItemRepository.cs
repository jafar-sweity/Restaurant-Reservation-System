using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{

    public class OrderItemRepository : Repository<OrderItem>, IRepository<OrderItem>
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderItemRepository(RestaurantReservationDbContext context) : base(context) { }

        public async Task<List<MenuItem>> ListOrderedMenuItems(int ReservationId)
        {
            return await _context.OrderItems
                                 .Include(oi => oi.Item)
                                 .Include(oi => oi.Order)
                                 .Where(oi => oi.Order.ReservationId == ReservationId).Select(i => i.Item).ToListAsync();
        }
    }
}
