using MongoDbInlämning.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning
{
    public class KonsolTextUI : IUI
    {
        public void Clear()
        {
            Console.Clear();
        }

        public string GetStringInput()
        {
            return Console.ReadLine();
        }

        public void PrintStringSameLine(string outPut)
        {
            Console.Write(outPut);
        }

        public void PrintString(string outPut)
        {
            Console.WriteLine(outPut);
        }
    }
}
