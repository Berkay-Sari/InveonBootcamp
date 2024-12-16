namespace LibraryManagement.Web.Models.DTOs;

public record UpdateUserRequest(
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber
);