using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContext : DbContext
    {

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // i will do it later 

        }
    }
}
