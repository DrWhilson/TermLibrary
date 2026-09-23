using maps;

namespace MyProject.General_Map;

public class GenMap
{
    private List<Node> all_locations { get; set; } = new List<Node>();

    private RuleMap rule_map { get; set; }

    public Node GenerateBaseMap()
    {
        return all_locations[0];
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

    public GenMap()
    {
        string path = @"rule.json";
        rule_map = RuleMap.LoadFromFile(path);

        Node start_hex = new Node("Hex");
        all_locations.Add(start_hex);

        GenerateNodes();
    }
}
