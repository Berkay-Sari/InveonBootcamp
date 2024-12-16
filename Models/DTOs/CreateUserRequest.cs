namespace LibraryManagement.Web.Models.DTOs;

public record CreateUserRequest(
    string UserName,
    string Email,
    string Password,
    string FirstName,
    string LastName
);
