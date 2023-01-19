using MongoDB.Bson;
using MongoDbInlämning.Interface;
using MongoDbInlämning.Models;

namespace MongoDbInlämning.BookshelfDAO
{
    public interface IBookDAO
    {
        void TestConnection();
        void Create(Book item);
        void Delete(ObjectId itemId);
        List<Book> ReadAll();
        List<Book> Read(string searchField, string searchValue);
        void Update<T>(ObjectId item, string field, T value);

    }
}