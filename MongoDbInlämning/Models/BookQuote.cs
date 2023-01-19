using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDbInlämning.Interface;

namespace MongoDbInlämning.Models
{
    public class BookQuote : IBookQuote
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public BookQuote(string text, string author)
        {
            Text = text;
            Author = author;
        }
    }
}
