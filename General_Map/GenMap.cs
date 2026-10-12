using System.Text.Json;
using System.Text.Json.Serialization;
using maps;

namespace TermLibrary.General_Map;

public class GenMap
{
    public List<Node> all_locations { get; private set; } = new List<Node>();

    private RuleMap rule_map { get; set; }

    private bool CheckReachable(Node start_node, (Node start, Node end) ignored_link)
    {
        if (start_node == null || ignored_link.start == null || ignored_link.end == null)
            return false;

        HashSet<Node> visited = new HashSet<Node>();

        return DeepSearch(start_node, (ignored_link.start, ignored_link.end), visited)
            == all_locations.Count; //FIX: Assumes graph is fully connected from start_node. During DropSomeLinks, nodes may already be disconnected making count mismatch
    }

    private int DeepSearch(Node current, (Node start, Node end) ignored_link, HashSet<Node> visited)
    {
        visited.Add(current);
        int count = 1;

        foreach (Node neighbor in current.links)
        {
            if (
                !visited.Contains(neighbor)
                && (current != ignored_link.start || neighbor != ignored_link.end)
                && (current != ignored_link.end || neighbor != ignored_link.start)
            )
            {
                count += DeepSearch(neighbor, ignored_link, visited);
            }
        }

        return count;
    }

    private void GenerateBaseMap()
    {
        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex); //FIX: Constructor already adds a "Hex" node before calling this, creating duplicate

        GenerateNodes(); // Gen couple of locations

        GenerateFullLink(); // Gen max of links

        DropSomeLinks(50);
    }

    private void GenerateNodes()
    {
        List<string> posible_locations = rule_map.GetAllNames();

        foreach (string unique_location in posible_locations)
        {
            for (int i = 0; i < Random.Shared.Next(3, 8); i++) // Random number of any locations //FIX: Creates multiple nodes with same name. Linking logic uses names, may cause ambiguity
            {
                Node newLocation = new Node(unique_location);
                all_locations.Add(newLocation);
            }
        }
    }

    private void TryLink(Node node1, Node node2)
    {
        if (node1 == node2)
            return;

        if (!rule_map.GetLinksFor(node1.name).Contains(node2.name))
            return; //FIX: GetLinksFor returns bidirectional links, may allow unintended connections

        node1.AddNewNeighbor(node2);
        node2.AddNewNeighbor(node1);
    }

    private void GenerateFullLink()
    {
        foreach (Node location in all_locations)
            foreach (Node other_locations in all_locations)
                TryLink(location, other_locations); //FIX: Iterates all pairs including (A,B) and (B,A). TryLink adds bidirectional links, creating duplicate edges
    }

    private void DropLink(Node node1, Node node2)
    {
        node1.DropNeighbour(node2);
        node2.DropNeighbour(node1);
    }

    private void DropSomeLinks(int percent)
    {
        foreach (Node location in all_locations)
        {
            List<Node> neighbour = location.links; //FIX: This is a reference to the original list, not a copy. Modifying it during iteration causes issues
            for (int i = 0; i < neighbour.Count; i++) //FIX: Dropping links modifies the list while iterating, causing index shifting and skipped elements
            {
                if (
                    Random.Shared.Next(100) < percent
                    && CheckReachable(location, (location, neighbour[i]))
                )
                    DropLink(location, neighbour[i]);
            }
        }
    }

    private void SaveGraph(string file_name = "rule_map_graph.json")
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        string folder_name = "Saves";

        string folder_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder_name);

        if (!Directory.Exists(folder_path))
        {
            Directory.CreateDirectory(folder_path);
        }

        string full_path = Path.Combine(folder_path, file_name);

        string jsonString = JsonSerializer.Serialize(all_locations, options);
        File.WriteAllText(full_path, jsonString);
    }

    bool LoadGraph(string file_name = "rule_map_graph.json")
    {
        string folder_name = "Saves";

        var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve };

        string folder_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder_name);

        if (!Directory.Exists(folder_path))
            return false;

        string full_path = Path.Combine(folder_path, file_name);

        string readJson = File.ReadAllText(full_path);
        var restoredGraph = JsonSerializer.Deserialize<List<Node>>(readJson, options);

        if (restoredGraph != null)
            all_locations = restoredGraph;
        else
            return false;

        return true;
    }

    public GenMap(bool force_regen = false)
    {
        string path = @"rule.json";
        rule_map = RuleMap.LoadFromFile(path);

        if (!force_regen && LoadGraph())
            return;

        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex); //FIX: GenerateBaseMap() also creates a "Hex" node, resulting in duplicate "Hex" nodes when force_regen=true or LoadGraph fails

        GenerateBaseMap();

        SaveGraph();
    }
}
