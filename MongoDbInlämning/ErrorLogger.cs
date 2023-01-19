using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbInlämning
{
    public static class ErrorLogger
    {
        public static async void LogInvalidInput(string fileName, string value) //Egentligen skulle man som villkor kunna sätta att om det är fler än x antal tecken så skriv in ett error-meddelande som är typ "To many signs"
        {
            if (File.Exists(fileName) && value.Length > 20)
            {
                await File.AppendAllTextAsync(fileName, "\nNew Error:\n" + "\nAnd over 20 characters: " + value.Substring(0, 19) + "...");
            }
            else if (File.Exists(fileName))
            {
                await File.AppendAllTextAsync(fileName, "\nNew Error:\n" + value);
            }
            else
            {
                await File.AppendAllTextAsync(fileName, "Invalid inputs:\n\nNew Error:" + value);
            }
        }
    }
}
