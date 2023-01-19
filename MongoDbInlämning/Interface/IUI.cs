using MongoDB.Bson;
using MongoDbInlämning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning.Interface
{
    public interface IUI
    {
        public void PrintStringSameLine(string outPut);
        public void PrintString(string outPut);
        public string GetStringInput();
        public void Clear();
    }
}
