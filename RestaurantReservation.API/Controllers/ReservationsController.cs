using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IRepository<Reservation> _repository;
        private readonly IRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;

        public ReservationsController(
            IRepository<Reservation> repository,
            IMapper mapper,
            IRepository<Restaurant> restaurantRepository,
            IRepository<Customer> customerRepository,
            IRepository<Order> orderRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
        {
            var reservations = await _repository.GetAllAsync();
            return reservations.Any()
                ? Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations))
                : NotFound("No reservations found.");
        }

        [HttpGet("{id:int}", Name = "GetReservation")]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            return reservation is not null
                ? Ok(_mapper.Map<ReservationDto>(reservation))
                : NotFound($"Reservation {id} not found.");
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(ReservationCreationDto creationDto)
        {
            if (!await _restaurantRepository.ExistsAsync(creationDto.RestaurantId))
                return NotFound($"Restaurant {creationDto.RestaurantId} not found.");

            var reservation = _mapper.Map<Reservation>(creationDto);
            var createdReservation = await _repository.CreatAsync(reservation);
            var result = _mapper.Map<ReservationDto>(createdReservation);

            return CreatedAtRoute(nameof(GetReservation), new { id = result.ReservationId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReservation(int id, ReservationUpdateDto updateDto)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation is null) return NotFound($"Reservation {id} not found.");

            _mapper.Map(updateDto, reservation);
            await _repository.UpdateAsync(reservation);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateReservation(int id, JsonPatchDocument<ReservationUpdateDto> patchDoc)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation is null) return NotFound($"Reservation {id} not found.");

            var patchTarget = _mapper.Map<ReservationUpdateDto>(reservation);
            patchDoc.ApplyTo(patchTarget, ModelState);

            if (!TryValidateModel(patchTarget)) return BadRequest(ModelState);

            _mapper.Map(patchTarget, reservation);
            await _repository.UpdateAsync(reservation);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation is null) return NotFound($"Reservation {id} not found.");

            await _repository.DeleteAsync(reservation);
            return NoContent();
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByCustomerId(int customerId)
        {
            if (!await _customerRepository.ExistsAsync(customerId))
                return NotFound($"Customer {customerId} not found.");

            var reservations = await _repository.GetAllAsync();
            var customerReservations = reservations.Where(r => r.customerId == customerId).ToList();

            return customerReservations.Any()
                ? Ok(_mapper.Map<IEnumerable<ReservationDto>>(customerReservations))
                : NotFound($"No reservations found for customer {customerId}.");
        }

        [HttpGet("{reservationId:int}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByReservationId(int reservationId)
        {
            if (!await _repository.ExistsAsync(reservationId))
                return NotFound($"Reservation {reservationId} not found.");

            var orders = await _orderRepository.GetAllAsync();
            var reservationOrders = orders.Where(o => o.ReservationId == reservationId).ToList();

            return reservationOrders.Any()
                ? Ok(_mapper.Map<IEnumerable<OrderDto>>(reservationOrders))
                : NotFound($"No orders found for reservation {reservationId}.");
        }

        [HttpGet("{reservationId:int}/menu-items")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItemsByReservationId(int reservationId)
        {
            if (!await _repository.ExistsAsync(reservationId))
                return NotFound($"Reservation {reservationId} not found.");

            var orders = await _orderRepository.GetAllAsync();
            var reservationOrders = orders.Where(o => o.ReservationId == reservationId).ToList();

            if (!reservationOrders.Any())
                return NotFound($"No orders found for reservation {reservationId}.");

            var menuItems = reservationOrders.SelectMany(o => o.OrderItems).ToList();
            return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
        }
    }
}