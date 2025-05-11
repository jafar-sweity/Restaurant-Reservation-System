using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models.Entities
{
    public class MenuItem
    {
        public MenuItem()
        {
            OrderItems = new List<OrderItem>();
        }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
