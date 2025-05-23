using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/orders/{orderId}/order-items")]
    [ApiController]
    [Authorize]
    public class OrderItemController : ControllerBase
    {
        private readonly IRepository<OrderItem> _repository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderItemController(
            IRepository<OrderItem> repository,
            IRepository<Order> orderRepository,
            IMapper mapper)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetAllOrderItems(int orderId)
        {
            var orderItems = await _repository.GetAllAsync();
            var filtered = orderItems.Where(oi => oi.OrderId == orderId).ToList();

            if (!filtered.Any())
                return NotFound($"No order items found for Order ID {orderId}.");

            return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(filtered));
        }

        [HttpGet("{id:int}", Name = "GetOrderItem")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItem(int orderId, int id)
        {
            var orderItem = await _repository.GetByIdAsync(id);

            if (orderItem == null || orderItem.OrderId != orderId)
                return NotFound($"Order item with ID {id} not found for Order ID {orderId}.");

            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(int orderId, OrderItemCreationDto orderItemCreationDto)
        {
            if (orderItemCreationDto == null)
                return BadRequest();

            var orderExists = await _orderRepository.ExistsAsync(orderId);
            if (!orderExists)
                return NotFound($"Order with ID {orderId} not found.");

            var orderItem = _mapper.Map<OrderItem>(orderItemCreationDto);
            orderItem.OrderId = orderId;

            var addedOrderItem = await _repository.CreatAsync(orderItem);
            var result = _mapper.Map<OrderItemDto>(addedOrderItem);

            return CreatedAtRoute("GetOrderItem", new { orderId = orderId, id = result.OrderItemId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrderItem(int orderId, int id, OrderItemDto orderItemDto)
        {
            if (orderItemDto == null || id != orderItemDto.OrderItemId)
                return BadRequest();

            var orderItem = await _repository.GetByIdAsync(id);

            if (orderItem == null || orderItem.OrderId != orderId)
                return NotFound($"Order item with ID {id} not found for Order ID {orderId}.");

            _mapper.Map(orderItemDto, orderItem);
            await _repository.UpdateAsync(orderItem);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdateOrderItem(int orderId, int id, JsonPatchDocument<OrderItemDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var orderItem = await _repository.GetByIdAsync(id);

            if (orderItem == null || orderItem.OrderId != orderId)
                return NotFound($"Order item with ID {id} not found for Order ID {orderId}.");

            var itemToPatch = _mapper.Map<OrderItemDto>(orderItem);
            patchDoc.ApplyTo(itemToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(itemToPatch, orderItem);
            await _repository.UpdateAsync(orderItem);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int id)
        {
            var orderItem = await _repository.GetByIdAsync(id);

            if (orderItem == null || orderItem.OrderId != orderId)
                return NotFound($"Order item with ID {id} not found for Order ID {orderId}.");

            await _repository.DeleteAsync(orderItem);
            return NoContent();
        }
    }
}
