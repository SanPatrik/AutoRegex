namespace AutoRegex;

public class Automat
{
    public Node? InitialNode { get; set; }
    public List<Node> AcceptNodes { get; set; }

    public Automat()
    {
        AcceptNodes = new List<Node>();
    }
}

public class Node
{
    public List<Tuple<Node, string>> Edges { get; set; }
    
    public Node()
    {
        Edges = new List<Tuple<Node, string>>();
    }
    public void AddEdge(Node node, string symbol)
    {
        Edges.Add(new Tuple<Node, string>(node, symbol));
    }
}