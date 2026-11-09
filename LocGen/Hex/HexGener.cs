using TermLibrary.HexNodes;

namespace TermLibrary.SubLocGener;

class HexGener
{
    private static HexRoom GenerateCircles(int num_layers, int layer_id)
    {
        HexRoom base_room = new HexRoom(0, layer_id);
        List<HexRoom> outer_nodes = [base_room];

        int id_counter = 1;
        for (int i = 1; i <= num_layers; i++)
        {
            // ==Count how much rooms==
            int num_new_rooms = 6 * i;

            // ==Gen rooms without licking==
            HexRoom[] new_cycle_rooms = new HexRoom[num_new_rooms];
            for (int j = 0; j < num_new_rooms; j++)
                new_cycle_rooms[j] = new HexRoom(id_counter++, layer_id);

            // ==Link rooms within==
            for (int j = 0; j < num_new_rooms; j++) // Loop new corridors
                new_cycle_rooms[j].AddNeighbour(new_cycle_rooms[(j + 1) % num_new_rooms]);

            // ==Connect circles==
            if (outer_nodes.Contains(base_room)) // Initial Connect
            {
                foreach (HexRoom room in new_cycle_rooms)
                    base_room.AddNeighbour(room);
                continue;
            }

            int step_btw_hex = num_new_rooms / 6;

            for (int j = 0; j < num_new_rooms; j++)
            {
                new_cycle_rooms[j].AddNeighbour(outer_nodes[j - (j / step_btw_hex)]); //FIX: Индекс j - (j / step_btw_hex) может выйти за границы outer_nodes при j < step_btw_hex
                if (j % step_btw_hex != 0)
                    new_cycle_rooms[j].AddNeighbour(outer_nodes[j - (j / step_btw_hex) - 1]); //FIX: Индекс может стать отрицательным (-1) при j < step_btw_hex
            }

            // ==Redefine outer_nodes==
            outer_nodes = new_cycle_rooms.ToList();
        }
        return base_room;
    }

    public static List<HexRoom> DeepSearch(HexRoom base_node)
    {
        HashSet<HexRoom> visited_nodes = [base_node];

        DeepSearch(base_node, visited_nodes);

        return visited_nodes.ToList();
    }

    private static List<HexRoom> DeepSearchByLayer(
        List<HexRoom> cur_layer,
        HashSet<HexRoom> visited_nodes
    )
    {
        foreach (HexRoom room in cur_layer.ToList()) //FIX: Модификация cur_layer (AddRange) внутри foreach по той же коллекции вызовет InvalidOperationException
        {
            List<HexRoom> new_rooms = room.GetNextRooms()
                .Where(r => !visited_nodes.Contains(r))
                .ToList();

            cur_layer.AddRange(new_rooms);
            visited_nodes.UnionWith(new_rooms);
        }

        if (cur_layer == null) //FIX: cur_layer никогда не будет null после foreach, проверка бесполезна
            return visited_nodes.ToList();
        return DeepSearchByLayer(cur_layer, visited_nodes); //FIX: Рекурсия не имеет условия выхода кроме null, приведёт к StackOverflow - cur_layer только растёт
    }

    private static void DeepSearch(HexRoom cur_room, HashSet<HexRoom> visited_nodes)
    {
        List<HexRoom> new_rooms = cur_room
            .GetNextRooms()
            .Where(r => !visited_nodes.Contains(r))
            .ToList();

        visited_nodes.UnionWith(new_rooms);

        foreach (HexRoom new_room in new_rooms)
        {
            DeepSearch(new_room, visited_nodes);
        }
    }

    private static void ConectLayers(HexRoom base_room_1, HexRoom base_room_2)
    {
        List<HexRoom> all_nodes_1 = DeepSearch(base_room_1);
        List<HexRoom> all_nodes_2 = DeepSearch(base_room_2);

        var layer_2 = all_nodes_2.ToDictionary(b => b.id);

        foreach (HexRoom room_a in all_nodes_1)
            if (layer_2.TryGetValue(room_a.id, out var room_b))
                room_a.AddNeighbourUp(room_b);
    }

    private static HexRoom GenerateLayers(int num_layers)
    {
        HexRoom base_room = GenerateCircles(num_layers, 0);
        List<HexRoom> layers = [base_room];

        for (int i = -num_layers / 2; i <= num_layers / 2; i++)
        {
            if (i == 0)
                continue;
            layers.Add(GenerateCircles(num_layers, i));
        }
        layers.Sort((x, y) => x.layer_id.CompareTo(y.layer_id));

        for (int i = 0; i < layers.Count - 1; i++)
        {
            ConectLayers(layers[i], layers[i + 1]);
        }
        return base_room;
    }

    public static void GenerateLevel()
    {
        int num_layers = Random.Shared.Next(8, 15);
        HexRoom base_room = GenerateLayers(num_layers);
    }
}
