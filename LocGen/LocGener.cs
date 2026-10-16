using TermLibrary.HexNodes;
using TermLibrary.SubLocGener;

namespace TermLibrary.LocController;

class LocController
{
    private void GenLocation(General_Map.Node new_loc)
    {
        switch (new_loc.name)
        {
            case "Hex":
                // TODO: Check Exist
                HexGener.GenerateLevel();
                // TODO: Save
                break;
            default:
                break;
        }
    }

    public LocController(List<General_Map.Node> all_locations)
    {
        foreach (General_Map.Node super_location in all_locations)
        {
            GenLocation(super_location);
        }
    }
}
