namespace AutoRegex;

class Program
{
    // private static string regexFilePathTest = "C:\\Users\\Patrik-Desktop\\Desktop\\AFJ_02\\regex";
    // private static string stringFilePathTest = "C:\\Users\\Patrik-Desktop\\Desktop\\AFJ_02\\retazce";
    
    static void Main(string[] args)
    {
        // for (int i = 1; i < 6; i++)
        // {
        //     var regexFilePath = $"{regexFilePathTest}{i}.txt";
        //     var inputStringsFilePath = $"{stringFilePathTest}{i}.txt";
        //     Console.WriteLine($"TEST {i}");
        //     ValidateInput(regexFilePath, inputStringsFilePath);
        // }
        
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: AutoRegex <regex_file> <input_strings_file>");
            return;
        }
        
        var regexFilePath = args[0];
        var inputStringsFilePath = args[1];
        ValidateInput(regexFilePath, inputStringsFilePath);
    }
    
    private static void ValidateInput(string regexFilePath, string inputStringsFilePath)
    {
        try
        {
            var regexesAndOperations = Parser.ParseRegexFile(regexFilePath);
            
            var startNode = Nfa.Construct(regexesAndOperations);
            
            var inputStrings = Parser.ParseInputStringsFile(inputStringsFilePath);

            foreach (var inputString in inputStrings)
            {
                var matches = Nfa.Validate(startNode, inputString);
                Console.WriteLine($"String: {inputString} \t Is valid regex? {matches}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
