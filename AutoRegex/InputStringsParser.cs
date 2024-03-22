namespace AutoRegex;

public class InputStringsParser
{
    public List<string> ParseInputStringsFile(string filePath)
    {
        var allLines = File.ReadAllLines(filePath).ToList();

        allLines.RemoveAt(0);

        return allLines;
    }
}