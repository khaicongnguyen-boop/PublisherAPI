using Microsoft.Extensions.Logging;
using Moq;
using Publisher.Business.Book.Implementation;
using Publisher.Business.Book.Interface;
using Publisher.Business.Shared.DTO;
using Publisher.Business.Shared.Mapper.Interface;
using Publisher.Data.Repositories.Interface;

namespace Publisher.Business.Test
{
    [TestClass]
    public class BookServiceTests
    {
        private Mock<ILogger<BookService>> _mockLogger;
        private Mock<IBookRepository> _mockBookRepository;
        private Mock<IDTOMapper> _mockDTOMapper;
        private IBookService _bookService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockLogger = new Mock<ILogger<BookService>>();
            _mockBookRepository = new Mock<IBookRepository>();
            _mockDTOMapper = new Mock<IDTOMapper>();
            _bookService = new BookService(_mockLogger.Object, _mockBookRepository.Object, _mockDTOMapper.Object);
        }

        #region CreateAsync Tests

        [TestMethod]
        public async Task CreateAsync_WithValidDTO_ReturnsCreatedBookDTO()
        {
            // Arrange
            var bookDto = new BookDTO
            {
                Title = "Test Book",
                PublicationDate = new DateOnly(2024, 1, 15)
            };

            var createdBook = new Data.Entities.Book
            {
                BookId = 1,
                Title = "Test Book",
                PublishDate = new DateOnly(2024, 1, 15),
                AuthorId = 0
            };

            var returnedBookDTO = new BookDTO
            {
                BookId = 1,
                Title = "Test Book",
                PublicationDate = new DateOnly(2024, 1, 15)
            };

            _mockBookRepository.Setup(r => r.CreateAsync(It.IsAny<Data.Entities.Book>()))
                .ReturnsAsync(createdBook);
            _mockDTOMapper.Setup(m => m.ToBookDTO(createdBook))
                .Returns(returnedBookDTO);

            // Act
            var result = await _bookService.CreateAsync(bookDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.BookId);
            Assert.AreEqual("Test Book", result.Title);
            _mockBookRepository.Verify(r => r.CreateAsync(It.IsAny<Data.Entities.Book>()), Times.Once);
            _mockDTOMapper.Verify(m => m.ToBookDTO(It.IsAny<Data.Entities.Book>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task CreateAsync_WithNullDTO_ThrowsException()
        {
            // Arrange & Act & Assert
            await _bookService.CreateAsync(null);
        }

        #endregion

        #region GetByIdAsync Tests

        [TestMethod]
        public async Task GetByIdAsync_WithValidId_ReturnsBookDTO()
        {
            // Arrange
            var bookId = 1;
            var book = new Data.Entities.Book
            {
                BookId = bookId,
                Title = "Test Book",
                PublishDate = new DateOnly(2024, 1, 15),
                AuthorId = 1
            };

            var bookDTO = new BookDTO
            {
                BookId = bookId,
                Title = "Test Book",
                PublicationDate = new DateOnly(2024, 1, 15)
            };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId, false))
                .ReturnsAsync(book);
            _mockDTOMapper.Setup(m => m.ToBookDTO(book))
                .Returns(bookDTO);

            // Act
            var result = await _bookService.GetByIdAsync(bookId, false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookId, result.BookId);
            Assert.AreEqual("Test Book", result.Title);
            _mockBookRepository.Verify(r => r.GetByIdAsync(bookId, false), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithNonexistentId_ReturnsNull()
        {
            // Arrange
            var bookId = 999;
            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId, false))
                .ReturnsAsync((Data.Entities.Book)null);

            // Act
            var result = await _bookService.GetByIdAsync(bookId, false);

            // Assert
            Assert.IsNull(result);
            _mockBookRepository.Verify(r => r.GetByIdAsync(bookId, false), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithIncludeAuthor_CallsRepositoryWithCorrectParameter()
        {
            // Arrange
            var bookId = 1;
            var book = new Data.Entities.Book
            {
                BookId = bookId,
                Title = "Test Book",
                PublishDate = new DateOnly(2024, 1, 15),
                AuthorId = 1,
                Author = new Data.Entities.Author { AuthorId = 1, FirstName = "John", LastName = "Doe" }
            };

            var bookDTO = new BookDTO
            {
                BookId = bookId,
                Title = "Test Book",
                PublicationDate = new DateOnly(2024, 1, 15)
            };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId, true))
                .ReturnsAsync(book);
            _mockDTOMapper.Setup(m => m.ToBookDTO(book))
                .Returns(bookDTO);

            // Act
            var result = await _bookService.GetByIdAsync(bookId, includeAuthor: true);

            // Assert
            Assert.IsNotNull(result);
            _mockBookRepository.Verify(r => r.GetByIdAsync(bookId, true), Times.Once);
        }

        #endregion

        #region GetAllAsync Tests

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Data.Entities.Book>
            {
                new Data.Entities.Book { BookId = 1, Title = "Book 1", PublishDate = new DateOnly(2024, 1, 1), AuthorId = 1 },
                new Data.Entities.Book { BookId = 2, Title = "Book 2", PublishDate = new DateOnly(2024, 2, 1), AuthorId = 2 }
            };

            var bookDTOs = new List<BookDTO>
            {
                new BookDTO { BookId = 1, Title = "Book 1", PublicationDate = new DateOnly(2024, 1, 1) },
                new BookDTO { BookId = 2, Title = "Book 2", PublicationDate = new DateOnly(2024, 2, 1) }
            };

            _mockBookRepository.Setup(r => r.GetAllAsync(false))
                .ReturnsAsync(books);
            _mockDTOMapper.Setup(m => m.ToBookDTO(books[0]))
                .Returns(bookDTOs[0]);
            _mockDTOMapper.Setup(m => m.ToBookDTO(books[1]))
                .Returns(bookDTOs[1]);

            // Act
            var result = await _bookService.GetAllAsync(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _mockBookRepository.Verify(r => r.GetAllAsync(false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAsync_WithEmptyList_ReturnsEmptyCollection()
        {
            // Arrange
            var books = new List<Data.Entities.Book>();
            _mockBookRepository.Setup(r => r.GetAllAsync(false))
                .ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllAsync(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        #endregion

        #region GetByAuthorIdAsync Tests

        [TestMethod]
        public async Task GetByAuthorIdAsync_WithValidAuthorId_ReturnsBooksByAuthor()
        {
            // Arrange
            var authorId = 1;
            var books = new List<Data.Entities.Book>
            {
                new Data.Entities.Book { BookId = 1, Title = "Book 1", PublishDate = new DateOnly(2024, 1, 1), AuthorId = authorId },
                new Data.Entities.Book { BookId = 2, Title = "Book 2", PublishDate = new DateOnly(2024, 2, 1), AuthorId = authorId }
            };

            var bookDTOs = new List<BookDTO>
            {
                new BookDTO { BookId = 1, Title = "Book 1", PublicationDate = new DateOnly(2024, 1, 1) },
                new BookDTO { BookId = 2, Title = "Book 2", PublicationDate = new DateOnly(2024, 2, 1) }
            };

            _mockBookRepository.Setup(r => r.GetByAuthorIdAsync(authorId))
                .ReturnsAsync(books);
            _mockDTOMapper.Setup(m => m.ToBookDTO(books[0]))
                .Returns(bookDTOs[0]);
            _mockDTOMapper.Setup(m => m.ToBookDTO(books[1]))
                .Returns(bookDTOs[1]);

            // Act
            var result = await _bookService.GetByAuthorIdAsync(authorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _mockBookRepository.Verify(r => r.GetByAuthorIdAsync(authorId), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ReturnsUpdatedBookDTO()
        {
            // Arrange
            var bookId = 1;
            var updateBookDTO = new BookDTO
            {
                Title = "Updated Book",
                PublicationDate = new DateOnly(2024, 3, 15)
            };

            var updatedBook = new Data.Entities.Book
            {
                BookId = bookId,
                Title = "Updated Book",
                PublishDate = new DateOnly(2024, 3, 15),
                AuthorId = 1
            };

            var returnedBookDTO = new BookDTO
            {
                BookId = bookId,
                Title = "Updated Book",
                PublicationDate = new DateOnly(2024, 3, 15)
            };

            _mockBookRepository.Setup(r => r.UpdateAsync(bookId, It.IsAny<Data.Entities.Book>()))
                .ReturnsAsync(updatedBook);
            _mockDTOMapper.Setup(m => m.ToBookDTO(updatedBook))
                .Returns(returnedBookDTO);

            // Act
            var result = await _bookService.UpdateAsync(bookId, updateBookDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Book", result.Title);
            _mockBookRepository.Verify(r => r.UpdateAsync(bookId, It.IsAny<Data.Entities.Book>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_WithNonexistentId_ReturnsNull()
        {
            // Arrange
            var bookId = 999;
            var updateBookDTO = new BookDTO
            {
                Title = "Updated Book",
                PublicationDate = new DateOnly(2024, 3, 15)
            };

            _mockBookRepository.Setup(r => r.UpdateAsync(bookId, It.IsAny<Data.Entities.Book>()))
                .ReturnsAsync((Data.Entities.Book)null);

            // Act
            var result = await _bookService.UpdateAsync(bookId, updateBookDTO);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region DeleteAsync Tests

        [TestMethod]
        public async Task DeleteAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var bookId = 1;
            _mockBookRepository.Setup(r => r.DeleteAsync(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _bookService.DeleteAsync(bookId);

            // Assert
            Assert.IsTrue(result);
            _mockBookRepository.Verify(r => r.DeleteAsync(bookId), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_WithNonexistentId_ReturnsFalse()
        {
            // Arrange
            var bookId = 999;
            _mockBookRepository.Setup(r => r.DeleteAsync(bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _bookService.DeleteAsync(bookId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region ExistsAsync Tests

        [TestMethod]
        public async Task ExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            var bookId = 1;
            _mockBookRepository.Setup(r => r.ExistsAsync(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _bookService.ExistsAsync(bookId);

            // Assert
            Assert.IsTrue(result);
            _mockBookRepository.Verify(r => r.ExistsAsync(bookId), Times.Once);
        }

        [TestMethod]
        public async Task ExistsAsync_WithNonexistentId_ReturnsFalse()
        {
            // Arrange
            var bookId = 999;
            _mockBookRepository.Setup(r => r.ExistsAsync(bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _bookService.ExistsAsync(bookId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region CountAsync Tests

        [TestMethod]
        public async Task CountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var expectedCount = 5;
            _mockBookRepository.Setup(r => r.CountAsync())
                .ReturnsAsync(expectedCount);

            // Act
            var result = await _bookService.CountAsync();

            // Assert
            Assert.AreEqual(expectedCount, result);
            _mockBookRepository.Verify(r => r.CountAsync(), Times.Once);
        }

        #endregion
    }
}
