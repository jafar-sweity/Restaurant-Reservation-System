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
        public OrderRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
