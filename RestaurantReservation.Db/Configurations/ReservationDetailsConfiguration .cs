using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Configurations
{
    class ReservationDetailsConfiguration : IEntityTypeConfiguration<ReservationDetailsView>
    {
        public void Configure(EntityTypeBuilder<ReservationDetailsView> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_ReservationDetails");
            builder.Property(e => e.ReservationId).HasColumnName("Reservation_Id");
            builder.Property(e => e.ReservationDate).HasColumnName("Reservation_Date");
            builder.Property(e => e.PartySize).HasColumnName("Party_Size");
        }
    }
}
