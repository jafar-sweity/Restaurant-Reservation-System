# ??? Restaurant Reservation System - .NET Core (EF Core + API)

A full-stack backend system built with **.NET Core**, **Entity Framework Core**, and **ASP.NET Core Web API**, focused on managing restaurant reservations, customers, orders, and employee tasks using a layered architecture.

---

## ?? Project Structure

- RestaurantReservation

    - RestaurantReservation.Console    # Console app to test EF Core logic
    - RestaurantReservation.Db         # Class library for models, DbContext, repositories, and logic
    - RestaurantReservation.API        # ASP.NET Core Web API project (controllers, endpoints)





---

## ?? Technologies Used

- .NET 5.0+
- Entity Framework Core
- SQL Server
- ASP.NET Core Web API
- AutoMapper
- FluentValidation
- JWT Authentication
- Swagger for API documentation

---

## ??? Database Setup

1. Create a database: `RestaurantReservationCore` using **SSMS**.
2. Use EF Core Code-First:
   - Create `RestaurantReservationDbContext`.
   - Define entities with proper relationships and constraints.
   - Add migrations.
   - Seed at least **5 records** in each table.

---

## ??? Key Features

### ?? EF Core (Console Application)

- ? CRUD for all entities (Create, Read, Update, Delete).
- ? Asynchronous LINQ queries.
- ? Specialized Queries:
  - `ListManagers()`
  - `GetReservationsByCustomer(customerId)`
  - `ListOrdersAndMenuItems(reservationId)`
  - `ListOrderedMenuItems(reservationId)`
  - `CalculateAverageOrderAmount(employeeId)`
- ? Database Views:
  - View with all reservations including customer and restaurant info.
  - View with employees and their restaurant details.
- ? Database Functions:
  - Calculate total revenue by restaurant.
- ? Stored Procedures:
  - Get customers with reservation party size above threshold.

### ?? ASP.NET Core Web API

- ? CRUD API Controllers for all entities.
- ? Specific endpoints:
  - `GET /api/employees/managers`
  - `GET /api/reservations/customer/{customerId}`
  - `GET /api/reservations/{reservationId}/orders`
  - `GET /api/reservations/{reservationId}/menu-items`
  - `GET /api/employees/{employeeId}/average-order-amount`
- ? Authentication & Authorization using JWT.
- ? Input validation & error handling with user-friendly messages.
- ? Swagger API Documentation with:
  - Request/response examples
  - Status codes
  - Security info (JWT Bearer)

---

## ?? Authentication

Secure API using JWT:
- Protected endpoints require Bearer token.
- Implemented with middleware and claims-based authorization.

---

## ?? Git Best Practices

- ? Meaningful commit messages
- ? Push after each phase
- ? Modular structure
- ? Use of repositories and clean architecture

---

## ?? How to Run

### Console App

```bash
cd RestaurantReservation.Console
dotnet run
```
### API App

```bash
cd RestaurantReservation.Console
dotnet run
```
### Visit Swagger UI:
```bash
https://localhost:<port>/swagger
```