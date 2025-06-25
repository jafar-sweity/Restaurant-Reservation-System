﻿namespace RestaurantReservation.API.Models.Customer
{
    public class CustomerUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
