using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.Property(t => t.tableId).HasColumnName("table_id");
            builder.Property(t => t.restaurantId).HasColumnName("restaurant_id");
            builder.Property(t => t.capacity).HasColumnName("capacity").IsRequired();

            builder.HasOne(r => r.Restaurant)
              .WithMany(t => t.Tables)
              .HasForeignKey(r => r.restaurantId)
              .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
