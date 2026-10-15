using TermLibrary.HexNodes;

namespace TermLibrary.SubLocGener;

class HexGener
{
    public static List<HexRoom> FindOuterCircle(
        List<HexRoom> curr_circle,
        HashSet<HexRoom> visited_rooms
    )
    {
        List<HexRoom> new_outer = new List<HexRoom>();

        foreach (HexRoom room in curr_circle)
        {
            new_outer.AddRange(
                room.GetNextRooms().Where(val => !visited_rooms.Contains(val)).ToList()
            );

            visited_rooms.UnionWith(new_outer);
        }

        if (new_outer.Any())
            return FindOuterCircle(new_outer, visited_rooms);

        return curr_circle;
    }

    public static HexRoom GenerateCircles(int num_layers)
    {
        HexRoom base_room = new HexRoom(0);

        int id_counter = 1;
        for (int i = 1; i <= num_layers; i++)
        {
            // Count how much rooms
            int num_new_rooms = 6 * i;
            int num_new_corr = num_new_rooms + (6 * (1 + 2 * (i - 1)));

            // Gen rooms without licking
            HexRoom[] new_cycle_rooms = new HexRoom[num_new_rooms];
            for (int j = 0; j < num_new_rooms; j++)
            {
                HexRoom new_room = new HexRoom(id_counter);
                new_cycle_rooms[j] = new_room;
                id_counter++;
            }
            HexCorridor[] new_cycle_corridors = new HexCorridor[num_new_corr];
            for (int j = 0; j < num_new_corr; j++)
            {
                HexCorridor new_corridor = new HexCorridor(id_counter);
                new_cycle_corridors[j] = new_corridor;
                id_counter++;
            }

            // Link rooms within
            for (int j = 0; j < num_new_rooms; j++) // Loop new corridors
            {
                new_cycle_rooms[j].AddNeighbour(new_cycle_corridors[j]);
                new_cycle_corridors[j].AddNeighbour(new_cycle_rooms[j]);

                if (j == num_new_rooms - 1)
                {
                    new_cycle_rooms[0].AddNeighbour(new_cycle_corridors[j]);
                    new_cycle_corridors[j].AddNeighbour(new_cycle_rooms[0]);
                }
                else
                {
                    new_cycle_rooms[j + 1].AddNeighbour(new_cycle_corridors[j]);
                    new_cycle_corridors[j].AddNeighbour(new_cycle_rooms[j + 1]);
                }
            }

            // Find outer circle
            HashSet<HexRoom> visited_nodes = new HashSet<HexRoom>();
            visited_nodes.Add(base_room);
            List<HexRoom> outer_nodes = new List<HexRoom>();
            outer_nodes.Add(base_room);

            outer_nodes = FindOuterCircle(outer_nodes, visited_nodes);
        }
        return base_room;
    }
}
