namespace AutoRegex;

class Program
{
    private static string regexFilePathTest = "C:\\Users\\Patrik-Desktop\\Desktop\\AFJ_02\\regex";
    private static string stringFilePathTest = "C:\\Users\\Patrik-Desktop\\Desktop\\AFJ_02\\retazce";
    
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: AutoRegex <regex_file> <input_strings_file>");
            return;
        }

        // var regexFilePath = args[0];
        // var inputStringsFilePath = args[1];

        for (int i = 1; i < 6; i++)
        {
            var regexFilePath = $"{regexFilePathTest}{i}.txt";
            var inputStringsFilePath = $"{stringFilePathTest}{i}.txt";
            Console.WriteLine($"TEST {i}");
            ValidateInput(regexFilePath, inputStringsFilePath);
        }
        
        
        
    }
    
    public static void ValidateInput(string regexFilePath, string inputStringsFilePath)
    {
        try
        {
            var regexParser = new RegexParser();
            var regexesAndOperations = regexParser.ParseRegexFile(regexFilePath);

            var nfa = new NFA();
            nfa.ConstructNFA(regexesAndOperations);

            var inputStringsParser = new InputStringsParser();
            var inputStrings = inputStringsParser.ParseInputStringsFile(inputStringsFilePath);

            foreach (var inputString in inputStrings)
            {
                var matches = nfa.Matches(inputString);
                Console.WriteLine($"String: {inputString} \t Is valid regex? {matches}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}