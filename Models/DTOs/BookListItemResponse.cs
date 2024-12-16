namespace LibraryManagement.Web.Models.DTOs;

public record BookListItemResponse(
    int Id,
    string Title,
    string Author,
    int PublicationYear,
    string ISBN
);