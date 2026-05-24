using Publisher.Data.Entities;

namespace Publisher.Data.Repositories.Interface
{
    public interface IAuthorRepository
    {
        // Create
        Task<Author> CreateAsync(Author author);

        // Read
        Task<Author?> GetByIdAsync(int id, bool includeBooks = false);
        Task<IEnumerable<Author>> GetAllAsync(bool includeBooks = false);

        // Update
        Task<Author?> UpdateAsync(int id, Author author);

        // Delete
        Task<bool> DeleteAsync(int id);

        // Additional utility methods
        //Task<bool> ExistsAsync(int id);
        //Task<int> CountAsync();
    }
}
