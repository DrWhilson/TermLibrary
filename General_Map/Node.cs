namespace MyProject.General_Map;

public class Node
{
    private string name { get; set; }

    private List<Node> links { get; set; } = new List<Node>();
    public void AddNewNeighbor(Node newNode)
    {
        if (newNode != null) links.Add(newNode);
    }

    public List<Node> GetAllNeighbours()
    {
        return links;
    }

    public string GetName()
    {
        return this.name;
    }

    public Node(string new_name)
    {
        name = new_name;
    }
}
