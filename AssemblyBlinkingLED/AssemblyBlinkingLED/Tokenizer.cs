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
                ConvertToBinary(commandCode);
            }
        }

        private bool IsValidInstruction(string instruction)
        {
            bool result = false;

            if (instruction == "MOVW" || instruction == "MOVT" || instruction == "ADD" || instruction == "OR" || instruction == "SUB"
                 || instruction == "CMP" || instruction == "LDR" || instruction == "STR" || instruction == "B" || instruction == "BNE")
            {
                result = true;
            }

            return result;
        }

        private void ConvertToBinary(string[] commandCode)
        {
            string[] commandCodeBinary = new string[32];
            switch (commandCode[0])
            {
                case "MOVW":
                    commandCodeBinary = ConvertMOVTypes(commandCode[0], commandCode[1], commandCode[2]);
                    break;
                case "MOVT":
                    commandCodeBinary = ConvertMOVTypes(commandCode[0], commandCode[1], commandCode[2]);
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
            Console.WriteLine(commandCodeBinary);
        }

        private string[] ConvertMOVTypes(string instruction, string destinationReg, string immediateValue)
        {
            string[] returnedBinaryString = AddNoFlagConditional(new string[32]);

            returnedBinaryString[4] = "0";
            returnedBinaryString[5] = "0";
            returnedBinaryString[6] = "1";
            returnedBinaryString[7] = "1";

            if (instruction == "MOVW")
            {
                returnedBinaryString[8] = "0";
                returnedBinaryString[9] = "0";
                returnedBinaryString[10] = "0";
                returnedBinaryString[11] = "0";
            }
            else if (instruction == "MOVT")
            {
                returnedBinaryString[8] = "0";
                returnedBinaryString[9] = "1";
                returnedBinaryString[10] = "0";
                returnedBinaryString[11] = "0";
            }

            string destinationRegBin = Convert.ToString(Convert.ToInt32(destinationReg[1].ToString(), 16), 2);
            
            returnedBinaryString[16] = destinationRegBin[0].ToString();
            returnedBinaryString[17] = destinationRegBin[1].ToString();
            returnedBinaryString[18] = destinationRegBin[2].ToString();
            returnedBinaryString[19] = destinationRegBin[3].ToString();

            returnedBinaryString = AddImmediateValue(returnedBinaryString, immediateValue);

            return returnedBinaryString;
            
        }
        private string[] AddNoFlagConditional(string[] binaryString)
        {
            binaryString[0] = "1";
            binaryString[1] = "1";
            binaryString[2] = "1";
            binaryString[3] = "0";
            return binaryString;
        }

        private string[] AddImmediateValue(string[] binaryString, string immediateValue)
        {
            string binary = Convert.ToString(Convert.ToInt32(immediateValue, 16), 2);

            binaryString[12] = binary[0].ToString();
            binaryString[13] = binary[1].ToString();
            binaryString[14] = binary[2].ToString();
            binaryString[15] = binary[3].ToString();


            binaryString[20] = binary[4].ToString();
            binaryString[21] = binary[5].ToString();
            binaryString[22] = binary[6].ToString();
            binaryString[23] = binary[7].ToString();
            binaryString[24] = binary[8].ToString();
            binaryString[25] = binary[9].ToString();
            binaryString[26] = binary[10].ToString();
            binaryString[27] = binary[11].ToString();
            binaryString[28] = binary[12].ToString();
            binaryString[29] = binary[13].ToString();
            binaryString[30] = binary[14].ToString();
            binaryString[31] = binary[15].ToString();

            return binaryString;
        }
    }
}
