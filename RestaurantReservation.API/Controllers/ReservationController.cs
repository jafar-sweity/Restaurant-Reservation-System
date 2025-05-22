using AutoMapper;
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
    }
}
