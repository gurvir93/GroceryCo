using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GroceryCo.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                string filePath = args[0];
                if (File.Exists(filePath) && Path.GetExtension(filePath).Equals("txt", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        string[] items = File.ReadAllLines(filePath);
                        List<string> itemList = new List<string>(items);
                    }
                    catch
                    {
                        Console.WriteLine("File type is invalid.");
                    }
                }
            }
            else
            {
                Console.WriteLine("GroceryCo checkout requires an argument containing the file path to a list of groceries.  The file must have a 'txt' extension.");
            }
        }
    }
}
