namespace AutoRegex;

public abstract class Node
{
    public Node? Node11 { get; set; }
    public Node? Node22 { get; set; }
    
    
    public bool? IsAccepting { get; set; }
    public string? Symbol1 { get; set; }
    
    public Node()
    {
        IsAccepting = null;
    }
    
    public Node Clone()
    {
        var node = (Node) MemberwiseClone();
        node.IsAccepting = true;
        return node;
    }
}