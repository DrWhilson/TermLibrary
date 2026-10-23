using TermLibrary.HexNodes;

namespace TermLibrary.SubLocGener;

class HexGener
{
    public static HexRoom GenerateCircles(int num_layers)
    {
        HexRoom base_room = new HexRoom(0);
        List<HexRoom> outer_nodes = [base_room];

        int id_counter = 1;
        for (int i = 1; i <= num_layers; i++)
        {
            // ==Count how much rooms==
            int num_new_rooms = 6 * i;
            int num_new_corr = num_new_rooms + (6 * (1 + 2 * (i - 1)));

            // ==Gen rooms without licking==
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

            // ==Link rooms within==
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

            // ==Connect circles==
            int step_new = num_new_rooms / 6;
            int step_old = outer_nodes.Count / 6;

            for (int outer_stepper = 0, inner_steper = 0; outer_stepper < num_new_rooms; outer_stepper += step_new, inner_steper += step_old)
            {
                // TODO: connect nodes
            }

            // ==Redefine outer_nodes==
            outer_nodes = new_cycle_rooms.ToList();
        }
        return base_room;
    }

    public static void GenerateLevel()
    {
        int num_layers = Random.Shared.Next(1, 15);
        HexRoom base_hex_node = GenerateCircles(num_layers);
    }
}
