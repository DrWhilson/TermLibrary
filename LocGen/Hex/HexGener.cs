using TermLibrary.HexNodes;

namespace TermLibrary.SubLocGener;

class HexGener
{
    // public static HexRoom GrownRecur(HexRoom center, int curr_layer, int curr_id) //TODO: gen with recurs

    public static HexRoom GenerateCurcles(int num_layers)
    {
        HexRoom base_room = new HexRoom(0);

        int id_counter = 1;
        for (int i = 0; i < num_layers; i++)
        {
            // Count how much rooms
            int num_new_rooms = 6 * num_layers;
            int num_new_corr = num_new_rooms + (6 * (1 + 2 * num_layers));

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
                new_cycle_corridors[i] = new_corridor;
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

        }
        return base_room;
    }
}
