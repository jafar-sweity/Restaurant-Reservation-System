using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Restaurant;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("{id:int}", Name = "GetRestaurant")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurant(int id)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return Ok(restaurantDto);
        }

        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant(RestaurantCreationDto restaurantCreationDto)
        {
            if (restaurantCreationDto == null)
            {
                return BadRequest("Restaurant data is null.");
            }

            var restaurant = _mapper.Map<Restaurant>(restaurantCreationDto);
            var createdRestaurant = await _repository.CreatAsync(restaurant);
            var restaurantDto = _mapper.Map<RestaurantDto>(createdRestaurant);

            return CreatedAtRoute("GetRestaurant", new { id = restaurantDto.RestaurantId }, restaurantDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto)
        {
            if (restaurantUpdateDto == null)
            {
                return BadRequest("Restaurant data is null.");
            }
            var existingRestaurant = await _repository.GetByIdAsync(id);
            if (existingRestaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }
            var restaurant = _mapper.Map<Restaurant>(restaurantUpdateDto);
            restaurant.RestaurantId = id;
            await _repository.UpdateAsync(restaurant);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateRestaurant(int id, JsonPatchDocument<RestaurantUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }
            var existingRestaurant = await _repository.GetByIdAsync(id);
            if (existingRestaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }
            var restaurantToPatch = _mapper.Map<RestaurantUpdateDto>(existingRestaurant);
            patchDocument.ApplyTo(restaurantToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var restaurant = _mapper.Map<Restaurant>(restaurantToPatch);
            restaurant.RestaurantId = id;
            await _repository.UpdateAsync(restaurant);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var existingRestaurant = await _repository.GetByIdAsync(id);
            if (existingRestaurant == null)
            {
                return NotFound($"Restaurant with ID {id} not found.");
            }
            await _repository.DeleteAsync(existingRestaurant);
            return NoContent();
        }
    }
}
