using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbInlämning.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.Models
{
    [BsonIgnoreExtraElements]
    public class Comic : IBookshelfItem
    {
        [BsonId]
        public ObjectId databaseId { get; set; }

        public string Title { get; set; }

        public string Vol { get; set; }
        public string Author { get; set; }

        public Comic( string title, string vol, string author)
        {
            
            Title = title;
            Vol = vol;
            Author = author;
        }
    }
}
