using System;
using System.IO;

namespace AssemblyBlinkingLED
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string inputPath = "C:\\Users\\WaffleDefender\\Documents\\ProgrammingLanguages\\inputCode.txt";
            string inputPath = "C:\\Users\\waffl\\Documents\\SchoolCode\\ProgrammingLanguages\\inputCode.txt";
            string outputPath = "C:\\Users\\waffl\\Documents\\SchoolCode\\ProgrammingLanguages\\kernel7.img";
            string[] stream = File.ReadAllLines(inputPath);
            StreamWriter writer = new StreamWriter(outputPath, false);
            Tokenizer tokenizer = new Tokenizer();

            foreach (string s in stream)
            {
                tokenizer.Parse(s, writer);
            }            
        }
    }
}
