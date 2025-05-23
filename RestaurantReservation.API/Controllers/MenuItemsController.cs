using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuItemsController : ControllerBase
    {
        private readonly IRepository<MenuItem> _repository;
        private readonly IRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;

        public MenuItemsController(IRepository<MenuItem> repository, IMapper mapper, IRepository<Restaurant> restaurantRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
        {
            var menuItems = await _repository.GetAllAsync();
            if (menuItems == null || !menuItems.Any())
            {
                return NotFound("No menu items found.");
            }

            return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
        }

        [HttpGet("{id:int}", Name = "GetMenuItem")]
        public async Task<ActionResult<MenuItemDto>> GetMenuItem(int id)
        {
            var menuItem = await _repository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} not found.");
            }

            return Ok(_mapper.Map<MenuItemDto>(menuItem));
        }

        [HttpPost]
        public async Task<ActionResult<MenuItemDto>> CreateMenuItem(MenuItemCreationDto menuItemCreationDto)
        {
            var exists = await _restaurantRepository.ExistsAsync(menuItemCreationDto.RestaurantId);
            if (!exists)
            {
                return NotFound($"Restaurant with ID {menuItemCreationDto.RestaurantId} not found.");
            }

            var menuItem = _mapper.Map<MenuItem>(menuItemCreationDto);
            var createdMenuItem = await _repository.CreatAsync(menuItem);
            var menuItemDto = _mapper.Map<MenuItemDto>(createdMenuItem);

            return CreatedAtRoute("GetMenuItem", new { id = menuItemDto.ItemId }, menuItemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItemUpdateDto menuItemUpdateDto)
        {
            var existingMenuItem = await _repository.GetByIdAsync(id);
            if (existingMenuItem == null)
                return NotFound();

            var restaurantExists = await _restaurantRepository.ExistsAsync(menuItemUpdateDto.RestaurantId);
            if (!restaurantExists)
            {
                return NotFound(new { Message = "Restaurant not found." });
            }

            _mapper.Map(menuItemUpdateDto, existingMenuItem);
            await _repository.UpdateAsync(existingMenuItem);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateMenuItem(int id, JsonPatchDocument<MenuItemUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var menuItem = await _repository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            var menuItemToPatch = _mapper.Map<MenuItemUpdateDto>(menuItem);
            patchDoc.ApplyTo(menuItemToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(menuItemToPatch, menuItem);
            await _repository.UpdateAsync(menuItem);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _repository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} not found.");
            }

            await _repository.DeleteAsync(menuItem);
            return NoContent();
        }
    }
}
