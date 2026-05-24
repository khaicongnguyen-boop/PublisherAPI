using Microsoft.AspNetCore.Mvc;
using Publisher.Business.Author.Interface;
using Publisher.Business.Shared.DTO;

namespace Publisher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController(
        ILogger<AuthorsController> logger,
        IAuthorService authorService) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> Get(int id, bool includeBooks = false)
        {
            try
            {
                var author = await authorService.GetByIdAsync(id, includeBooks);
                if (author == null)
                {
                    return NotFound();
                }

                return Ok(author);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while fetching the author with ID {AuthorId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> Get(bool includeBooks = false)
        {
            try
            {
                var authors = await authorService.GetAllAsync(includeBooks);
                return Ok(authors);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while fetching all authors");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> Post([FromBody] AuthorDTO authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdAuthor = await authorService.CreateAsync(authorDto);
                return CreatedAtAction(nameof(Get), new { id = createdAuthor.AuthorId }, createdAuthor);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while creating a new author");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDTO>> Put(int id, [FromBody] AuthorDTO authorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedAuthor = await authorService.UpdateAsync(id, authorDto);
                if (updatedAuthor == null)
                {
                    return NotFound();
                }

                return Ok(updatedAuthor);
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while updating the author with ID {AuthorId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await authorService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                logger.LogError("An error occurred while deleting the author with ID {AuthorId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
