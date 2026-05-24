using Publisher.Business.Shared.DTO;
using Publisher.Business.Shared.Mapper.Interface;

namespace Publisher.Business.Shared.Mapper.Implementation
{
    public class DTOMapper : IDTOMapper
    {
        public BookDTO ToBookDTO(Data.Entities.Book book)
        {
            return new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                PublicationDate = book.PublishDate,
                AuthorId = book.AuthorId,
                Author = book.Author != null ? ToAuthorOnlyDTO(book.Author) : null
            };
        }

        public AuthorDTO ToAuthorDTO(Data.Entities.Author author)
        {
            var name = $"{author.FirstName} {author.LastName}";

            return new AuthorDTO
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Name = $"{author.FirstName} {author.LastName}",
                Books = author.Books?.Select(ToBookDTO).ToList() ?? new List<BookDTO>()
            };
        }

        public AuthorDTO ToAuthorOnlyDTO(Data.Entities.Author author)
        {
            return new AuthorDTO
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Name = $"{author.FirstName} {author.LastName}"
            };
        }
    }
}
