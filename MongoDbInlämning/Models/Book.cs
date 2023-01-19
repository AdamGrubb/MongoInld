using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbInlämning.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.Models
{
    public class Book: IBookshelfItem
    {
        [BsonId]
        public ObjectId databaseId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }
        public Book(string title, string author, int pages)
        {
            Title = title;
            Author = author;
            Pages = pages;
        }

    }
}
