namespace AutoRegex;

public static class Nfa
{
    public static bool Validate(Automat nfa, string inputString)
    {
        var currentStates = EpsilonReachable(new HashSet<Node> { nfa.InitialNode });

        if (string.IsNullOrEmpty(inputString) && currentStates.Any(state => nfa.AcceptNodes.Contains(state)))
        {
            return true;
        }

        foreach (var symbol in inputString)
        {
            var nextStates = new HashSet<Node>();
            foreach (var state in currentStates)
            {
                nextStates.UnionWith(Move(state.Edges, symbol.ToString()));
            }
            currentStates = EpsilonReachable(nextStates);
        }

        return currentStates.Any(state => nfa.AcceptNodes.Contains(state));
    }

    private static HashSet<Node> EpsilonReachable(HashSet<Node> states)
    {
        var reach = new HashSet<Node>(states);
        var stack = new Stack<Node>(states);

        while (stack.Count > 0)
        {
            var state = stack.Pop();
            foreach (var edge in state.Edges)
            {
                if (edge.Item2 == "" && !reach.Contains(edge.Item1))
                {
                    reach.Add(edge.Item1);
                    stack.Push(edge.Item1);
                }
            }
        }

        return reach;
    }

    private static HashSet<Node> Move(List<Tuple<Node, string>> edges, string label)
    {
        return new HashSet<Node>(edges.Where(edge => edge.Item2 == label).Select(edge => edge.Item1));
    }
    
    public static Automat Construct(List<RegexAndOperation> regexesAndOperations)
    {
        var automats = new List<Automat>();
    
        foreach (var regexAndOperation in regexesAndOperations)
        {
            Automat? automat = new Automat();
        
            if (regexAndOperation.Operation == OperationType.Symbol)
            {
                if (regexAndOperation.Symbol == "")
                {
                    var initialAndAccpet = new Node();
                    automat.InitialNode = initialAndAccpet;
                    automat.AcceptNodes.Add(initialAndAccpet);  
                }
                else
                {
                    var acceptNode = new Node();
                    var initialNode = new Node();
                    initialNode.AddEdge(acceptNode, regexAndOperation.Symbol!);
                    automat.InitialNode = initialNode;
                    automat.AcceptNodes.Add(acceptNode);  
                }
            }
            else
            {
                Automat automat1 = automats[regexAndOperation.Operand1!.Value - 1];
                Automat? automat2 = null;
                if (regexAndOperation.Operand2.HasValue)
                {
                    automat2 = automats[regexAndOperation.Operand2.Value - 1];
                }
                if (regexAndOperation.Operation == OperationType.Union)
                {
                    var initialNode = new Node();
                    initialNode.AddEdge(automat1.InitialNode, "");
                    initialNode.AddEdge(automat2.InitialNode, "");
                    
                    automat.InitialNode = initialNode;
                    automat.AcceptNodes.AddRange(automat1.AcceptNodes);
                    automat.AcceptNodes.AddRange(automat2.AcceptNodes);
                    
                }
                else if (regexAndOperation.Operation == OperationType.Concatenation)
                {
                    automat.InitialNode = automat1.InitialNode;
                    foreach (var acceptNode in automat1.AcceptNodes)
                    {
                        acceptNode.AddEdge(automat2!.InitialNode, "");
                    }
                    automat.AcceptNodes = automat2!.AcceptNodes;
                }
                else if (regexAndOperation.Operation == OperationType.Iteration)
                {
                    var initialNode = new Node();
                    
                    foreach (var acceptNode in automat1.AcceptNodes)
                    {
                        acceptNode.AddEdge(automat1.InitialNode, "");
                    }
                    
                    initialNode.AddEdge(automat1.InitialNode, "");
                    automat.AcceptNodes.Add(initialNode);  
                    automat.AcceptNodes.AddRange(automat1.AcceptNodes);
                    automat.InitialNode = initialNode;
                }
            }
            automats.Add(automat);
        }
        return automats.Last();
    }
}


