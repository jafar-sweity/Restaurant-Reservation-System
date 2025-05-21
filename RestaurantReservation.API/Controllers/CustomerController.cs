using AutoMapper;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomerController(IRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _repository.GetAllAsync();

            if (customers == null || !customers.Any())
            {
                return NotFound("No customers found.");
            }

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDtos);
        }

    }
}
