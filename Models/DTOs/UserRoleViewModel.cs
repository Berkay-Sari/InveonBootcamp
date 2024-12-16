namespace LibraryManagement.Web.Models.DTOs;

public record UserRoleViewModel(
    Guid RoleId,
    string RoleName,
    bool IsAssigned
);
