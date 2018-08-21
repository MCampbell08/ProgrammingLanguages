using System;
using System.IO;

namespace AssemblyBlinkingLED
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = "C:\\Users\\WaffleDefender\\Documents\\ProgrammingLanguages\\input.txt";
            string[] stream = File.ReadAllLines(path);
            Tokenizer tokenizer = new Tokenizer();
            StreamWriter writer = new StreamWriter(path, false);

            foreach (string s in stream)
            {
                tokenizer.Parse(s, writer);
            }            
        }
        public static string[] ParseFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
