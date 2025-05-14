namespace RestaurantReservation.Db.Models.Entities
{
    public class Table
    {
        public int tableId { get; set; }
        public int restaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int capacity { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
    }
}
