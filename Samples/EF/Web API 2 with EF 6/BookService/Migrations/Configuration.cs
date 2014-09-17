using System.Data.Entity.Migrations;
using BookService.Models;

namespace BookService.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BookServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// You can use the DbSet<T/>.AddOrUpdate() helper extension method to avoid creating duplicate seed data.
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(BookServiceContext context)
        {
            context.Authors.AddOrUpdate(x => x.Id,
                new Author { Id = 1, Name = "Jane Austen" },
                new Author { Id = 2, Name = "Charles Dickens" },
                new Author { Id = 3, Name = "Miguel de Cervantes" },
                new Author { Id = 4, Name = "Joseph Albahari" });

            context.Books.AddOrUpdate(x => x.Id,
                new Book { Id = 1, Title = "Pride and Prejudice",           Year = 1813, AuthorId = 1, Price = 9.99m,   Genre = "Comedy of manners" },
                new Book { Id = 2, Title = "Northanger Abbey",              Year = 1817, AuthorId = 1, Price = 12.95m,  Genre = "Gothic parody" },
                new Book { Id = 3, Title = "David Copperfield",             Year = 1850, AuthorId = 2, Price = 15m,     Genre = "Bildungsroman" },
                new Book { Id = 4, Title = "Don Quixote",                   Year = 1617, AuthorId = 3, Price = 8.95m,   Genre = "Picaresque" },
                new Book { Id = 5, Title = "LINQ Pocket Reference",         Year = 2008, AuthorId = 4, Price = 18.95m,  Genre = "Programming" },
                new Book { Id = 6, Title = "C# 4.0 in a Nutshell",          Year = 2012, AuthorId = 4, Price = 4.3m,    Genre = "Programming" },
                new Book { Id = 7, Title = "C# 4.5 in a Nutshell (vol 1)",  Year = 2014, AuthorId = 4, Price = 55.77m,  Genre = "Programming" });
        }
    }
}
