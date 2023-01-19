using MongoDB.Bson;
using MongoDbInlämning.Interface;
using MongoDbInlämning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.BookshelfDAO
{
    public interface IComicDAO
    {
        void TestConnection();
        void Create(Comic item);
        void Delete(ObjectId itemId);
        List<Comic> ReadAll();
        List<Comic> Read(string searchField, string searchValue);
        void Update<T>(ObjectId item, string field, T value);
    }
}
