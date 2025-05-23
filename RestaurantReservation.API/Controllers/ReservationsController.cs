using AutoMapper;
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
    public class ReservationsController : ControllerBase
    {
        private readonly IRepository<Reservation> _repository;
        private readonly IRepository<Restaurant> _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;

        public ReservationsController(IRepository<Reservation> repository, IMapper mapper, IRepository<Restaurant> reservationRepository, IRepository<Customer> customerRepository,IRepository <Order> orederRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _orderRepository = orederRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
        {
            var reservations = await _repository.GetAllAsync();
            if (reservations == null || !reservations.Any())
            {
                return NotFound("No reservations found.");
            }
            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);
            return Ok(reservationDtos);
        }

        [HttpGet("{id:int}", Name = "GetReservation")]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }
            var reservationDto = _mapper.Map<ReservationDto>(reservation);
            return Ok(reservationDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(ReservationCreationDto reservationCreationDto)
        {
            if (reservationCreationDto == null)
            {
                return BadRequest("Reservation data is null.");
            }

            var existingRestaurant = await _reservationRepository.ExistsAsync(reservationCreationDto.RestaurantId);

            if (!existingRestaurant)
            {
                return NotFound($"Restaurant with ID {reservationCreationDto.RestaurantId} not found.");
            }

            var reservation = _mapper.Map<Reservation>(reservationCreationDto);
            var createdReservation = await _repository.CreatAsync(reservation);
            var reservationDto = _mapper.Map<ReservationDto>(createdReservation);

            return CreatedAtRoute("GetReservation", new { id = reservationDto.ReservationId }, reservationDto);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReservation(int id, ReservationUpdateDto reservationUpdateDto)
        {
            var existingReservation = await _repository.GetByIdAsync(id);
            if (existingReservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }
            var updatedReservation = _mapper.Map<Reservation>(reservationUpdateDto);
            await _repository.UpdateAsync(updatedReservation);
            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateReservation(int id, JsonPatchDocument<ReservationUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document.");
            }
            var existingReservation = await _repository.GetByIdAsync(id);
            if (existingReservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }
            var reservationToPatch = _mapper.Map<ReservationUpdateDto>(existingReservation);
            patchDocument.ApplyTo(reservationToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedReservation = _mapper.Map<Reservation>(reservationToPatch);
            await _repository.UpdateAsync(updatedReservation);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var existingReservation = await _repository.GetByIdAsync(id);

            if (existingReservation == null)
            {
                return NotFound($"Reservation with ID {id} not found.");
            }

            await _repository.DeleteAsync(existingReservation);

            return NoContent();
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByCustomerId(int customerId)
        {
            var existCustomer = _customerRepository.ExistsAsync(customerId);

            if (!existCustomer.Result)
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            var reservations = await _repository.GetAllAsync();
            var customerReservations = reservations.Where(r => r.customerId == customerId).ToList();

            if (customerReservations == null || !customerReservations.Any())
            {
                return NotFound($"No reservations found for customer with ID {customerId}.");
            }

            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(customerReservations);

            return Ok(reservationDtos);
        }

        [HttpGet("{reservationId:int}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByReservationId(int reservationId)
        {
            var existingReservation = await _repository.GetByIdAsync(reservationId);

            if (existingReservation == null)
            {
                return NotFound($"Reservation with ID {reservationId} not found.");
            }

            var orders = await _orderRepository.GetAllAsync();
            var reservationOrders = orders.Where(o => o.ReservationId == reservationId).ToList();

            if (reservationOrders == null || !reservationOrders.Any())
            {
                return NotFound($"No orders found for reservation with ID {reservationId}.");
            }

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(reservationOrders);
            return Ok(orderDtos);
        }
        //GET /api/reservations/{reservationId}/menu-items - List ordered menu items for a reservation.
        [HttpGet("{reservationId:int}/menu-items")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItemsByReservationId(int reservationId)
        {
            var existingReservation = await _repository.GetByIdAsync(reservationId);

            if (existingReservation == null)
            {
                return NotFound($"Reservation with ID {reservationId} not found.");
            }

            var orders = await _orderRepository.GetAllAsync();
            var reservationOrders = orders.Where(o => o.ReservationId == reservationId).ToList();

            if (reservationOrders == null || !reservationOrders.Any())
            {
                return NotFound($"No orders found for reservation with ID {reservationId}.");
            }

            var menuItems = reservationOrders.SelectMany(o => o.OrderItems).ToList();
            var menuItemDtos = _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);

            return Ok(menuItemDtos);
        }
    }
}