using TermLibrary.HexNodes;

namespace TermLibrary.SubLocGener;

class HexGener
{
    private static HexRoom GenerateCircles(int num_layers)
    {
        HexRoom base_room = new HexRoom(0);
        List<HexRoom> outer_nodes = [base_room];

        int id_counter = 1;
        for (int i = 1; i <= num_layers; i++)
        {
            // ==Count how much rooms==
            int num_new_rooms = 6 * i;

            // ==Gen rooms without licking==
            HexRoom[] new_cycle_rooms = new HexRoom[num_new_rooms];
            for (int j = 0; j < num_new_rooms; j++)
                new_cycle_rooms[j] = new HexRoom(id_counter++);

            // ==Link rooms within==
            for (int j = 0; j < num_new_rooms; j++) // Loop new corridors
                new_cycle_rooms[j].AddNeighbour(new_cycle_rooms[(j + 1) % num_new_rooms]);

            // ==Connect circles==
            if (outer_nodes.Contains(base_room))
            {
                foreach (HexRoom room in new_cycle_rooms) base_room.AddNeighbour(room);
                continue;
            }

            int step_btw_hex = num_new_rooms / 6;

            for (int j = 0; j < num_new_rooms; j++)
            {
                new_cycle_rooms[j].AddNeighbour(outer_nodes[j - (j / step_btw_hex)]);
                if (j % step_btw_hex != 0)
                    new_cycle_rooms[j].AddNeighbour(outer_nodes[j - (j / step_btw_hex) - 1]);
            }

            // ==Redefine outer_nodes==
            outer_nodes = new_cycle_rooms.ToList();
        }
        return base_room;
    }

    private static HexRoom GenerateLayers(int num_layers)
    {
        HexRoom base_room = GenerateCircles(num_layers);
        return base_room;
    }

    public static void GenerateLevel()
    {
        int num_layers = Random.Shared.Next(1, 15);
        HexRoom base_room = GenerateLayers(num_layers);
    }
}
