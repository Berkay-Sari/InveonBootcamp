namespace LibraryManagement.Web.Models.Entities;

public class Book : BaseEntity<int>
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int PublicationYear { get; set; }
    public string ISBN { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string Publisher { get; set; } = default!;
    public int PageCount { get; set; }
    public string Language { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public int AvailableCopies { get; set; } = default!;
}