using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c=> c.CustomerId).HasColumnName("customer_id");
            builder.Property(c => c.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
           builder.Property(c => c.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            builder.Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("email");
            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(13)
                .HasColumnName("phone_number");
        }
    }
}
