using ItemChanger;
using KorzUtils.Helper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ExtraRando.ModInterop.ItemChanger;

public static class ItemManager
{
    #region Constants

    #region Items

    public const string Progressive_Left_Cloak = "Progressive_Left_Cloak";
    public const string Progressive_Right_Cloak = "Progressive_Right_Cloak";
    public const string Hot_Spring_Water = "Hot_Spring_Water";

    #endregion

    #region Locations

    public const string Crossroads_Hot_Spring = "Crossroads-Hot_Spring";
    public const string Colosseum_Hot_Spring = "Colosseum-Hot_Spring";
    public const string Deepnest_Hot_Spring = "Deepnest-Hot_Spring";
    public const string Bathhouse_Hot_Spring = "Crossroads-Hot_Spring";
    public const string Lower_Godhome_Hot_Spring = "Lower_Godhome-Hot_Spring";
    public const string Upper_Godhome_Hot_Spring = "Upper_Godhome-Hot_Spring";

    #endregion

    #endregion

    #region Methods

    internal static void Initialize()
    {
        JsonSerializer jsonSerializer = new() { TypeNameHandling = TypeNameHandling.Auto };
        using Stream itemStream = ResourceHelper.LoadResource<ExtraRando>("ItemChanger.Items.json");
        using StreamReader reader = new(itemStream);

        foreach (AbstractItem item in jsonSerializer.Deserialize<List<AbstractItem>>(new JsonTextReader(reader)))
            Finder.DefineCustomItem(item);

        using Stream locationStream = ResourceHelper.LoadResource<ExtraRando>("ItemChanger.Locations.json");
        using StreamReader reader2 = new(locationStream);
        foreach (AbstractLocation location in jsonSerializer.Deserialize<List<AbstractLocation>>(new JsonTextReader(reader2)))
            Finder.DefineCustomLocation(location);
    }

    #endregion
}
