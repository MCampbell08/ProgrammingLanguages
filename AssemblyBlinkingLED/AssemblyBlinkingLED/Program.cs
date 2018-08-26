using System;
using System.IO;
using System.Linq;

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
            Tokenizer tokenizer = new Tokenizer();
            string completeBinaryString = "";

            foreach (string s in stream)
            {
                completeBinaryString += tokenizer.Parse(s);
            }
            
            var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                .Select(x => x * 8)
                .TakeWhile(x => x < completeBinaryString.ToString().Length)
                .Select(x => completeBinaryString.ToString().Substring(x, 8))
                .Select(x => Convert.ToByte(x, 2))
                .ToArray();

            File.WriteAllBytes(outputPath, byteArray);
        }
    }
}
