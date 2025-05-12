using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models.Views
{
    public class ReservationDetailsView
    {
            public int ReservationId { get; set; }
            public DateTime ReservationDate { get; set; }
            public int PartySize { get; set; }
            public string CustomerFirstName { get; set; }
            public string CustomerLastName { get; set; }
            public string RestaurantName { get; set; }
    }
}
