namespace RestaurantReservation.API.Models.Order
{
    public class OrderUpdateDto
    {
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public int TotalAmount { get; set; }
    }
}
