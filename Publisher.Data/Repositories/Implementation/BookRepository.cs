using Publisher.Data.Entities;
using Publisher.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Publisher.Data.Repositories.Implementation
{
    public class BookRepository(PubContext context) : IBookRepository
    {
        public async Task<Book> CreateAsync(Book book)
        {
            context.Set<Book>().Add(book);
            await context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> GetByIdAsync(int id, bool includeAuthor = false)
        {
            var query = context.Set<Book>().AsQueryable();

            if (includeAuthor)
            {
                query = query.Include(b => b.Author);
            }

            return await query.FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync(bool includeAuthor = false)
        {
            var query = context.Set<Book>().AsQueryable();

            if (includeAuthor)
            {
                query = query.Include(b => b.Author);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await context.Set<Book>()
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<Book?> UpdateAsync(int id, Book book)
        {
            var existingBook = await GetByIdAsync(id);
            if (existingBook == null)
            {
                return null;
            }

            existingBook.Title = book.Title;
            existingBook.PublishDate = book.PublishDate;
            existingBook.BasePrice = book.BasePrice;
            existingBook.AuthorId = book.AuthorId;

            context.Set<Book>().Update(existingBook);
            await context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }

            context.Set<Book>().Remove(book);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Set<Book>()
                .AnyAsync(b => b.BookId == id);
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<Book>().CountAsync();
        }
    }
}