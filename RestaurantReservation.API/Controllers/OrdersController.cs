using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly IMapper _mapper;

        public OrdersController(IRepository<Order> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _repository.GetAllAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("{id:int}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
        {
            var existingOrder = await _repository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            var updatedOrder = _mapper.Map<Order>(orderUpdateDto);
            await _repository.UpdateAsync(updatedOrder);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOrder(int id, JsonPatchDocument<OrderUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }
            var existingOrder = await _repository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            var orderToPatch = _mapper.Map<OrderUpdateDto>(existingOrder);
            patchDocument.ApplyTo(orderToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedOrder = _mapper.Map<Order>(orderToPatch);
            await _repository.UpdateAsync(updatedOrder);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var existingOrder = await _repository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            await _repository.DeleteAsync(existingOrder);
            return NoContent();
        }
    }
}
