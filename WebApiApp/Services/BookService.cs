using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync()
        {
            return (await _books.FindAsync(book => true)).ToList();
        }

        public async Task<Book> GetAsync(string id)
        {
            return (await _books.FindAsync(book => book.Id == id)).FirstOrDefault();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task UpdateAsync(string id, Book bookIn)
        {
            await _books.ReplaceOneAsync(book => book.Id == id, bookIn);
        }

        public async Task RemoveAsync(Book bookIn)
        {
            await _books.DeleteOneAsync(book => book.Id == bookIn.Id);
        }

        public async Task RemoveAsync(string id)
        {
            await _books.DeleteOneAsync(book => book.Id == id);
        }
    }
}