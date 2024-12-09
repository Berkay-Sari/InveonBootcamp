namespace BestPractice.API.Models
{
    //since its a basic crud api no need to Rich Domain Model
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Quantity { get; set; }
    }
}
