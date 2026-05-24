using Microsoft.Extensions.Logging;
using Publisher.Business.Author.Interface;
using Publisher.Business.Shared.DTO;
using Publisher.Business.Shared.Mapper.Interface;
using Publisher.Data.Repositories.Interface;

namespace Publisher.Business.Author.Implementation
{
    public class AuthorService(ILogger<AuthorService> logger,
        IAuthorRepository authorRepository,
        IDTOMapper dtoMapper) : IAuthorService
    {
        public async Task<AuthorDTO?> GetByIdAsync(int id, bool includeBooks = false)
        {
            try
            {
                logger.LogInformation("Fetching author with ID: {AuthorId}", id);

                var author = await authorRepository.GetByIdAsync(id, includeBooks);

                if (author == null)
                {
                    logger.LogWarning("Author with ID: {AuthorId} not found", id);
                    return null;
                }

                return dtoMapper.ToAuthorDTO(author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching author with ID: {AuthorId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAsync(bool includeBooks = false)
        {
            try
            {
                logger.LogInformation("Fetching all authors");

                var authors = await authorRepository.GetAllAsync(includeBooks);
                return authors.Select(dtoMapper.ToAuthorDTO).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching all authors");
                throw;
            }
        }

        public async Task<AuthorDTO> CreateAsync(AuthorDTO authorDto)
        {
            try
            {
                var author = new Data.Entities.Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                };

                var createdAuthor = await authorRepository.CreateAsync(author);
                logger.LogInformation("Author created successfully with ID: {AuthorId}", createdAuthor.AuthorId);

                return dtoMapper.ToAuthorDTO(createdAuthor);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating author: {FirstName} {LastName}", authorDto.FirstName, authorDto.LastName);
                throw;
            }
        }

        public async Task<AuthorDTO?> UpdateAsync(int id, AuthorDTO authorDto)
        {
            try
            {

                var author = new Data.Entities.Author
                {
                    AuthorId = id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                };

                var updatedAuthor = await authorRepository.UpdateAsync(id, author);
                if (updatedAuthor == null)
                {
                    logger.LogWarning("Author with ID: {AuthorId} not found for update", id);
                    return null;
                }

                logger.LogInformation("Author with ID: {AuthorId} updated successfully", id);
                return dtoMapper.ToAuthorDTO(updatedAuthor);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating author with ID: {AuthorId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                logger.LogInformation("Deleting author with ID: {AuthorId}", id);

                var result = await authorRepository.DeleteAsync(id);
                if (!result)
                {
                    logger.LogWarning("Author with ID: {AuthorId} not found for deletion", id);
                    return false;
                }

                logger.LogInformation("Author with ID: {AuthorId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting author with ID: {AuthorId}", id);
                throw;
            }
        }
    }
}
