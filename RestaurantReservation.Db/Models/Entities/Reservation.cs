using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models.Entities
{
    public class Reservation
    {
        public Reservation()
        {
            Orders = new List<Order>();
        }
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int customerId { get; set; }
        public Customer Customer { get; set; }
        public int PartySize { get; set; }
        public List<Order> Orders { get; set; }
        public DateTime reservation_date { get; set; }
    }
}
