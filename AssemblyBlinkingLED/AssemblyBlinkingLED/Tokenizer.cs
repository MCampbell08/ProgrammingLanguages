using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AssemblyBlinkingLED
{
    public class Tokenizer
    {
        public void Parse(string input, StreamWriter writer)
        {
            string[] commandCode = input.Split(' ');

            if (IsValidInstruction(commandCode[0])){
                
            }
        }

        private bool IsValidInstruction(string instruction)
        {
            bool result = false;

            if (instruction.Equals("MOVW") || instruction.Equals("MOVT") || instruction.Equals("ADD") || instruction.Equals("OR") || instruction.Equals("SUB")
                 || instruction.Equals("CMP") || instruction.Equals("LDR") || instruction.Equals("STR") || instruction.Equals("B") || instruction.Equals("BNE"))
            {
                result = true;
            }

            return result;
        }

        private void ConvertToBinary(string instruction)
        {
            switch (instruction)
            {
                case "MOVW":
                    
                    break;
                case "MOVT":
                    break;
                case "ADD":
                    break;
                case "OR":
                    break;
                case "SUB":
                    break;
                case "CMP":
                    break;
                case "LDR":
                    break;
                case "STR":
                    break;
                case "B":
                    break;
                case "BNE":
                    break;
            }
        }

        private 
    }
}
