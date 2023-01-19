using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.Interface
{
    public interface IBookshelfController
    {   
        void AddItem();
        void SearchItem();
        void UpdateItem();
        void DeleteItem();

    }
}
