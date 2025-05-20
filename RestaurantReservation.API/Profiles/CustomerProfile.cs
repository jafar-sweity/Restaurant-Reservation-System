using AutoMapper;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile() 
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
