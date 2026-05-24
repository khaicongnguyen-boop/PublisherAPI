using Microsoft.AspNetCore.Mvc;
using Publisher.Business.Book.Interface;
using Publisher.Business.Shared.DTO;

namespace Publisher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(
        ILogger<BooksController> logger,
        IBookService bookService) : ControllerBase
    {
        /// <summary>
        /// Get a book by ID
        /// </summary>
        /// <param name="id">The book ID</param>
        /// <param name="includeAuthor">Include author information</param>
        /// <returns>Book details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> Get(int id, bool includeAuthor = false)
        {
            try
            {
                var book = await bookService.GetByIdAsync(id, includeAuthor);
                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while fetching the book with ID {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <param name="includeAuthor">Include author information</param>
        /// <returns>List of all books</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> Get(bool includeAuthor = false)
        {
            try
            {
                var books = await bookService.GetAllAsync(includeAuthor);
                return Ok(books);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while fetching all books");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Get books by author ID
        /// </summary>
        /// <param name="authorId">The author ID</param>
        /// <returns>List of books by the specified author</returns>
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetByAuthorId(int authorId)
        {
            try
            {
                var books = await bookService.GetByAuthorIdAsync(authorId);
                return Ok(books);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while fetching books for author ID {AuthorId}", authorId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Create a new book
        /// </summary>
        /// <param name="bookDto">The book data</param>
        /// <returns>Created book details</returns>
        [HttpPost]
        public async Task<ActionResult<BookDTO>> Post([FromBody] BookDTO bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBook = await bookService.CreateAsync(bookDto);
                return CreatedAtAction(nameof(Get), new { id = createdBook.BookId }, createdBook);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while creating a new book");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Update an existing book
        /// </summary>
        /// <param name="id">The book ID</param>
        /// <param name="bookDto">The updated book data</param>
        /// <returns>Updated book details</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> Put(int id, [FromBody] BookDTO bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedBook = await bookService.UpdateAsync(id, bookDto);
                if (updatedBook == null)
                {
                    return NotFound();
                }

                return Ok(updatedBook);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while updating the book with ID {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id">The book ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await bookService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while deleting the book with ID {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Check if a book exists
        /// </summary>
        /// <param name="id">The book ID</param>
        /// <returns>Boolean indicating if book exists</returns>
        [HttpHead("{id}")]
        public async Task<ActionResult> Head(int id)
        {
            try
            {
                var exists = await bookService.ExistsAsync(id);
                if (!exists)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while checking if book with ID {BookId} exists", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Get total count of books
        /// </summary>
        /// <returns>Total number of books</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            try
            {
                var count = await bookService.CountAsync();
                return Ok(count);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while counting books");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}