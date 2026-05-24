namespace Publisher.Business.Shared.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateOnly PublicationDate { get; set; }
        public int AuthorId { get; set; }

        public AuthorDTO Author { get; set; }
    }
}
