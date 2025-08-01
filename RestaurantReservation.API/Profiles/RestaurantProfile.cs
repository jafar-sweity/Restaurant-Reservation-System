﻿using AutoMapper;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantDto>();
            CreateMap<RestaurantCreationDto, Restaurant>();
            CreateMap<RestaurantUpdateDto, Restaurant>().ReverseMap();
        }
    }
}
