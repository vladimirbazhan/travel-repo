using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Id property will become the primary key column of the database table.
        /// </summary>
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        public int AuthorId { get; set; }
        
        /// <summary>
        /// Navigation property
        /// </summary>
        public Author Author { get; set; }
    }
}
