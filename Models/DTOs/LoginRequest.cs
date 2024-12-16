namespace LibraryManagement.Web.Models.DTOs;

public record LoginRequest(
    string UserName,
    string Password,
    bool RememberMe
);
