using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiApp.Models
{
    public class Book
    {
        public Book()
        {
        }

        public Book(string id, string name, decimal price, string category, string author)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
            Author = author;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}