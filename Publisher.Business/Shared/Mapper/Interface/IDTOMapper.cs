using Publisher.Business.Shared.DTO;

namespace Publisher.Business.Shared.Mapper.Interface
{
    public interface IDTOMapper
    {
        BookDTO ToBookDTO(Data.Entities.Book book);
        AuthorDTO ToAuthorDTO(Data.Entities.Author author);

        AuthorDTO ToAuthorOnlyDTO(Data.Entities.Author author);
    }
}
