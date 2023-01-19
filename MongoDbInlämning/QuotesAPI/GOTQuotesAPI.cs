using MongoDB.Bson.IO;
using MongoDbInlämning.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.QuotesAPI
{
    public class GOTQuotesAPI : IQuotesApiClient
    {
        public BookQuote? Read()
        {
            string url = $"https://api.gameofthronesquotes.xyz/v1/random";
            using (HttpClient client = new HttpClient())
            {
                    var json = client.GetStringAsync(url).Result;
                    var searchResults = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
                    if (searchResults!=null) return new BookQuote(searchResults.sentence.ToString(), searchResults.character.name.ToString());
                    return null;
            }
        }
    }
}
