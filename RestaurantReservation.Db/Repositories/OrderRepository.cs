using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;

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

        public async Task<decimal> CalculateAverageOrderAmount(int EmployeeId)
        {
            var avg = await _context.Orders.Where(o => o.EmployeeId == EmployeeId).AverageAsync(o => o.TotalAmount);
            return (decimal)avg;
        }
    }
}
