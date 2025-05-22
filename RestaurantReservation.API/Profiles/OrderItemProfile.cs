using AutoMapper;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class OrderItemProfile: Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
