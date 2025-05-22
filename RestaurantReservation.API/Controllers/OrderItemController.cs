using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly IRepository<OrderItem> _repository;
        private readonly IMapper _mapper;

        public OrderItemController(IRepository<OrderItem> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetAllOrderItems()
        {
            var orderItems = await _repository.GetAllAsync();
            if (orderItems == null || !orderItems.Any())
            {
                return NotFound("No order items found.");
            }
            var orderItemDtos = _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
            return Ok(orderItemDtos);
        }

        [HttpGet("{id:int}", Name = "GetOrderItem")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItem(int id)
        {
            var orderItem = await _repository.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound($"Order item with ID {id} not found.");
            }
            var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);
            return Ok(orderItemDto);
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItemByOrderId(int orderId)
        {
            var orderItems = await _repository.GetAllAsync();
            var orderItem = orderItems.FirstOrDefault(oi => oi.OrderId == orderId);
            if (orderItem == null)
            {
                return NotFound($"Order item with Order ID {orderId} not found.");
            }
            var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);
            return Ok(orderItemDto);
        }

    }
}
