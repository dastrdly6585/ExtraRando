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

    public const string Fireball_Spell = "Fireball_Spell";
    public const string Dive_Spell = "Dive_Spell"; 
    public const string Scream_Spell = "Scream_Spell";
    public const string Left_Cloak = "Left_Cloak";
    public const string Right_Cloak = "Right_Cloak";
    public const string Cloak = "Cloak";
    public const string Key_Ring = "Key_Ring";
    public const string Progressive_Left_Cloak = "Progressive_Left_Cloak";
    public const string Progressive_Right_Cloak = "Progressive_Right_Cloak";
    public const string Hot_Spring_Water = "Hot_Spring_Water";
    public const string Scarab_Marker_Hint = "Scarab_Marker_Hint";
    public const string Shell_Marker_Hint = "Shell_Marker_Hint";
    public const string Gleaming_Marker_Hint = "Gleaming_Marker_Hint";
    public const string Token_Marker_Hint = "Token_Marker_Hint";
    public const string Colo_Ticket_Bronze = "Colo_Ticket-Bronze"; 
    public const string Colo_Ticket_Silver = "Colo_Ticket-Silver";
    public const string Colo_Ticket_Gold = "Colo_Ticket-Gold";
    public const string Pantheon_Access_Master = "Pantheon_Access-Master";
    public const string Pantheon_Access_Artist = "Pantheon_Access-Artist";
    public const string Pantheon_Access_Sage = "Pantheon_Access-Sage";
    public const string Pantheon_Access_Knight = "Pantheon_Access-Knight"; 
    public const string Pantheon_Access_Hallownest = "Pantheon_Access-Hallownest";

    #endregion

    #region Locations

    public const string Crossroads_Hot_Spring = "Crossroads-Hot_Spring";
    public const string Colosseum_Hot_Spring = "Colosseum-Hot_Spring";
    public const string Deepnest_Hot_Spring = "Deepnest-Hot_Spring";
    public const string Bathhouse_Hot_Spring = "Crossroads-Hot_Spring";
    public const string Lower_Godhome_Hot_Spring = "Lower_Godhome-Hot_Spring";
    public const string Upper_Godhome_Hot_Spring = "Upper_Godhome-Hot_Spring";
    public const string Pantheon_Master = "Pantheon-Master";
    public const string Pantheon_Artist = "Pantheon-Artist";
    public const string Pantheon_Sage = "Pantheon-Sage";
    public const string Pantheon_Knight = "Pantheon-Knight";
    public const string Pantheon_Hallownest = "Pantheon-Hallownest";

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
