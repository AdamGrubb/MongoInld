using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.Interface
{
    public interface IBookshelfItem
    {
        public ObjectId databaseId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
