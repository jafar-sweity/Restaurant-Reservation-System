using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enum;
using System;

namespace RestaurantReservation.Db.Extensions
{
    public static class DataSeeding
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(GetCustomers());
            modelBuilder.Entity<Employee>().HasData(GetEmployees());
            modelBuilder.Entity<MenuItem>().HasData(GetMenuItems());
            modelBuilder.Entity<Order>().HasData(GetOrders());
            modelBuilder.Entity<OrderItem>().HasData(GetOrderItems());
            modelBuilder.Entity<Reservation>().HasData(GetReservations());
            modelBuilder.Entity<Restaurant>().HasData(GetRestaurants());
            modelBuilder.Entity<Table>().HasData(GetTables());
        }

        private static Customer[] GetCustomers() => new Customer[]
        {
            new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123456789" },
            new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "987654321" },
            new Customer { CustomerId = 3, FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com", PhoneNumber = "555666777" },
            new Customer { CustomerId = 4, FirstName = "Alice", LastName = "Green", Email = "alice.green@example.com", PhoneNumber = "999888777" },
            new Customer { CustomerId = 5, FirstName = "Charlie", LastName = "White", Email = "charlie.white@example.com", PhoneNumber = "111222333" },
        };

        public static Employee[] GetEmployees() => new Employee[]
        {
            new Employee { EmployeeId = 1, RestaurantId = 1, FirstName = "Mark", LastName = "Johnson", Position = EmployeePosition.Manager },
            new Employee { EmployeeId = 2, RestaurantId = 2, FirstName = "Sara", LastName = "Williams", Position = EmployeePosition.Waiter },
            new Employee { EmployeeId = 3, RestaurantId = 1, FirstName = "Tom", LastName = "Lee", Position = EmployeePosition.Chef },
            new Employee { EmployeeId = 4, RestaurantId = 3, FirstName = "Nancy", LastName = "Davis", Position = EmployeePosition.Waiter },
            new Employee { EmployeeId = 5, RestaurantId = 2, FirstName = "Jake", LastName = "Wilson", Position = EmployeePosition.Manager },
        };

        private static MenuItem[] GetMenuItems() => new MenuItem[]
        {
            new MenuItem { ItemId = 1, RestaurantId = 1, Name = "Pizza", Description = "Cheese Pizza", Price = 10.0m },
            new MenuItem { ItemId = 2, RestaurantId = 1, Name = "Burger", Description = "Beef Burger", Price = 8.0m },
            new MenuItem { ItemId = 3, RestaurantId = 2, Name = "Pasta", Description = "Spaghetti Bolognese", Price = 12.0m },
            new MenuItem { ItemId = 4, RestaurantId = 3, Name = "Salad", Description = "Caesar Salad", Price = 7.0m },
            new MenuItem { ItemId = 5, RestaurantId = 2, Name = "Soup", Description = "Tomato Soup", Price = 6.0m },
        };

        private static Restaurant[] GetRestaurants() => new Restaurant[]
        {
            new Restaurant { RestaurantId = 1, Name = "Italian Bistro", Address = "123 Main St", PhoneNumber = "555-1234", OpeningHours = "9 AM - 9 PM" },
            new Restaurant { RestaurantId = 2, Name = "American Grill", Address = "456 Oak St", PhoneNumber = "555-5678", OpeningHours = "11 AM - 10 PM" },
            new Restaurant { RestaurantId = 3, Name = "French Cafe", Address = "789 Pine St", PhoneNumber = "555-9876", OpeningHours = "8 AM - 8 PM" },
        };

        private static Table[] GetTables() => new Table[]
        {
            new Table { tableId = 1, restaurantId = 1, capacity = 4 },
            new Table { tableId = 2, restaurantId = 1, capacity = 2 },
            new Table { tableId = 3, restaurantId = 2, capacity = 6 },
            new Table { tableId = 4, restaurantId = 2, capacity = 4 },
            new Table { tableId = 5, restaurantId = 3, capacity = 4 },
        };

        private static Reservation[] GetReservations() => new Reservation[]
        {
            new Reservation { ReservationId = 1, customerId = 1, RestaurantId = 1, TableId = 1, reservation_date = new DateTime(2024, 05, 01, 18, 00, 00), PartySize = 4 },
            new Reservation { ReservationId = 2, customerId = 2, RestaurantId = 2, TableId = 3, reservation_date = new DateTime(2024, 05, 01, 19, 00, 00), PartySize = 6 },
            new Reservation { ReservationId = 3, customerId = 3, RestaurantId = 3, TableId = 5, reservation_date = new DateTime(2024, 05, 01, 20, 00, 00), PartySize = 4 },
            new Reservation { ReservationId = 4, customerId = 4, RestaurantId = 1, TableId = 2, reservation_date = new DateTime(2024, 05, 01, 17, 00, 00), PartySize = 2 },
            new Reservation { ReservationId = 5, customerId = 5, RestaurantId = 2, TableId = 4, reservation_date = new DateTime(2024, 05, 01, 21, 00, 00), PartySize = 4 },
        };

        private static Order[] GetOrders() => new Order[]
        {
            new Order { OrderId = 1, ReservationId = 1, EmployeeId = 1, OrderDate = new DateTime(2024, 05, 01, 18, 30, 00), TotalAmount = 50 },
            new Order { OrderId = 2, ReservationId = 2, EmployeeId = 2, OrderDate = new DateTime(2024, 05, 01, 19, 30, 00), TotalAmount = 60 },
            new Order { OrderId = 3, ReservationId = 3, EmployeeId = 3, OrderDate = new DateTime(2024, 05, 01, 20, 30, 00), TotalAmount = 70 },
            new Order { OrderId = 4, ReservationId = 4, EmployeeId = 4, OrderDate = new DateTime(2024, 05, 01, 17, 30, 00), TotalAmount = 40 },
            new Order { OrderId = 5, ReservationId = 5, EmployeeId = 5, OrderDate = new DateTime(2024, 05, 01, 21, 30, 00), TotalAmount = 30 },
        };

        private static OrderItem[] GetOrderItems() => new OrderItem[]
        {
            new OrderItem { OrderItemId = 1, OrderId = 1, ItemId = 1, Quantity = 2 },
            new OrderItem { OrderItemId = 2, OrderId = 1, ItemId = 2, Quantity = 1 },
            new OrderItem { OrderItemId = 3, OrderId = 2, ItemId = 3, Quantity = 3 },
            new OrderItem { OrderItemId = 4, OrderId = 3, ItemId = 4, Quantity = 2 },
            new OrderItem { OrderItemId = 5, OrderId = 4, ItemId = 5, Quantity = 2 },
        };
    }
}
