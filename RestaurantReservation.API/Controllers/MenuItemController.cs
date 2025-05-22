using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    public class MenuItemController : ControllerBase
    {
        private readonly IRepository<MenuItem> _repository;
        private readonly IMapper _mapper;

        public MenuItemController(IRepository<MenuItem> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            var menuItems = await _repository.GetAllAsync();
            if (menuItems == null || !menuItems.Any())
            {
                return NotFound("No menu items found.");
            }
            return Ok(menuItems);
        }
    }
}
