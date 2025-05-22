using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRepository<Restaurant> _repository;
        private readonly IMapper _mapper;
        public RestaurantController(IRepository<Restaurant> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
        {
            var restaurants = await _repository.GetAllAsync();
            if (restaurants == null || !restaurants.Any())
            {
                return NotFound("No restaurants found.");
            }
            var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
            return Ok(restaurantDtos);
        }
    }
}
