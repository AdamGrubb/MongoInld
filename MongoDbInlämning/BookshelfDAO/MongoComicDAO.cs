using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbInlämning.Interface;
using MongoDbInlämning.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MongoDbInlämning.BookshelfDAO
{
    public class MongoComicDAO : IComicDAO
    {
        private MongoClient mongoDbClient;
        private IMongoDatabase selectedDb;
        private IMongoCollection<Comic> comicCollection;
        public MongoComicDAO(string connectionString, string database) //Borde man göra en try catch här kanske? i konstruktorn.Så slipper du try catcha i Programmet.
        {
            mongoDbClient = new MongoClient(connectionString);
            selectedDb = mongoDbClient.GetDatabase(database);
            comicCollection = selectedDb.GetCollection<Comic>("Comics");
        }

        public void TestConnection()
        {
            mongoDbClient.ListDatabases();
        }

        public void Create(Comic item)
        {
                comicCollection.InsertOne(item);
        }

        public void Delete(ObjectId itemId)
        {
            var deleteFilter = Builders<Comic>.Filter.Eq("_id", itemId);
            comicCollection.DeleteOne(deleteFilter);
        }

        public List<Comic> Read(string searchField, string searchValue)
        {
            List<Comic> SearchCollection = new List<Comic>();
            searchValue = Regex.Escape(searchValue);
            try
            {
                var expr = new BsonRegularExpression(new Regex(searchValue, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(4)));
                var filterComic = Builders<Comic>.Filter.Regex(searchField, expr);
                comicCollection.Find(filterComic).ToList().ForEach(comic => SearchCollection.Add(comic));
                return SearchCollection;
            }
            catch (RegexParseException)
            {
                ErrorLogger.LogInvalidInput("MongoComicDAO_ParseError.txt", searchValue);
                throw;
            }
            catch (OverflowException) 
            {
                ErrorLogger.LogInvalidInput("MongoComicDAO_OverFlowError.txt", searchValue);
                throw;
            }
        }

        public List<Comic> ReadAll()
        {
                return comicCollection.Find(new BsonDocument()).ToList();
        }

        public void Update<T>(ObjectId item, string field, T value)
        {
                var filter = Builders<Comic>.Filter.Eq("_id", item);
                var update = Builders<Comic>.Update.Set(field, value);
                comicCollection.UpdateOne(filter, update);
        }
    }
}
