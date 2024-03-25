namespace AutoRegex;

public static class Parser
{
    public static List<string> ParseInputStringsFile(string filePath)
    {
        var allLines = File.ReadAllLines(filePath).ToList();

        return allLines.GetRange(1, int.Parse(allLines[0]));
    }
    
    public static List<RegexAndOperation> ParseRegexFile(string filePath)
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

    private static RegexAndOperation ParseLine(string line)
    {
        var regexAndOperation = new RegexAndOperation();

        if (string.IsNullOrEmpty(line))
        {
            regexAndOperation.Operation = OperationType.Symbol;
            regexAndOperation.Symbol = "";
        }
        else if (line.Length == 1)
        {
            regexAndOperation.Operation = OperationType.Symbol;
            regexAndOperation.Symbol = line;
        }
        else
        {
            var parts = line.Split(',');

            regexAndOperation.Symbol = null;
            regexAndOperation.Operand1 = int.Parse(parts[1]);

            if (parts[0] == "U")
            {
                regexAndOperation.Operation = OperationType.Union;
                regexAndOperation.Operand2 = int.Parse(parts[2]);
            }
            else if (parts[0] == "C")
            {
                regexAndOperation.Operation = OperationType.Concatenation;
                regexAndOperation.Operand2 = int.Parse(parts[2]);
            }
            else if (parts[0] == "I")
            {
                regexAndOperation.Operation = OperationType.Iteration;
            }
        }

        return regexAndOperation;
    }
}

public class RegexAndOperation
{
    public OperationType Operation { get; set; }
    public string? Symbol { get; set; }
    public int? Operand1 { get; set; }
    public int? Operand2 { get; set; }
}

public enum OperationType
{
    Symbol,
    Union,
    Concatenation,
    Iteration
}