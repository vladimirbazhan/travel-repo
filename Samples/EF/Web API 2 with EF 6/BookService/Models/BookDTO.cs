
namespace BookService.Models
{
    /// <summary>
    /// Data Transfer Object for class Book
    /// </summary>
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
    }
}
