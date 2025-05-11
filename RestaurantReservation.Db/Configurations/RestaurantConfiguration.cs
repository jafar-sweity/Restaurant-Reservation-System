using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.Property(r => r.RestaurantId).HasColumnName("restaurant_id");
            builder.Property(r => r.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            builder.Property(r => r.Address).HasColumnName("address").HasMaxLength(100);
            builder.Property(r => r.PhoneNumber).HasColumnName("phone_number").HasMaxLength(13).IsRequired();
            builder.Property(r => r.OpeningHours).HasColumnName("opening_hours");
        }
    }
}
