// using TermLibrary.General_Map;
using TermLibrary.LocNodes;

namespace TermLibrary.LocGener;

class LocGener
{
    private void GenLocation(General_Map.Node new_loc)
    {
        switch (new_loc.name)
        {
            case "Hex":
                break;
            default:
                break;
        }
    }

    public LocGener(List<General_Map.Node> all_locations)
    {
        foreach (General_Map.Node super_location in all_locations)
        {
            GenLocation(super_location);
        }
    }
}
