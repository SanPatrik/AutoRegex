using System;
using System.Collections.Generic;
using System.IO;

namespace AutoRegex
{
    public class RegexParser
    {
        public List<RegexAndOperation> ParseRegexFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var regexesAndOperations = new List<RegexAndOperation>();

            foreach (var line in lines)
            {
                var regexAndOperation = ParseLine(line);
                regexesAndOperations.Add(regexAndOperation);
            }

            return regexesAndOperations;
        }

        private RegexAndOperation ParseLine(string line)
        {
            var regexAndOperation = new RegexAndOperation();

            if (string.IsNullOrEmpty(line)) // It's an empty line
            {
                regexAndOperation.IsSymbol = true;
                regexAndOperation.Symbol = "";
            }
            else if (line.Length == 1) // It's a symbol
            {
                regexAndOperation.IsSymbol = true;
                regexAndOperation.Symbol = line;
            }
            else // It's an operation
            {
                var parts = line.Split(',');

                regexAndOperation.IsSymbol = false;
                regexAndOperation.Operand1 = int.Parse(parts[1]);

                if (parts[0] == "U")
                {
                    regexAndOperation.IsUnion = true;
                    regexAndOperation.Operand2 = int.Parse(parts[2]);
                }
                else if (parts[0] == "C")
                {
                    regexAndOperation.IsConcatenation = true;
                    regexAndOperation.Operand2 = int.Parse(parts[2]);
                }
                else if (parts[0] == "I")
                {
                    regexAndOperation.IsIteration = true;
                }
            }

            return regexAndOperation;
        }
    }
}