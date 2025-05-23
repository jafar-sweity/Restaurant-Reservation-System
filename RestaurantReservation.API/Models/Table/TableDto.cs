namespace RestaurantReservation.API.Models.Table
{
    public class TableDto
    {
        public int TableId { get; set; }
        public int RestaurantId { get; set; }
        public int Capacity { get; set; }
    }
}
