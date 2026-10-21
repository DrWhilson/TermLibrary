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
            //FIX: GetNextRooms() может вернуть null (коридор, связанный только с одной комнатой, даёт в GetOpositRoom() null),
            //FIX: null проходит проверку "не посещена", попадает в new_outer и в visited_rooms, и на следующем уровне рекурсии room.GetNextRooms() падает с NullReferenceException

            visited_rooms.UnionWith(new_outer);
            //FIX: visited_rooms меняется, пока текущий круг ещё разворачивается, поэтому сосед, найденный предыдущими комнатами круга,
            //FIX: отсекается у остальных комнат этого же круга (комната приписывается не тому "родителю", а порядок результата зависит от порядка curr_circle);
            //FIX: объединение надо делать после цикла, и тогда ещё нужен .Distinct(), чтобы не было дубликатов
        }

        if (new_outer.Any())
            return FindOuterCircle(new_outer, visited_rooms);

        return curr_circle; //FIX: комнаты возвращаются в порядке обнаружения BFS,
        //FIX: а не в круговом порядке, хотя вызывающий код (GenerateCircles) обращается с результатом как с упорядоченным кругом (outer_nodes[j],
        //FIX: шаг Count/6). На несвязном графе (базовая комната не связана с первым кольцом) метод возвращает исходный круг как "внешний"
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

            outer_nodes = FindOuterCircle(outer_nodes, visited_nodes); //FIX: базовая комната ни разу не связывается с первым кольцом,
            // FIX: а кольца не связаны между собой (цикл "Connect circles" ниже пустой), поэтому граф несвязный, FindOuterCircle всегда возвращает только базовую комнату и внешний круг фактически не находится

            // Connect circles
            int step_new = num_new_rooms / 6;
            int step_old = outer_nodes.Count / 6; //FIX: outer_nodes содержит только базовую комнату, поэтому step_old равен 0 и секторная логика ломается (как только цикл соединения начнёт шагать на step_old, он не закончится)

            for (int outer_stepper = 0, inner_steper = 0; outer_stepper < num_new_rooms; outer_stepper += step_new, inner_steper += step_old)
            {
                // TODO: connect nodes
            }
        }
        return base_room;
    }

    public static void GenerateLevel()
    {
        int num_layers = Random.Shared.Next(1, 15);
        HexRoom base_hex_node = GenerateCircles(num_layers);
    }
}
