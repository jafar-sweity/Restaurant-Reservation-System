using AutoMapper;
using RestaurantReservation.API.Models.Table;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableDto>();
            CreateMap<TableCreationDto, Table>();
            CreateMap<TableUpdateDto, Table>().ReverseMap();
        }
    }
}
