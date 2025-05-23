using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Table;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : Controller
    { 
        private readonly IRepository<Table> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<Restaurant> _restaurantRepository;

        public TableController(IRepository<Table> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<TableDto>> GetAll()
        {
            var tables = await _repository.GetAllAsync();
            var tableDtos = _mapper.Map<IEnumerable<TableDto>>(tables);
            return Ok(tableDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TableDto>> GetById(int id)
        {
            var table = await _repository.GetByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            var tableDto = _mapper.Map<TableDto>(table);
            return Ok(tableDto);
        }

    }
}
