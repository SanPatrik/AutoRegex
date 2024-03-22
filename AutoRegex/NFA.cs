namespace AutoRegex;

public class NFA
{
    public Node StartNode { get; set; }

    public bool Matches(string inputString)
    {
        return Matches(StartNode, inputString);
    }

    private bool Matches(Node node, string remainingInput)
    {
        if (node is SymbolNode symbolNode)
        {
            if (remainingInput.Length == 0)
            {
                return false;
            }
            if (remainingInput[0].ToString() == symbolNode.Symbol)
            {
                string newRemainingInput = remainingInput.Substring(1);
                return Matches(symbolNode, newRemainingInput);
            }
            return false;
        }
        else if (node is UnionNode unionNode)
        {
            bool matchesNode1 = Matches(unionNode.Node1, remainingInput);
            bool matchesNode2 = Matches(unionNode.Node2, remainingInput);
            return matchesNode1 || matchesNode2;
        }
        else if (node is ConcatenationNode concatenationNode)
        {
            for (int i = 1; i < remainingInput.Length; i++)
            {
                var part1 = remainingInput.Substring(0, i);
                var part2 = remainingInput.Substring(i);
                bool matchesNode1 = Matches(concatenationNode.Node1, part1);
                bool matchesNode2 = Matches(concatenationNode.Node2, part2);
                if (matchesNode1 && matchesNode2)
                {
                    return true;
                }
            }
            return false;
        }
        else if (node is IterationNode iterationNode)
        {
            if (remainingInput.Length == 0)
            {
                return true;
            }
            for (int i = 1; i < remainingInput.Length; i++)
            {
                var part1 = remainingInput.Substring(0, i);
                var part2 = remainingInput.Substring(i);
                bool matchesNode = Matches(iterationNode.Node, part1);
                bool matchesIterationNode = Matches(iterationNode, part2);
                if (matchesNode && matchesIterationNode)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            throw new ArgumentException("Unknown node type");
        }
    }
    
    public void ConstructNFA(List<RegexAndOperation> regexesAndOperations)
    {
        var nodes = new List<Node>();
        
        foreach (var regexAndOperation in regexesAndOperations)
        {
            Node newNode = null;
            
            
            if (regexAndOperation.IsSymbol)
            {
                newNode = new SymbolNode(regexAndOperation.Symbol);
            }
            else
            {
                var node1 = nodes[regexAndOperation.Operand1 - 1];
                Node node2 = null;
                if (regexAndOperation.Operand2.HasValue)
                {
                    node2 = nodes[regexAndOperation.Operand2.Value - 1];
                }

                if (regexAndOperation.IsUnion)
                {
                    newNode = new UnionNode(node1, node2);
                }
                else if (regexAndOperation.IsConcatenation)
                {
                    newNode = new ConcatenationNode(node1, node2);
                    node1.IsAccepting = false;
                }
                else if (regexAndOperation.IsIteration)
                {
                    newNode = new IterationNode(node1);
                }
            }

            nodes.Add(newNode);
        }

        foreach (var node in nodes)
        {
            if (node.Node11 == null && node.Node22 == null && node.IsAccepting == null)
            {
                node.IsAccepting = true;
            }
            else
            {
                node.IsAccepting = false;
            }
        }

        StartNode = nodes.Last();
    }
}

public class SymbolNode : Node
{
    public string Symbol { get; set; }

    public SymbolNode(string symbol)
    {
        Symbol = symbol;
        Symbol1 = symbol;
    }
}

public class UnionNode : Node
{
    public Node Node1 { get; set; }
    public Node Node2 { get; set; }

    public UnionNode(Node node1, Node node2)
    {
        Node1 = node1;
        Node2 = node2;
        Node11 = node1;
        Node22 = node2;
    }
}

public class ConcatenationNode : Node
{
    public Node Node1 { get; set; }
    public Node Node2 { get; set; }

    public ConcatenationNode(Node node1, Node node2)
    {
        Node1 = node1;
        Node2 = node2;
        Node11 = node1;
        Node22 = node2;
    }
}

public class IterationNode : Node
{
    public Node Node { get; set; }

    public IterationNode(Node node)
    {
        Node = node;
    }
}

public class RegexAndOperation
{
    public bool IsSymbol { get; set; }
    public bool IsUnion { get; set; }
    public bool IsConcatenation { get; set; }
    public bool IsIteration { get; set; }
    public string Symbol { get; set; }
    public int Operand1 { get; set; }
    public int? Operand2 { get; set; }
}