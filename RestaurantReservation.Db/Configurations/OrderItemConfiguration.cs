﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(i => i.OrderItemId).HasColumnName("order_item_id");
            builder.Property(i => i.OrderId).HasColumnName("order_id");
            builder.Property(i => i.ItemId).HasColumnName("item_id");
            builder.Property(i => i.Quantity).HasColumnName("quantity");

            builder.HasOne(i => i.Item)
              .WithMany(o => o.OrderItems)
              .HasForeignKey(i => i.ItemId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(o => o.Order)
              .WithMany(oi => oi.OrderItems)
              .HasForeignKey(o => o.OrderId)
              .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

}
