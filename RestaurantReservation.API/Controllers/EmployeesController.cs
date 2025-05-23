using AutoMapper;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Restaurant> _reservationRepository;
        private readonly IRepository<Order> _orderRepository;

        private readonly IMapper _mapper;

        public EmployeesController(IRepository<Employee> repository, IMapper mapper , IRepository<Restaurant> repository1, IRepository<Order> orderRepository)
        {
            _repository = repository;
            _reservationRepository = repository1;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _repository.GetAllAsync();
            if (employees == null || !employees.Any())
            {
                return NotFound("No employees found.");
            }
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("{id:int}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeCreationDto employeeCreationDto)
        {
            if (employeeCreationDto == null)
            {
                return BadRequest();
            }
            var existingRestaurant = await _reservationRepository.ExistsAsync(employeeCreationDto.RestaurantId);
            if  (!existingRestaurant)
            {
                return NotFound($"Restaurant with ID {employeeCreationDto.RestaurantId} not found.");
            }
            var employee = _mapper.Map<Employee>(employeeCreationDto);
            var addedEmployee = await _repository.CreatAsync(employee);
            var returnedEmployee = _mapper.Map<EmployeeDto>(addedEmployee);
            return CreatedAtRoute("GetEmployee", new { id = returnedEmployee.EmployeeId }, returnedEmployee);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, EmployeeUpdateDto employeeUpdateDto)
        {
            if (employeeUpdateDto == null)
            {
                return BadRequest();
            }
            var existingEmployee = await _repository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            var existingRestaurant = await _reservationRepository.ExistsAsync(employeeUpdateDto.RestaurantId);
            if (!existingRestaurant)
            {
                return NotFound($"Restaurant with ID {employeeUpdateDto.RestaurantId} not found.");
            }
            var employee = _mapper.Map<Employee>(employeeUpdateDto);
            employee.EmployeeId = id;
            await _repository.UpdateAsync(employee);
            var updatedEmployee = _mapper.Map<EmployeeDto>(employee);
            return Ok(updatedEmployee);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> PatchEmployee(int id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var existingEmployee = await _repository.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(existingEmployee);
            patchDocument.ApplyTo(employeeToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _mapper.Map<Employee>(employeeToPatch);
            employee.EmployeeId = id;
            await _repository.UpdateAsync(employee);
            var updatedEmployee = _mapper.Map<EmployeeDto>(employee);
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _repository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            await _repository.DeleteAsync(existingEmployee);
            return NoContent();
        }

        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetManagers()
        {
            var employees = await _repository.GetAllAsync();
            if (employees == null || !employees.Any())
            {
                return NotFound("No employees found.");
            }

            var managers = employees.Where(e => e.Position == EmployeePosition.Manager).ToList();
            var toReturn = _mapper.Map<IEnumerable<EmployeeDto>>(managers);
            return Ok(toReturn);
        }

        [HttpGet("{employeeId:int}/average-order-amount")]
        public async Task<ActionResult<decimal>> GetAverageOrderAmount(int employeeId)
        {
            var employee = await _repository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                return NotFound($"Employee with ID {employeeId} not found.");
            }

            var orders = await _orderRepository.GetAllAsync();
            var employeeOrders = orders.Where(o => o.EmployeeId == employeeId).ToList();

            if (!employeeOrders.Any())
            {
                return NotFound($"No orders found for employee with ID {employeeId}.");
            }

            var averageOrderAmount = employeeOrders.Average(o => o.TotalAmount);
            return Ok(averageOrderAmount);
        }
    }
}
