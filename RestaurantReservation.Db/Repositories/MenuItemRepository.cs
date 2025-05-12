using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories
{
    public class MenuItemRepository : Repository<MenuItem>
    {
        public MenuItemRepository(RestaurantReservationDbContext context) : base(context)
        {
        }
    }
}
