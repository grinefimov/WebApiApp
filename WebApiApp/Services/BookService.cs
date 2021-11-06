using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAsync();
        Task<Book> GetAsync(string id);
        Task CreateAsync(Book book);
        Task UpdateAsync(string id, Book bookIn);
        Task RemoveAsync(string id);
    }

    public class BookService : IBookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() => (await _books.FindAsync(book => true)).ToList();

        public async Task<Book> GetAsync(string id) => (await _books.FindAsync(book => book.Id == id)).FirstOrDefault();

        public async Task CreateAsync(Book book) => await _books.InsertOneAsync(book);

        public async Task UpdateAsync(string id, Book bookIn) =>
            await _books.ReplaceOneAsync(book => book.Id == id, bookIn);

        public async Task RemoveAsync(string id) => await _books.DeleteOneAsync(book => book.Id == id);
    }
}