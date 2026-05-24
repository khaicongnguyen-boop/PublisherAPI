namespace Publisher.Business.Shared.DTO
{
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name { get; set; }

        public List<BookDTO> Books { get; set; }
    }
}
