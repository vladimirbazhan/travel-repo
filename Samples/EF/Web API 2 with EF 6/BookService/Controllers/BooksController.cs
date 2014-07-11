using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookService.Models;

namespace BookService.Controllers
{
    /// <summary>
    /// Defines a Web API controller.
    /// The controller implements the REST API that clients use to perform CRUD operations on the list of Books.
    /// </summary>
    public class BooksController : ApiController
    {
        private BookServiceContext dbContext = new BookServiceContext();

        // GET: api/Books
        /// <summary>
        /// Get all books.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Book> GetBooks()
        {
            // Eager Loading of related data
            return dbContext.Books.Include(b => b.Author);
        }

        // GET: api/Books/5
        /// <summary>
        /// Get a book by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> GetBookAsync(int id)
        {
            Book book = await dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        /// <summary>
        /// Update an existing book.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBookAsync(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            dbContext.Entry(book).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        /// <summary>
        /// Create a new book.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> PostBookAsync(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        /// <summary>
        /// Delete a book.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> DeleteBookAsync(int id)
        {
            Book book = await dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return dbContext.Books.Count(e => e.Id == id) > 0;
        }
    }
}