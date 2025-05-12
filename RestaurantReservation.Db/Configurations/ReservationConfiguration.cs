using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(r => r.ReservationId).HasColumnName("reservation_id");
            builder.Property(r => r.customerId).HasColumnName("customer_id");
            builder.Property(r => r.RestaurantId).HasColumnName("restaurant_id");
            builder.Property(r => r.TableId).HasColumnName("table_id");
            builder.Property(r => r.PartySize).HasColumnName("party_size");
            builder.Property(r => r.reservation_date).HasColumnName("reservation_date");

            builder.HasOne(c => c.Customer)
              .WithMany(r => r.Reservations)
              .HasForeignKey(c => c.customerId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(re => re.Restaurant)
              .WithMany(r => r.Reservations)
              .HasForeignKey(re => re.RestaurantId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(t => t.Table)
              .WithMany(r => r.Reservations)
              .HasForeignKey(t => t.TableId)
              .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
