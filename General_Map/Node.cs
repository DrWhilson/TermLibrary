using System.Text.Json.Serialization;

namespace TermLibrary.General_Map;

public class Node
{
    [JsonInclude]
    public string name { get; private set; } = string.Empty;

    [JsonInclude]
    public List<Node> links { get; private set; } = new List<Node>();

    public void AddNewNeighbor(Node newNode)
    {
        if (newNode != null)
            links.Add(newNode); //FIX: No duplicate check. Calling multiple times with same node adds duplicates to links list
    }

    public void DropNeighbour(Node neighbour)
    {
        if (!links.Contains(neighbour))
            return;

        links.Remove(neighbour); //FIX: Removes only first occurrence. If duplicates exist (see AddNewNeighbor), only one is removed
    }

    public Node() { }

    public Node(string new_name)
    {
        name = new_name;
    }
}
