using TermLibrary.SubLocGener;
using TermLibrary.HexNodes;

namespace TermLibrary.LocController;

class LocController
{
    private void GenLocation(General_Map.Node new_loc)
    {
        switch (new_loc.name)
        {
            case "Hex":
                // TODO: Check Exist
                int num_layers = Random.Shared.Next(1, 15);
                HexRoom base_hex_node = HexGener.GrownLayer(num_layers);
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
