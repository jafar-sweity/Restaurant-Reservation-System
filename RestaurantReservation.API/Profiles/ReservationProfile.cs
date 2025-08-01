﻿using AutoMapper;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationDto>();
            CreateMap<ReservationCreationDto, Reservation>();
            CreateMap<ReservationUpdateDto, Reservation>().ReverseMap();
        }
    }
}
