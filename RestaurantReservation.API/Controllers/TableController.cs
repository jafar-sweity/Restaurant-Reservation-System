using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Table;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("{id}", Name = "GetTable")]
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

        [HttpPost]
        public async Task<ActionResult<TableDto>> Create(TableCreationDto tableCreationDto)
        {
            var restaurant = await _restaurantRepository.ExistsAsync(tableCreationDto.RestaurantId);

            if (!restaurant)
            {
                return NotFound($"Restaurant with ID {tableCreationDto.RestaurantId} not found.");
            }

            var table = _mapper.Map<Table>(tableCreationDto);
            var createdTable = await _repository.CreatAsync(table);
            var tableDto = _mapper.Map<TableDto>(createdTable);

            return CreatedAtRoute("GetTable", new { id = tableDto.TableId }, tableDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TableUpdateDto tableUpdateDto)
        {
            var existingTable = await _repository.GetByIdAsync(id);
            if (existingTable == null)
            {
                return NotFound($"Table with ID {id} not found.");
            }

            var restaurant = await _restaurantRepository.ExistsAsync(tableUpdateDto.RestaurantId);

            if (!restaurant)
            {
                return NotFound($"Restaurant with ID {tableUpdateDto.RestaurantId} not found.");
            }

            var table = _mapper.Map<Table>(tableUpdateDto);
            await _repository.UpdateAsync(table);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<TableUpdateDto> patchDocument)
        {
            var existingTable = await _repository.GetByIdAsync(id);

            if (existingTable == null)
            {
                return NotFound($"Table with ID {id} not found.");
            }

            var tableToPatch = _mapper.Map<TableUpdateDto>(existingTable);
            patchDocument.ApplyTo(tableToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = await _restaurantRepository.ExistsAsync(tableToPatch.RestaurantId);

            if (!restaurant)
            {
                return NotFound($"Restaurant with ID {tableToPatch.RestaurantId} not found.");
            }

            var table = _mapper.Map<Table>(tableToPatch);
            await _repository.UpdateAsync(table);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingTable = await _repository.GetByIdAsync(id);

            if (existingTable == null)
            {
                return NotFound($"Table with ID {id} not found.");
            }

            await _repository.DeleteAsync(existingTable);

            return NoContent();
        }
    }
}
