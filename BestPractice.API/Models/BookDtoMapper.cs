namespace BestPractice.API.Models;

public static class BookDtoMapper
{
    public static BookDto Map(Book book)
    {
        return new BookDto(book.Id, book.Title, book.Author, book.Quantity);
    }
}