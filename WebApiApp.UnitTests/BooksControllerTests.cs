using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiApp.Controllers;
using WebApiApp.Models;
using WebApiApp.Services;
using Xunit;

namespace WebApiApp.UnitTests
{
    public class BooksControllerTests
    {
        [Fact]
        public async Task GetAsync_ReturnsActionResultWithListOf2Books()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            bookServiceMock.Setup(service => service.GetAsync()).ReturnsAsync(GetTestBooks());
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<ActionResult<List<Book>>>(result);
            Assert.Equal(2, result.Value.Count);
        }

        [Fact]
        public async Task GetAsync_WithNonexistentItemId_ReturnsNotFoundResult()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            bookServiceMock.Setup(service => service.GetAsync("id")).ReturnsAsync((Book)null);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.GetAsync("id");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAsync_WithExistingItemId_ReturnsActionResultWithBook()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            var book = GetTestBooks()[1];
            bookServiceMock.Setup(service => service.GetAsync("id2")).ReturnsAsync(book);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.GetAsync("id2");

            // Assert
            Assert.IsType<ActionResult<Book>>(result);
            Assert.Equal(book, result.Value);
        }

        [Fact]
        public async Task CreateAsync_WithBook_ReturnsCreatedAtRouteResultWithCreatedBook()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            var book = new Book();
            bookServiceMock.Setup(service => service.CreateAsync(book)).Returns(Task.CompletedTask);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.CreateAsync(book);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(book, ((CreatedAtRouteResult)result.Result).Value);
        }

        [Fact]
        public async Task UpdateAsync_WithNonexistentItemIdAndBook_ReturnsNotFoundResult()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            var id = string.Empty;
            var book = new Book();
            bookServiceMock.Setup(service => service.GetAsync(id)).ReturnsAsync((Book)null);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.UpdateAsync(id, book);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateAsync_WithExistingItemIdAndBook_ReturnsNoContentResult()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            const string id = "id";
            var book = new Book();
            bookServiceMock.Setup(service => service.GetAsync(id)).ReturnsAsync(book);
            bookServiceMock.Setup(service => service.UpdateAsync(id, book)).Returns(Task.CompletedTask);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.UpdateAsync(id, book);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_WithNonexistentItemId_ReturnsNotFoundResult()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            var id = string.Empty;
            bookServiceMock.Setup(service => service.GetAsync(id)).ReturnsAsync((Book)null);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.DeleteAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_WithExistingItemId_ReturnsNoContentResult()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>(MockBehavior.Strict);
            const string id = "id";
            var book = new Book { Id = id };
            bookServiceMock.Setup(service => service.GetAsync(id)).ReturnsAsync(book);
            bookServiceMock.Setup(service => service.RemoveAsync(id)).Returns(Task.CompletedTask);
            var controller = new BooksController(bookServiceMock.Object);

            // Act
            var result = await controller.DeleteAsync(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        private static List<Book> GetTestBooks() =>
            new()
            {
                new Book(id: "Id1", name: "Name1", price: 1, category: "Category1", author: "Author1"),
                new Book(id: "Id2", name: "Name2", price: 2, category: "Category2", author: "Author2")
            };
    }
}