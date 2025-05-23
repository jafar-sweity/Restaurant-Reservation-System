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
    public class RestaurantsController : ControllerBase
    {
        private readonly IRepository<Restaurant> _repository;
        private readonly IMapper _mapper;

        public RestaurantsController(IRepository<Restaurant> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
        {
            var restaurants = await _repository.GetAllAsync();
            return restaurants.Any()
                ? Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants))
                : NotFound("No restaurants found.");
        }

        [HttpGet("{id:int}", Name = "GetRestaurant")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurant(int id)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            return restaurant is not null
                ? Ok(_mapper.Map<RestaurantDto>(restaurant))
                : NotFound($"Restaurant {id} not found.");
        }

        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant(RestaurantCreationDto creationDto)
        {
            var restaurant = _mapper.Map<Restaurant>(creationDto);
            var createdRestaurant = await _repository.CreatAsync(restaurant);
            var result = _mapper.Map<RestaurantDto>(createdRestaurant);

            return CreatedAtRoute(nameof(GetRestaurant),
                new { id = result.RestaurantId },
                result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRestaurant(int id, RestaurantUpdateDto updateDto)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant is null) return NotFound($"Restaurant {id} not found.");

            _mapper.Map(updateDto, restaurant);
            await _repository.UpdateAsync(restaurant);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateRestaurant(
            int id,
            JsonPatchDocument<RestaurantUpdateDto> patchDoc)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant is null) return NotFound($"Restaurant {id} not found.");

            var patchTarget = _mapper.Map<RestaurantUpdateDto>(restaurant);
            patchDoc.ApplyTo(patchTarget, ModelState);

            if (!TryValidateModel(patchTarget)) return BadRequest(ModelState);

            _mapper.Map(patchTarget, restaurant);
            await _repository.UpdateAsync(restaurant);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant is null) return NotFound($"Restaurant {id} not found.");

            await _repository.DeleteAsync(restaurant);
            return NoContent();
        }
    }
}