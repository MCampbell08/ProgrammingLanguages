using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssemblyBlinkingLED
{
    public class Tokenizer
    {
        public string Parse(string input)
        {
            string[] commandCode = input.Split(' ');

            if (IsValidInstruction(commandCode[0])){
                return ConvertToBinary(commandCode);
            }
            return null;
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

        private string ConvertToBinary(string[] commandCode)
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
                    commandCodeBinary = ConvertDataProcessing(commandCode[0], commandCode[1], commandCode[2], commandCode[3]);
                    break;
                case "OR":
                    commandCodeBinary = ConvertDataProcessing(commandCode[0], commandCode[1], commandCode[2], commandCode[3]);
                    break;
                case "SUB":
                    commandCodeBinary = ConvertDataProcessing(commandCode[0], commandCode[1], commandCode[2], commandCode[3]);
                    break;
                case "CMP":
                    commandCodeBinary = ConvertDataProcessing(commandCode[0], commandCode[1], commandCode[1], commandCode[2]);
                    break;
                case "LDR":
                    commandCodeBinary = ConvertSingleDataTransfer(commandCode[0], commandCode[1], commandCode[2]);
                    break;
                case "STR":
                    commandCodeBinary = ConvertSingleDataTransfer(commandCode[0], commandCode[1], commandCode[2]);
                    break;
                case "B":
                    commandCodeBinary = ConvertBranch(commandCode[0], commandCode[1]);
                    break;
                case "BNE":
                    commandCodeBinary = ConvertBranch(commandCode[0], commandCode[1]);
                    break;
            }

            StringBuilder completeBuilder = new StringBuilder();
            StringBuilder setBuilder = new StringBuilder();

            for (int i = 0; i < 32; i++)
            {
                setBuilder.Append(commandCodeBinary[i]);

                if (i == 7 || i == 15 || i == 23 || i == 31)
                {
                    string newForm = setBuilder.ToString() + completeBuilder.ToString();

                    completeBuilder.Clear();
                    setBuilder.Clear();
                    completeBuilder.Append(newForm);
                    newForm = "";
                }
            }

            return completeBuilder.ToString();
        }

        private string[] ConvertBranch(string instruction, string immediateVal)
        {
            string[] returnedBinaryString = new string[32];

            if (instruction == "B")
                returnedBinaryString = AddNoFlagConditional(returnedBinaryString);
            else
                returnedBinaryString = AddFlagConditional(returnedBinaryString);

            returnedBinaryString[4] = "1";
            returnedBinaryString[5] = "0";
            returnedBinaryString[6] = "1";
            returnedBinaryString[7] = "0";

            string immediateValBin = Convert.ToString(Convert.ToInt32(immediateVal.Split("x")[1].ToString(), 16), 2).PadLeft(24, '0');

            for (int i = 8, j = 0; i < 32 && j < 24; i++, j++)
            {
                returnedBinaryString[i] = immediateValBin[j].ToString();
            }

            return returnedBinaryString;
        }

        private string[] ConvertSingleDataTransfer(string instruction, string destinationReg, string baseReg)
        {
            string[] returnedBinaryString = AddNoFlagConditional(new string[32]);

            returnedBinaryString = AlterSingleDataTransferProps(returnedBinaryString, instruction);

            string baseRegBin = Convert.ToString(Convert.ToInt32(baseReg[1].ToString(), 16), 2).PadLeft(4, '0');
            string destinationRegBin = Convert.ToString(Convert.ToInt32(destinationReg[1].ToString(), 16), 2).PadLeft(4, '0');
            
            for (int i = 12, j = 0; i < 16 && j < 4; i++, j++)
            {
                returnedBinaryString[i] = baseRegBin[j].ToString();
            }

            for (int i = 16, j = 0; i < 20 && j < 4; i++, j++)
            {
                returnedBinaryString[i] = destinationRegBin[j].ToString();
            }

            for (int i = 20; i < 32; i++)
            {
                returnedBinaryString[i] = "0";
            }

            return returnedBinaryString;
        }

        private string[] ConvertDataProcessing(string instruction, string destinationReg, string operandOne, string operandTwo)
        {
            string[] returnedBinaryString = AddNoFlagConditional(new string[32]);

            returnedBinaryString = AlterImmediateOperand(returnedBinaryString, operandTwo);
            returnedBinaryString = AlterOperationCode(returnedBinaryString, instruction);

            string operandOneBin = Convert.ToString(Convert.ToInt32(operandOne[1].ToString(), 16), 2).PadLeft(4, '0');
            string destinationRegBin = Convert.ToString(Convert.ToInt32(destinationReg[1].ToString(), 16), 2).PadLeft(4, '0');
            
            for (int i = 12, j = 0; i < 16 && j < 4; i++, j++)
            {
                returnedBinaryString[i] = operandOneBin[j].ToString();
            }

            for (int i = 16, j = 0; i < 20 && j < 4; i++, j++)
            {
                returnedBinaryString[i] = destinationRegBin[j].ToString();
            }

            return returnedBinaryString;
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

            string destinationRegBin = Convert.ToString(Convert.ToInt32(destinationReg[1].ToString(), 16), 2).PadLeft(4, '0');
            
            for (int i = 16, j = 0; i < 20 && j < 4; i++, j++)
            {
                returnedBinaryString[i] = destinationRegBin[j].ToString();
            }

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

        private string[] AddFlagConditional(string[] binaryString)
        {
            binaryString[0] = "0";
            binaryString[1] = "0";
            binaryString[2] = "0";
            binaryString[3] = "1";

            return binaryString;
        }

        private string[] AddImmediateValue(string[] binaryString, string immediateValue)
        {
            string binary = immediateValue.Contains("x") ? Convert.ToString(int.Parse(immediateValue.Split('x')[1].ToString(), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(16, '0') : Convert.ToString(Convert.ToInt32(immediateValue), 2).PadLeft(16, '0');
                        
            for (int i = 12, j = 0; i < 16 && j < 4; i++, j++)
            {
                binaryString[i] = binary[j].ToString();
            }
            
            for (int i = 20, j = 4; i < 32 && j < 16; i++, j++)
            {
                binaryString[i] = binary[j].ToString();
            }

            return binaryString;
        }

        private string[] AlterImmediateOperand(string[] binaryString, string operandTwo)
        {
            string binary = "";

            if (!operandTwo.Contains("r"))
            {
                binary = Convert.ToString(int.Parse(operandTwo.Split('x')[1].ToString(), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(8, '0');

                binaryString[4] = "0";
                binaryString[5] = "0";
                binaryString[6] = "1";

                binaryString[20] = "0";
                binaryString[21] = "0";
                binaryString[22] = "0";
                binaryString[23] = "0";

                for (int i = 24, j = 0; i < 32 && j < 8; i++, j++)
                {
                    binaryString[i] = binary[j].ToString();
                }
            }
            else
            {
                binary = Convert.ToString(int.Parse(operandTwo.Split('r')[1].ToString(), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(4, '0');

                binaryString[4] = "0";
                binaryString[5] = "0";
                binaryString[6] = "0";

                binaryString[20] = "0";
                binaryString[21] = "0";
                binaryString[22] = "0";
                binaryString[23] = "0";
                binaryString[24] = "0";
                binaryString[25] = "0";
                binaryString[26] = "0";
                binaryString[27] = "0";

                for (int i = 28, j = 0; i < 32 && j < 4; i++, j++)
                {
                    binaryString[i] = binary[j].ToString();
                }
            }
            return binaryString;
        }

        private string[] AlterOperationCode(string[] binaryString, string instruction)
        {
            switch (instruction)
            {
                case "ADD":
                    binaryString[7]  = "0";
                    binaryString[8]  = "1";
                    binaryString[9]  = "0";
                    binaryString[10] = "0";

                    binaryString = AlterSetConditionCodes(binaryString, false);
                    break;
                case "SUB":
                    binaryString[7]  = "0";
                    binaryString[8]  = "0";
                    binaryString[9]  = "1";
                    binaryString[10] = "0";

                    binaryString = AlterSetConditionCodes(binaryString, false);
                    break;
                case "OR":
                    binaryString[7]  = "1";
                    binaryString[8]  = "1";
                    binaryString[9]  = "0";
                    binaryString[10] = "0";

                    binaryString = AlterSetConditionCodes(binaryString, false);
                    break;
                case "CMP":
                    binaryString[7]  = "1";
                    binaryString[8]  = "0";
                    binaryString[9]  = "1";
                    binaryString[10] = "0";

                    binaryString = AlterSetConditionCodes(binaryString, true);
                    break;
            }

            return binaryString;
        }

        private string[] AlterSetConditionCodes(string[] binaryString, bool setCode)
        {
            binaryString[11] = (setCode) ? "1" : "0";

            return binaryString;
        }

        private string[] AlterSingleDataTransferProps(string[] binaryString, string instruction)
        {
            binaryString[4] = "0";
            binaryString[5] = "1";
            binaryString[6] = "0";
            binaryString[7] = "0";
            binaryString[8] = "1";
            binaryString[9] = "0";
            binaryString[10] = "0";

            if (instruction == "STR")
                binaryString[11] = "0";
            else
                binaryString[11] = "1";

            return binaryString;
        }
    }
}
