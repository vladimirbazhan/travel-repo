using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Id property will become the primary key column of the database table.
        /// </summary>
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
