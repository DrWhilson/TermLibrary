using TermLibrary.General_Map;
using TermLibrary.LocGener;

class Program
{
    static void Main()
    {
        GenMap gen_map = new GenMap();

        LocGener loc_gener = new LocGener(gen_map.all_locations);
    }
}
