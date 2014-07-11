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
    /// The controller implements the REST API that clients use to perform CRUD operations on the list of Authors.
    /// </summary>
    public class AuthorsController : ApiController
    {
        private BookServiceContext dbContext = new BookServiceContext();

        // GET: api/Authors
        /// <summary>
        /// Get all authors.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Author> GetAuthors()
        {
            return dbContext.Authors;
        }

        // GET: api/Authors/5
        /// <summary>
        /// Get an author by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Author))]
        public async Task<IHttpActionResult> GetAuthorAsync(int id)
        {
            Author author = await dbContext.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/Authors/5
        /// <summary>
        /// Update an existing author.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAuthorAsync(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.Id)
            {
                return BadRequest();
            }

            dbContext.Entry(author).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        /// <summary>
        /// Create a new author.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [ResponseType(typeof(Author))]
        public async Task<IHttpActionResult> PostAuthorAsync(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.Authors.Add(author);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        /// <summary>
        /// Delete an author.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Author))]
        public async Task<IHttpActionResult> DeleteAuthorAsync(int id)
        {
            Author author = await dbContext.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            dbContext.Authors.Remove(author);
            await dbContext.SaveChangesAsync();

            return Ok(author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return dbContext.Authors.Count(e => e.Id == id) > 0;
        }
    }
}