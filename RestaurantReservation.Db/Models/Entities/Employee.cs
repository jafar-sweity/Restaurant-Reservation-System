using RestaurantReservation.Db.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models.Entities
{
    public class Employee
    {
        public Employee()
        {
            oredres = new List<Order>();
        }
        public int EmployeeId { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public EmployeePosition Position { get; set; }
        public List<Order> oredres { get; set; }

    }
}
