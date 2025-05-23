using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.Employees;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enum;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Restaurant> _restaurantRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public EmployeesController(
            IRepository<Employee> repository,
            IMapper mapper,
            IRepository<Restaurant> restaurantRepository,
            IRepository<Order> orderRepository)
        {
            _repository = repository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _repository.GetAllAsync();
            if (employees == null || !employees.Any()) return NotFound("No employees found.");
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        [HttpGet("{id:int}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return NotFound($"Employee with ID {id} not found.");
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeCreationDto dto)
        {
            if (dto == null) return BadRequest();

            var restaurantExists = await _restaurantRepository.ExistsAsync(dto.RestaurantId);
            if (!restaurantExists)
                return NotFound($"Restaurant with ID {dto.RestaurantId} not found.");

            var employee = _mapper.Map<Employee>(dto);
            var addedEmployee = await _repository.CreatAsync(employee);
            var returnedEmployee = _mapper.Map<EmployeeDto>(addedEmployee);

            return CreatedAtRoute("GetEmployee", new { id = returnedEmployee.EmployeeId }, returnedEmployee);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, EmployeeUpdateDto dto)
        {
            if (dto == null) return BadRequest();

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound($"Employee with ID {id} not found.");

            var restaurantExists = await _restaurantRepository.ExistsAsync(dto.RestaurantId);
            if (!restaurantExists)
                return NotFound($"Restaurant with ID {dto.RestaurantId} not found.");

            var employee = _mapper.Map<Employee>(dto);
            employee.EmployeeId = id;
            await _repository.UpdateAsync(employee);

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> PatchEmployee(int id, JsonPatchDocument<EmployeeUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound($"Employee with ID {id} not found.");

            var toPatch = _mapper.Map<EmployeeUpdateDto>(existing);
            patchDoc.ApplyTo(toPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var employee = _mapper.Map<Employee>(toPatch);
            employee.EmployeeId = id;
            await _repository.UpdateAsync(employee);

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return NotFound($"Employee with ID {id} not found.");

            await _repository.DeleteAsync(employee);
            return NoContent();
        }

        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetManagers()
        {
            var employees = await _repository.GetAllAsync();
            if (employees == null || !employees.Any()) return NotFound("No employees found.");

            var managers = employees.Where(e => e.Position == EmployeePosition.Manager).ToList();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(managers));
        }

        [HttpGet("{employeeId:int}/average-order-amount")]
        public async Task<ActionResult<decimal>> GetAverageOrderAmount(int employeeId)
        {
            var employee = await _repository.GetByIdAsync(employeeId);
            if (employee == null) return NotFound($"Employee with ID {employeeId} not found.");

            var orders = await _orderRepository.GetAllAsync();
            var employeeOrders = orders.Where(o => o.EmployeeId == employeeId).ToList();
            if (!employeeOrders.Any()) return NotFound($"No orders found for employee with ID {employeeId}.");

            return Ok(employeeOrders.Average(o => o.TotalAmount));
        }
    }
}
