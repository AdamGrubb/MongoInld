using MongoDbInlämning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.QuotesAPI
{
    public interface IQuotesApiClient
    {
        BookQuote? Read();
    }
}
