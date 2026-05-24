using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Publisher.Data.Entities;

namespace Publisher.Data
{
    public class PubContext(IConfiguration config) : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = config.GetConnectionString("PubDB");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                     new Author { AuthorId = 1, FirstName = "Rhoda", LastName = "Lerman" });

            var authorList = new Author[]{
                new Author {AuthorId = 2, FirstName = "Ruth", LastName = "Ozeki" },
                new Author {AuthorId = 3, FirstName = "Sofia", LastName = "Segovia" },
                new Author {AuthorId = 4, FirstName = "Ursula K.", LastName = "LeGuin" },
                new Author {AuthorId = 5, FirstName = "Hugh", LastName = "Howey" },
                new Author {AuthorId = 6, FirstName = "Isabelle", LastName = "Allende" }
            };
            modelBuilder.Entity<Author>().HasData(authorList);

            var someBooks = new Book[]
            {
                new Book { BookId = 1, Title = "The Child Garden", AuthorId = 1, BasePrice=54.99m, PublishDate = new DateOnly(2024, 6, 1) },
                new Book { BookId = 2, Title = "The Book of the Unnamed Midwife", AuthorId = 2, BasePrice = 19.99m, PublishDate = new DateOnly(2020, 12, 25) },
                new Book { BookId = 3, Title = "The Great Alone", AuthorId = 3, BasePrice = 29.99m, PublishDate = new DateOnly(2021, 5, 19) },
                new Book {BookId = 4, Title = "The Left Hand of Darkness", AuthorId = 4, BasePrice = 9.95m, PublishDate = new DateOnly(2020, 11, 16)},
                new Book {BookId = 5, Title = "Wool", AuthorId = 5, BasePrice = 14.99m, PublishDate = new DateOnly(2022, 10, 2)},
                new Book {BookId = 6, Title = "The House of the Spirits", AuthorId = 6, BasePrice = 22.99m, PublishDate = new DateOnly(1995, 1, 2)}
            };

            modelBuilder.Entity<Book>().HasData(someBooks);
        }
    }
}
