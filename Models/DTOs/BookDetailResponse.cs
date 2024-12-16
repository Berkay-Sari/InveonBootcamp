namespace LibraryManagement.Web.Models.DTOs;
public record BookDetailResponse(
    int Id,
    string Title,
    string Author,
    int PublicationYear,
    string ISBN,
    string Genre,
    string Publisher,
    int PageCount,
    string Language,
    string Summary,
    int AvailableCopies
);
