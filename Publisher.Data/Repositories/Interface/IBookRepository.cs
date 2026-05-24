using Publisher.Data.Entities;

namespace Publisher.Data.Repositories.Interface
{
    public interface IBookRepository
    {
        // Create
        Task<Book> CreateAsync(Book book);

        // Read
        Task<Book?> GetByIdAsync(int id, bool includeAuthor = false);
        Task<IEnumerable<Book>> GetAllAsync(bool includeAuthor = false);
        Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId);

        // Update
        Task<Book?> UpdateAsync(int id, Book book);

        // Delete
        Task<bool> DeleteAsync(int id);

        // Additional utility methods
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
    }
}
