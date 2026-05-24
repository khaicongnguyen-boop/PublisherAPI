using Microsoft.Extensions.Logging;
using Moq;
using Publisher.Business.Author.Implementation;
using Publisher.Business.Shared.DTO;
using Publisher.Business.Shared.Mapper.Interface;
using Publisher.Data.Repositories.Interface;

namespace Publisher.Business.Test
{
    [TestClass]
    public class AuthorServiceTest
    {
        private Mock<ILogger<AuthorService>> _mockLogger;
        private Mock<IAuthorRepository> _mockRepository;
        private Mock<IDTOMapper> _mockMapper;
        private AuthorService _authorService;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<AuthorService>>();
            _mockRepository = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IDTOMapper>();
            _authorService = new AuthorService(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        #region GetByIdAsync Tests

        [TestMethod]
        public async Task GetByIdAsync_WithValidId_ReturnsAuthorDTO()
        {
            // Arrange
            int authorId = 1;
            var author = new Data.Entities.Author
            {
                AuthorId = authorId,
                FirstName = "John",
                LastName = "Doe",
                Books = new List<Data.Entities.Book>()
            };

            var authorDto = new AuthorDTO
            {
                AuthorId = authorId,
                FirstName = "John",
                LastName = "Doe",
                Books = new List<BookDTO>()
            };

            _mockRepository.Setup(r => r.GetByIdAsync(authorId, false))
                .ReturnsAsync(author);

            _mockMapper.Setup(m => m.ToAuthorDTO(author))
                .Returns(authorDto);

            // Act
            var result = await _authorService.GetByIdAsync(authorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(authorId, result.AuthorId);
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
            _mockRepository.Verify(r => r.GetByIdAsync(authorId, false), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int authorId = 999;
            _mockRepository.Setup(r => r.GetByIdAsync(authorId, false))
                .ReturnsAsync((Data.Entities.Author)null);

            // Act
            var result = await _authorService.GetByIdAsync(authorId);

            // Assert
            Assert.IsNull(result);
            _mockRepository.Verify(r => r.GetByIdAsync(authorId, false), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetByIdAsync_WhenRepositoryThrows_ThrowsException()
        {
            // Arrange
            int authorId = 1;
            _mockRepository.Setup(r => r.GetByIdAsync(authorId, false))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            await _authorService.GetByIdAsync(authorId);

            // Assert is handled by ExpectedException
        }

        #endregion

        #region GetAllAsync Tests

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Data.Entities.Author>
            {
                new Data.Entities.Author { AuthorId = 1, FirstName = "John", LastName = "Doe", Books = new List<Data.Entities.Book>() },
                new Data.Entities.Author { AuthorId = 2, FirstName = "Jane", LastName = "Smith", Books = new List<Data.Entities.Book>() }
            };

            _mockRepository.Setup(r => r.GetAllAsync(false))
                .ReturnsAsync(authors);

            // Act
            var result = await _authorService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _mockRepository.Verify(r => r.GetAllAsync(false), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoAuthors_ReturnsEmptyList()
        {
            // Arrange
            var authors = new List<Data.Entities.Author>();
            _mockRepository.Setup(r => r.GetAllAsync(false))
                .ReturnsAsync(authors);

            // Act
            var result = await _authorService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockRepository.Verify(r => r.GetAllAsync(false), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        [TestMethod]
        public async Task CreateAsync_WithValidDTO_ReturnsCreatedAuthorDTO()
        {
            // Arrange
            var authorDto = new AuthorDTO
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var createdAuthor = new Data.Entities.Author
            {
                AuthorId = 1,
                FirstName = "John",
                LastName = "Doe",
                Books = new List<Data.Entities.Book>()
            };

            var createdAuthorDto = new AuthorDTO
            {
                AuthorId = 1,
                FirstName = "John",
                LastName = "Doe",
                Books = new List<BookDTO>()
            };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Data.Entities.Author>()))
                .ReturnsAsync(createdAuthor);

            _mockMapper.Setup(m => m.ToAuthorDTO(createdAuthor))
                .Returns(createdAuthorDto);

            // Act
            var result = await _authorService.CreateAsync(authorDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.AuthorId);
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Data.Entities.Author>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateAsync_WhenRepositoryThrows_ThrowsException()
        {
            // Arrange
            var authorDto = new AuthorDTO
            {
                FirstName = "John",
                LastName = "Doe"
            };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Data.Entities.Author>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            await _authorService.CreateAsync(authorDto);

            // Assert is handled by ExpectedException
        }

        #endregion

        #region UpdateAsync Tests

        [TestMethod]
        public async Task UpdateAsync_WithValidId_ReturnsUpdatedAuthorDTO()
        {
            // Arrange
            int authorId = 1;
            var authorDto = new AuthorDTO
            {
                FirstName = "Jane",
                LastName = "Smith"
            };

            var updatedAuthor = new Data.Entities.Author
            {
                AuthorId = authorId,
                FirstName = "Jane",
                LastName = "Smith",
                Books = new List<Data.Entities.Book>()
            };

            var updatedAuthorDto = new AuthorDTO
            {
                AuthorId = authorId,
                FirstName = "Jane",
                LastName = "Smith",
                Books = new List<BookDTO>()
            };

            _mockRepository.Setup(r => r.UpdateAsync(authorId, It.IsAny<Data.Entities.Author>()))
                .ReturnsAsync(updatedAuthor);

            _mockMapper.Setup(m => m.ToAuthorDTO(updatedAuthor))
                .Returns(updatedAuthorDto);

            // Act
            var result = await _authorService.UpdateAsync(authorId, authorDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(authorId, result.AuthorId);
            Assert.AreEqual("Jane", result.FirstName);
            Assert.AreEqual("Smith", result.LastName);
            _mockRepository.Verify(r => r.UpdateAsync(authorId, It.IsAny<Data.Entities.Author>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int authorId = 999;
            var authorDto = new AuthorDTO
            {
                FirstName = "Jane",
                LastName = "Smith"
            };

            _mockRepository.Setup(r => r.UpdateAsync(authorId, It.IsAny<Data.Entities.Author>()))
                .ReturnsAsync((Data.Entities.Author)null);

            // Act
            var result = await _authorService.UpdateAsync(authorId, authorDto);

            // Assert
            Assert.IsNull(result);
            _mockRepository.Verify(r => r.UpdateAsync(authorId, It.IsAny<Data.Entities.Author>()), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        [TestMethod]
        public async Task DeleteAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            int authorId = 1;
            _mockRepository.Setup(r => r.DeleteAsync(authorId))
                .ReturnsAsync(true);

            // Act
            var result = await _authorService.DeleteAsync(authorId);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.DeleteAsync(authorId), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            int authorId = 999;
            _mockRepository.Setup(r => r.DeleteAsync(authorId))
                .ReturnsAsync(false);

            // Act
            var result = await _authorService.DeleteAsync(authorId);

            // Assert
            Assert.IsFalse(result);
            _mockRepository.Verify(r => r.DeleteAsync(authorId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteAsync_WhenRepositoryThrows_ThrowsException()
        {
            // Arrange
            int authorId = 1;
            _mockRepository.Setup(r => r.DeleteAsync(authorId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            await _authorService.DeleteAsync(authorId);

            // Assert is handled by ExpectedException
        }

        #endregion
    }
}
