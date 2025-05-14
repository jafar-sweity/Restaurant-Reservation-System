using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class RestaurantRepository : Repository<Restaurant>
    {
        private readonly RestaurantReservationDbContext _context;

        public RestaurantRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }

        [DbFunction("fn_CalculateRestaurantRevenue", "dbo")]
        public Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {
            var revenu = _context.Restaurants
                                 .Where(r => r.RestaurantId == restaurantId)
                                 .Select(r=>_context.CalculateRestaurantRevenue(r.RestaurantId))
                                 .FirstOrDefaultAsync();
            return revenu;
        }
    }
}
