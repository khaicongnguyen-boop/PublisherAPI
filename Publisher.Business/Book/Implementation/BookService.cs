using Microsoft.Extensions.Logging;
using Publisher.Business.Book.Interface;
using Publisher.Business.Shared.DTO;
using Publisher.Business.Shared.Mapper.Interface;
using Publisher.Data.Repositories.Interface;

namespace Publisher.Business.Book.Implementation
{
    public class BookService(ILogger<BookService> logger,
        IBookRepository bookRepository,
        IDTOMapper dtoMapper) : IBookService
    {
        public async Task<BookDTO> CreateAsync(BookDTO bookDto)
        {
            try
            {
                logger.LogInformation("Creating new book: {Title}", bookDto.Title);

                var book = new Data.Entities.Book
                {
                    Title = bookDto.Title,
                    PublishDate = bookDto.PublicationDate
                };

                var createdBook = await bookRepository.CreateAsync(book);
                logger.LogInformation("Book created successfully with ID: {BookId}", createdBook.BookId);

                return dtoMapper.ToBookDTO(createdBook);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating book: {Title}", bookDto.Title);
                throw;
            }
        }

        public async Task<BookDTO?> GetByIdAsync(int id, bool includeAuthor = false)
        {
            try
            {
                logger.LogInformation("Fetching book with ID: {BookId}", id);

                var book = await bookRepository.GetByIdAsync(id, includeAuthor);
                if (book == null)
                {
                    logger.LogWarning("Book with ID: {BookId} not found", id);
                    return null;
                }

                return dtoMapper.ToBookDTO(book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching book with ID: {BookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BookDTO>> GetAllAsync(bool includeAuthor = false)
        {
            try
            {
                logger.LogInformation("Fetching all books");

                var books = await bookRepository.GetAllAsync(includeAuthor);
                return books.Select(dtoMapper.ToBookDTO).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching all books");
                throw;
            }
        }

        public async Task<IEnumerable<BookDTO>> GetByAuthorIdAsync(int authorId)
        {
            try
            {
                logger.LogInformation("Fetching books for author ID: {AuthorId}", authorId);

                var books = await bookRepository.GetByAuthorIdAsync(authorId);
                return books.Select(dtoMapper.ToBookDTO).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching books for author ID: {AuthorId}", authorId);
                throw;
            }
        }

        public async Task<BookDTO?> UpdateAsync(int id, BookDTO bookDto)
        {
            try
            {
                logger.LogInformation("Updating book with ID: {BookId}", id);

                var book = new Data.Entities.Book
                {
                    BookId = id,
                    Title = bookDto.Title,
                    PublishDate = bookDto.PublicationDate
                };

                var updatedBook = await bookRepository.UpdateAsync(id, book);
                if (updatedBook == null)
                {
                    logger.LogWarning("Book with ID: {BookId} not found for update", id);
                    return null;
                }

                logger.LogInformation("Book with ID: {BookId} updated successfully", id);
                return dtoMapper.ToBookDTO(updatedBook);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating book with ID: {BookId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                logger.LogInformation("Deleting book with ID: {BookId}", id);

                var result = await bookRepository.DeleteAsync(id);
                if (!result)
                {
                    logger.LogWarning("Book with ID: {BookId} not found for deletion", id);
                    return false;
                }

                logger.LogInformation("Book with ID: {BookId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting book with ID: {BookId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                logger.LogInformation("Checking if book with ID: {BookId} exists", id);
                return await bookRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while checking if book with ID: {BookId} exists", id);
                throw;
            }
        }

        public async Task<int> CountAsync()
        {
            try
            {
                logger.LogInformation("Counting all books");
                return await bookRepository.CountAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while counting books");
                throw;
            }
        }
    }
}