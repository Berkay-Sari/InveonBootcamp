#  Inveon-Homework-2
Simple Library Management System Web Application

## Getting Started

1. Clone the repository:
2. Open the solution in Visual Studio:
3. Restore the dependencies:
4. Build the project:
5. Run the project:

## Project Structure

- `Data`: Contains db context and repos.
- `Views` : Contains the Razor views for the project.
- `Controllers` : Contains the controllers for the project.
- `Models/DTOs` : Request and Response DTOs for data transfer.
- `Models/Entities` : EF entities.
- `Repository : IRepository` : Generic Repository for those other than Identity. 
- `UnitOfWork : IUnitOfWork` : Unit of work for those other than Identity.
- `ServiceResult` : Used as the return type of Services, except for Identity. Used IdentityResult for Identity.
- `seed.sql` : Query file that inserts the initial books, since the create, update and delete operations will be implemented in the next assignment.

## Tech Stack

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet/mvc)
- [Razor Views](https://docs.microsoft.com/aspnet/core/mvc/views/razor)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [SQLServer](https://www.microsoft.com/sql-server/)
- [Mapster](https://github.com/MapsterMapper/Mapster)
- [Bootstrap](https://getbootstrap.com/)

## Note: 
```
The Unit of Work pattern is symbolic here.
This is because we didn't implement books data changer operations such as create, update, delete in this assignment. 
We will implement these operations in the next assignment. 
And thus there is no need to SaveChange.
```
