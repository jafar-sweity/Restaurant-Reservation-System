namespace RestaurantReservation.API.Models.Order
{
    public class OrderCreationDto
    {
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
    }
}
