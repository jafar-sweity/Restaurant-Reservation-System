using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IRepository<Reservation> _repository;
        private readonly IRepository<Restaurant> _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationController(IRepository<Reservation> repository, IMapper mapper, IRepository<Restaurant> reservationRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
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
    }
}
