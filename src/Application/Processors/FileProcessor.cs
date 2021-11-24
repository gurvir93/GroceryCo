using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Application.Processors
{
    public abstract class FileProcessor
    {
        public string FilePath { get; set; }
        public abstract bool ParseLines();
        public string[] ReadFile()
        {
            string[] lines = null;
            if (File.Exists(FilePath))
            {
                lines = File.ReadAllLines(FilePath);
            }
            else
            {
                Console.WriteLine($"Could not find file with path [{FilePath}].");
            }

            return lines;
        }
    }
}
