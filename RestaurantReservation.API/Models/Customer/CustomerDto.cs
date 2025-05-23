namespace RestaurantReservation.API.Models.Customer
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }= string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
