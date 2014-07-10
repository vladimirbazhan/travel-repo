using System.Data.Entity;

namespace BookService.Models
{
    /// <summary>
    /// Manages entity objects during run time, which includes populating objects with data from a database, change tracking, and persisting data to the database.
    /// </summary>
    public class BookServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation: http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BookServiceContext() : base("name=BookServiceContext")
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    
    }
}
