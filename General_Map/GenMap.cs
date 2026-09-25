using maps;

namespace MyProject.General_Map;

public class GenMap
{
    private List<Node> all_locations { get; set; } = new List<Node>();

    private RuleMap rule_map { get; set; }

    private void GenerateBaseMap()
    {
        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex);

        GenerateNodes(); // Gen couple of locations

        GenerateFullLink(); // Gen max of links
    }

    private void GenerateNodes()
    {
        List<string> posible_locations = rule_map.GetAllNames();

        foreach (string unique_location in posible_locations)
        {
            for (int i = 0; i < Random.Shared.Next(3, 8); i++) // Random number of any locations
            {
                Node newLocation = new Node(unique_location);
                all_locations.Add(newLocation);
            }
        }
    }

    private void TryLink(Node node1, Node node2)
    {
        if (node1 == node2) return;

        if (!rule_map.GetLinksFor(node1.GetName()).Contains(node2.GetName())) return;

        node1.AddNewNeighbor(node2);
        node2.AddNewNeighbor(node1);
    }

    void GenerateFullLink()
    {
        foreach (Node location in all_locations)
            foreach (Node other_locations in all_locations)
                TryLink(location, other_locations);
    }

    public GenMap()
    {
        string path = @"rule.json";
        rule_map = RuleMap.LoadFromFile(path);

        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex);

        GenerateNodes();
    }
}
