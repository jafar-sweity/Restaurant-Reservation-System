using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    public class OrdersController : Controller
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

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreationDto orderCreationDto)
        {
            if (orderCreationDto == null)
            {
                return BadRequest();
            }
            var order = _mapper.Map<Order>(orderCreationDto);
            var addedOrder = await _repository.CreatAsync(order);
            var returnedOrder = _mapper.Map<OrderDto>(addedOrder);
            return CreatedAtRoute("GetOrder", new { id = returnedOrder.OrderId }, returnedOrder);
        }
    }
}
