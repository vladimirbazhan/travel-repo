﻿
namespace BookService.Models
{
    /// <summary>
    /// Detail Data Transfer Object for class Book
    /// </summary>
    public class BookDetailDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; }
        public string Genre { get; set; }
    }
}
