namespace AutoRegex;

public class Edge
{
    public string Symbol { get; set; }
    public Node NextNode { get; set; }

    public Edge(string symbol, Node nextNode)
    {
        Symbol = symbol;
        NextNode = nextNode;
    }
}