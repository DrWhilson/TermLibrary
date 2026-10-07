using TermLibrary.General_Map;
using TermLibrary.LocController;

class Program
{
    static void Main()
    {
        GenMap gen_map = new GenMap();

        LocController loc_gener = new LocController(gen_map.all_locations);
    }
}
