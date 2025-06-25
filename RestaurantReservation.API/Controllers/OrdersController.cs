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
            return orders.Any()
                ? Ok(_mapper.Map<IEnumerable<OrderDto>>(orders))
                : NotFound("No orders found.");
        }

        [HttpGet("{id:int}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order is not null
                ? Ok(_mapper.Map<OrderDto>(order))
                : NotFound($"Order {id} not found.");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto updateDto)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order is null) return NotFound($"Order {id} not found.");

            _mapper.Map(updateDto, order);
            await _repository.UpdateAsync(order);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOrder(int id, JsonPatchDocument<OrderUpdateDto> patchDoc)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order is null) return NotFound($"Order {id} not found.");

            var patchTarget = _mapper.Map<OrderUpdateDto>(order);
            patchDoc.ApplyTo(patchTarget, ModelState);

            if (!TryValidateModel(patchTarget)) return BadRequest(ModelState);

            _mapper.Map(patchTarget, order);
            await _repository.UpdateAsync(order);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order is null) return NotFound($"Order {id} not found.");

            await _repository.DeleteAsync(order);
            return NoContent();
        }
    }
}