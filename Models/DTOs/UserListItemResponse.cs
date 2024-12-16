namespace LibraryManagement.Web.Models.DTOs;

public record UserListItemResponse(
    Guid Id,
    string UserName,
    string Email,
    string FullName
);
