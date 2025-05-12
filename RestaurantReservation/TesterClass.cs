using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation
{
    public class TesterClass
    {
        private readonly RestaurantReservationDbContext _context;

         public TesterClass(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task Test()
        {
            await TestListManagers();
            await TestGetReservationDetailsAsync();
            await TestGetEmployeesWithRestaurantDetailsAsync();
            await TestCalculateRestaurantRevenueAsync();
            await TestGetCustomersWithReservationsAbovePartySizeAsync();



        }

        private async Task TestListManagers()
        {
            var employeeRepo = new EmployeeRepository(_context);
            var managers = await employeeRepo.ListManagersAsync();

            Console.WriteLine("List of Managers:");
            foreach (var manager in managers)
            {
                Console.WriteLine($"Manager: {manager.FirstName} {manager.LastName}");
            }
        }
        private async Task TestGetReservationDetailsAsync()
        {
            var reservationRepo = new ReservationRepository(_context);
            var reservations = await reservationRepo.GetReservationDetailsAsync();

            if (reservations != null && reservations.Count > 0)
            {
                Console.WriteLine("\nReservations retrieved successfully!");
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"Reservation ID: {reservation.ReservationId}, Date: {reservation.ReservationDate}, Party Size: {reservation.PartySize}, Customer: {reservation.CustomerFirstName} {reservation.CustomerLastName}, Restaurant: {reservation.RestaurantName}");
                }
            }
            else
            {
                Console.WriteLine("No reservations found.");
            }
        }

        private async Task TestGetEmployeesWithRestaurantDetailsAsync()
        {
            var employeeRepo = new EmployeeRepository(_context);
            var employees = await employeeRepo.GetEmployeesWithRestaurantDetailsAsync();
            if (employees != null && employees.Count > 0)
            {
                Console.WriteLine("\nEmployees with Restaurant Details retrieved successfully!");
                foreach (var employee in employees)
                {
                    Console.WriteLine($"Employee: {employee.EmployeeFirstName} {employee.EmployeeLastName}, Restaurant: {employee.RestaurantName}");
                }
            }
            else
            {
                Console.WriteLine("No employees found.");
            }
        }

        private async Task TestCalculateRestaurantRevenueAsync()
        {
            var restaurantRepo = new RestaurantRepository(_context);
            int restaurantId = 1;
            decimal revenue = await restaurantRepo.CalculateRestaurantRevenueAsync(restaurantId);

            Console.WriteLine($"\nRevenue for Restaurant ID {restaurantId}: {revenue}");


        }

        private async Task TestGetCustomersWithReservationsAbovePartySizeAsync()
        {
            var customerRepo = new CustomerRepository(_context);

            int partySize = 1;

            var customersWithLargeReservations = await customerRepo.GetCustomersWithReservationsAbovePartySizeAsync(partySize);

            if (customersWithLargeReservations.Count > 0)
            {
                Console.WriteLine($"\nCustomers with reservations larger than {partySize}:");
                foreach (var customer in customersWithLargeReservations)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                }
            }
            else
            {
                Console.WriteLine($"No customers found with reservations larger than {partySize}.");
            }
        }
    }
}

