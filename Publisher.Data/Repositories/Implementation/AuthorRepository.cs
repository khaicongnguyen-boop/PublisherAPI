using Microsoft.EntityFrameworkCore;
using Publisher.Data.Entities;
using Publisher.Data.Repositories.Interface;

namespace Publisher.Data.Repositories.Implementation
{
    public class AuthorRepository(PubContext context) : IAuthorRepository
    {
        public async Task<Author> CreateAsync(Author author)
        {
            context.Set<Author>().Add(author);
            await context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> GetByIdAsync(int id, bool includeBooks = false)
        {
            if (includeBooks)
            {
                var author = await context.Authors.Include(b => b.Books)
                                .FirstOrDefaultAsync(a => a.AuthorId == id);
                return author;
            }
            else
            {
                var author = await context.Authors
                                .FirstOrDefaultAsync(a => a.AuthorId == id);
                return author;
            }
        }

        public async Task<IEnumerable<Author>> GetAllAsync(bool includeBooks = false)
        {
            if (includeBooks)
            {
                var authors = await context.Authors.Include(b => b.Books).ToListAsync();
                return authors;
            }
            else
            {
                var authors = await context.Authors.ToListAsync();
                return authors;
            }
        }

        public async Task<Author?> UpdateAsync(int id, Author author)
        {
            var existingAuthor = await GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return null;
            }

            existingAuthor.FirstName = author.FirstName;
            existingAuthor.LastName = author.LastName;

            context.Set<Author>().Update(existingAuthor);
            await context.SaveChangesAsync();
            return existingAuthor;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await GetByIdAsync(id);
            if (author == null)
            {
                return false;
            }

            context.Set<Author>().Remove(author);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
