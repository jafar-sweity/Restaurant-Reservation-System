using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(RestaurantReservationDbContext context) : base(context)
        {
        }
    }
}
