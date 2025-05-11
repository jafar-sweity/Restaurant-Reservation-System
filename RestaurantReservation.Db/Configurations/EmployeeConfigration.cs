using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Configurations
{
    public class EmployeeConfigration  :IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.EmployeeId).HasColumnName("employee_id");
            builder.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            builder.Property(e => e.FirstName).HasMaxLength(50) .HasColumnName("first_name");
            builder.Property(e => e.LastName).HasMaxLength(50).HasColumnName("last_name");
            builder.Property(e => e.Position).HasMaxLength(10).HasColumnName("position").IsRequired();

            builder.HasOne(r=>r.Restaurant).WithMany(e => e.Employees)
                .HasForeignKey(e => e.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
