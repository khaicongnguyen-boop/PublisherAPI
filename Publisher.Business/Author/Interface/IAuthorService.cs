using Publisher.Business.Shared.DTO;

namespace Publisher.Business.Author.Interface
{
    public interface IAuthorService
    {
        // Create
        Task<AuthorDTO> CreateAsync(AuthorDTO authorDto);
        // Read
        Task<AuthorDTO?> GetByIdAsync(int id, bool includeBooks = false);
        Task<IEnumerable<AuthorDTO>> GetAllAsync(bool includeBooks = false);
        //// Update
        Task<AuthorDTO?> UpdateAsync(int id, AuthorDTO authorDto);
        //// Delete
        Task<bool> DeleteAsync(int id);
    }
}
