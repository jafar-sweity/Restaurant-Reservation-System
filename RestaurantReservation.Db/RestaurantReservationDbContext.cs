﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db.Configurations;
using RestaurantReservation.Db.Extensions;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContext : DbContext
    {
        public RestaurantReservationDbContext(DbContextOptions<RestaurantReservationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ReservationDetailsView> ReservationDetailsView { get; set; }
        public DbSet<EmployeeRestaurantDetailsView> EmployeeRestaurantDetailsView { get; set; }
        public decimal CalculateRestaurantRevenue(int restaurantId) => throw new Exception();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfiguration).Assembly);
            modelBuilder.Seed();
            modelBuilder.HasDbFunction(

                typeof(RestaurantReservationDbContext).GetMethod(
                nameof(CalculateRestaurantRevenue),
                new[] { typeof(int) })).HasName("fn_CalculateRestaurantRevenue");
        }
    }
}
