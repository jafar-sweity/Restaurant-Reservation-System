using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.Employees;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Restaurant> _reservationRepository;

        private readonly IMapper _mapper;

        public EmployeeController(IRepository<Employee> repository, IMapper mapper , IRepository<Restaurant> repository1)
        {
            _repository = repository;
            _reservationRepository = repository1;
            _mapper = mapper;
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
    }
}
