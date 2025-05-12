using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IRepository<OrderItem>
    {
        public OrderItemRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
