using Publisher.Business.Shared.DTO;

namespace Publisher.Business.Book.Interface
{
    public interface IBookService
    {
        // Create
        Task<BookDTO> CreateAsync(BookDTO bookDto);

        // Read
        Task<BookDTO?> GetByIdAsync(int id, bool includeAuthor = false);
        Task<IEnumerable<BookDTO>> GetAllAsync(bool includeAuthor = false);
        Task<IEnumerable<BookDTO>> GetByAuthorIdAsync(int authorId);

        // Update
        Task<BookDTO?> UpdateAsync(int id, BookDTO bookDto);

        // Delete
        Task<bool> DeleteAsync(int id);

        // Utility methods
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
    }
}
