using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbInlämning.BookshelfDAO;
using MongoDbInlämning.Interface;
using MongoDbInlämning.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MongoDbInlämning
{
    public class MongoBookDAO : IBookDAO
    {
        private MongoClient mongoDbClient;
        private IMongoDatabase selectedDb;
        private IMongoCollection<Book> bookCollection;
        public MongoBookDAO(string connectionString, string database)
        {
            mongoDbClient = new MongoClient(connectionString);
            selectedDb = mongoDbClient.GetDatabase(database);
            bookCollection = selectedDb.GetCollection<Book>("Books");

        }
        public void TestConnection() //Triggar ett exception ifall man inte har rätt credentials.
        {
            mongoDbClient.ListDatabases();
        }

        public void Create(Book item)
        {
            bookCollection.InsertOne(item);
        }

        public void Delete(ObjectId itemId)
        {
            var deleteFilter = Builders<Book>.Filter.Eq("_id", itemId);

            bookCollection.DeleteOne(deleteFilter);
        }

        public List<Book> Read(string searchField, string searchValue)
        {
            /*
             * Använder här ett regex-filter. Gör en try catch här där jag loggar sökningar som triggar excpetions.
             * Kastar sedan vidare exception som tas emot i kontrollern.
             */
            List<Book> SearchCollection = new List<Book>();
            searchValue = Regex.Escape(searchValue); //Gör att man även kan stoppa in special characters
            try
            {
                var expr = new BsonRegularExpression(new Regex(searchValue, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(4)));
                var filterBook = Builders<Book>.Filter.Regex(searchField, expr);
                bookCollection.Find(filterBook).ToList().ForEach(book => SearchCollection.Add(book));
                return SearchCollection;
            }
            catch (RegexParseException)
            {
                ErrorLogger.LogInvalidInput("MongoBookDAO_ParseError.txt", searchValue);
                throw;
            }
            catch (OverflowException) //Ifall sökningen skulle ta för lång tid så triggas denna.
            {
                ErrorLogger.LogInvalidInput("MongoBookDAO_OverFlowError.txt", searchValue);
                throw;
            }
        }

        public List<Book> ReadAll()
        {
            return bookCollection.Find(new BsonDocument()).ToList();
        }

        //Tar emot T value så att man ska kunna stoppa in int osv.
        public void Update<T>(ObjectId item, string field, T value)
        {
            var filter = Builders<Book>.Filter.Eq("_id", item);
            var update = Builders<Book>.Update.Set(field, value);
            bookCollection.UpdateOne(filter, update);
        }
    }
}
