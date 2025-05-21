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

        [HttpGet("{id:int}", Name = "GetCustomer")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerCreationDto customerCreationDto)
        {
            if (customerCreationDto == null)
            {
                return BadRequest();
            }

            var customer = _mapper.Map<Customer>(customerCreationDto);
            var addedCustomer = await _repository.CreatAsync(customer);
            var returnedCustomer = _mapper.Map<CustomerDto>(addedCustomer);

            return CreatedAtRoute(nameof(GetCustomer), new { id = returnedCustomer.CustomerId }, returnedCustomer);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCustomer(int id, CustomerUpdateDto customerUpdateDto)
        {
            var existingCustomer = await _repository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return BadRequest("Customer ID mismatch.");
            }

            _mapper.Map(customerUpdateDto, existingCustomer); 
            await _repository.UpdateAsync(existingCustomer);
            return NoContent();
        }
    }
}
