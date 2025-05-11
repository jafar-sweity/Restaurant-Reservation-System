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
            Reservations = new List<Reservation>();
        }
        public int EmployeeId { get; set; }
        public int RestaurantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Position { get; set; }
        public List<Reservation> Reservations { get; set; }

    }
}
