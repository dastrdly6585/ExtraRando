using ExtraRando.Enums;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Placements;
using KorzUtils.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class PinItem : AbstractItem
{
    #region Members

    // Also include all items which contains "Geo_Rock", "Lore_Tablet", "Journal_Entry", "Geo_Chest", "Hunter's_Notes", "Boss_Geo" "Map" and "Soul_Totem"
    private static readonly string[] _viableUselessItems = new string[]
    {
        ItemNames.Soul_Refill,
        ItemManager.Hot_Spring_Water,
        ItemNames.Hunters_Journal,
        ItemNames.Quill,
        ItemNames.Godtuner
    };

    private static readonly string[] _viablePotentialItems = new string[]
    {
        ItemNames.Wanderers_Journal,
        ItemNames.Hallownest_Seal,
        ItemNames.Kings_Idol,
        ItemNames.Arcane_Egg,
        ItemNames.Grub,
        ItemNames.Rancid_Egg,
        ItemNames.Simple_Key,
        ItemNames.Tram_Pass,
        ItemNames.Elevator_Pass,
        ItemNames.Elegant_Key,
        ItemNames.Love_Key,
        ItemNames.Gathering_Swarm,
        ItemNames.Grubsong,
        ItemNames.Stalwart_Shell,
        ItemNames.Baldur_Shell,
        ItemNames.Fury_of_the_Fallen,
        ItemNames.Quick_Focus,
        ItemNames.Lifeblood_Heart,
        ItemNames.Lifeblood_Core,
        ItemNames.Defenders_Crest,
        ItemNames.Flukenest,
        ItemNames.Mark_of_Pride,
        ItemNames.Steady_Body,
        ItemNames.Heavy_Blow,
        ItemNames.Sharp_Shadow,
        ItemNames.Spore_Shroom,
        ItemNames.Longnail,
        ItemNames.Shaman_Stone,
        ItemNames.Soul_Catcher,
        ItemNames.Soul_Eater,
        ItemNames.Glowing_Womb,
        ItemNames.Fragile_Heart,
        ItemNames.Fragile_Greed,
        ItemNames.Fragile_Strength,
        ItemNames.Unbreakable_Greed,
        ItemNames.Unbreakable_Heart,
        ItemNames.Unbreakable_Strength,
        ItemNames.Nailmasters_Glory,
        ItemNames.Jonis_Blessing,
        ItemNames.Shape_of_Unn,
        ItemNames.Hiveblood,
        ItemNames.Dream_Wielder,
        ItemNames.Dashmaster,
        ItemNames.Quick_Slash,
        ItemNames.Spell_Twister,
        ItemNames.Deep_Focus,
        ItemNames.Grubberflys_Elegy,
        ItemNames.Sprintmaster,
        ItemNames.Dreamshield,
        ItemNames.Weaversong,
        ItemNames.Grimmchild1,
        ItemNames.Grimmchild2,
        ItemNames.Pale_Ore,
        ItemNames.Mask_Shard,
        ItemNames.Double_Mask_Shard,
        ItemNames.Full_Mask,
        ItemNames.Vessel_Fragment,
        ItemNames.Double_Vessel_Fragment,
        ItemNames.Full_Soul_Vessel,
        ItemNames.Charm_Notch,
        ItemNames.Great_Slash,
        ItemNames.Dash_Slash,
        ItemNames.Cyclone_Slash,
        "Nail_Upgrade", // For Rando plus
        "Bronze_Trial_Ticket", // Curse Rando
        "Silver_Trial_Ticket", // Curse Rando
        "Gold_Trial_Ticket" // Curse Rando
    };

    private static readonly string[] _viableUsefulItems = new string[]
    {
        // Skills
        ItemNames.Focus,
        ItemNames.Mothwing_Cloak,
        ItemNames.Left_Mothwing_Cloak,
        ItemNames.Right_Mothwing_Cloak,
        ItemNames.Split_Shade_Cloak,
        ItemNames.Shade_Cloak,
        ItemManager.Cloak,
        ItemManager.Left_Cloak,
        ItemManager.Right_Cloak,
        ItemManager.Progressive_Left_Cloak,
        ItemManager.Progressive_Right_Cloak,
        ItemNames.Mantis_Claw,
        ItemNames.Left_Mantis_Claw,
        ItemNames.Right_Mantis_Claw,
        ItemNames.Monarch_Wings,
        ItemNames.Crystal_Heart,
        ItemNames.Left_Crystal_Heart,
        ItemNames.Right_Crystal_Heart,
        ItemNames.Ismas_Tear,
        ItemNames.Swim,
        ItemNames.Dream_Nail,
        ItemNames.Dream_Gate,
        ItemNames.Awoken_Dream_Nail,
        ItemNames.Vengeful_Spirit,
        ItemNames.Shade_Soul,
        ItemManager.Left_Vengeful_Spirit,
        ItemManager.Left_Shade_Soul,
        ItemManager.Left_Fireball,
        ItemManager.Right_Vengeful_Spirit,
        ItemManager.Right_Shade_Soul,
        ItemManager.Right_Fireball,
        ItemNames.Desolate_Dive,
        ItemNames.Descending_Dark,
        ItemNames.Howling_Wraiths,
        ItemNames.Abyss_Shriek,
        ItemManager.Fireball_Spell,
        ItemManager.Dive_Spell,
        ItemManager.Scream_Spell,
        ItemManager.Key_Ring,
        // Charms
        ItemNames.Kingsoul,
        ItemNames.Void_Heart,
        ItemNames.Wayward_Compass,
        // Dreamer
        ItemNames.Monomon,
        ItemNames.Lurien,
        ItemNames.Herrah,
        ItemNames.Dreamer,
        // Nail
        ItemNames.Leftslash,
        ItemNames.Rightslash,
        ItemNames.Upslash,
        // Other
        "Listen_Ability", // Lore Rando
        "Read_Ability", // Lore Rando
        "Geo_Wallet", // Curse Rando
        "Dreamnail_Fragment", // Curse Rando
        ItemManager.Key_Ring
    };

    private const string ShellWhisper = "You can find '{0}' at '{1}'.";
    private const string TokenWhisper = "An important item can be found at '{0}'.";
    private const string ScarabWhisper = "'{0}' holds junk";
    private const string GleamingWhisper = "Something potentially useful lies at '{0}'.";

    #endregion

    #region Properties

    public MarkerType Type { get; set; }

    public string LocationName { get; set; }

    public string ItemName { get; set; }

    #endregion

    #region Methods

    public override void GiveImmediate(GiveInfo info)
    {
        // If for some reason the game manager doesn't exist, we create a temporarly holder.
        if (GameManager.instance is null)
        {
            GameObject holderObject = new("Hint Holder");
            holderObject.AddComponent<NonBouncer>().StartCoroutine(WaitForControl());
        }
        else
            GameManager.instance.StartCoroutine(WaitForControl());
    }

    private IEnumerator WaitForControl()
    {
        while (HeroController.instance?.acceptingInput != true)
            yield return null;
        if (HeroController.instance != null)
            try
            {
                if (string.IsNullOrEmpty(LocationName))
                {
                    if (Type == MarkerType.ShellMarker)
                    {
                        List<AbstractPlacement> viablePlacements = Ref.Settings.Placements.Where(x => !x.Value.AllObtained() && x.Value is not IMultiCostPlacement)
                            .Select(x => x.Value)
                            .ToList();
                        if (!viablePlacements.Any())
                            yield break;
                        AbstractPlacement selectedPlacement = viablePlacements[UnityEngine.Random.Range(0, viablePlacements.Count)];
                        LocationName = selectedPlacement.Name.Replace("_", " ").Replace("-", " ");
                        ItemName = selectedPlacement.Items.First(x => !x.IsObtained()).name.Replace("_", " ").Replace("-", " ");
                    }
                    else
                    {
                        List<string> matchingLocations;
                        matchingLocations = GetViableLocations();
                        if (!matchingLocations.Any())
                            yield break;
                        LocationName = matchingLocations[UnityEngine.Random.Range(0, matchingLocations.Count)];
                    }
                }
                GameHelper.DisplayMessage(Type switch
                {
                    MarkerType.GleamingMarker => string.Format(GleamingWhisper, LocationName),
                    MarkerType.ScarabMarker => string.Format(ScarabWhisper, LocationName),
                    MarkerType.TokenMarker => string.Format(TokenWhisper, LocationName),
                    _ => string.Format(ShellWhisper, ItemName, LocationName)
                });
            }
            catch (Exception exception)
            {
                LogHelper.Write<ExtraRando>("Failed to generate hint: ", exception);
            }
    }

    private List<string> GetViableLocations()
    {
        List<string> viablePlacements = new();
        if (Type == MarkerType.ScarabMarker)
            viablePlacements = Ref.Settings.Placements.Where(x =>
            {
                if (x.Value.Items.Any(x => x.IsObtained()))
                    return false;
                string name = x.Key;
                if (_viableUselessItems.Contains(name))
                    return true;
                foreach (AbstractItem item in x.Value.Items)
                    if (item.name.StartsWith("Geo_Rock") || item.name.StartsWith("Soul_Totem")
                        || item.name.EndsWith("_Map") || item.name.StartsWith("Lore_Tablet") || item.name.StartsWith("Journal_Entry")
                        || item.name.StartsWith("Hunter's_Notes") || item.name.StartsWith("Boss_Geo") || item.name.StartsWith("Geo_Chest"))
                        return true;

                return false;
            }).Select(x => x.Key.Replace("_", " ").Replace("-", " "))
            .ToList();
        else if (Type == MarkerType.GleamingMarker)
            viablePlacements = Ref.Settings.Placements.Where(x => x.Value.Items.All(x => !x.IsObtained())
            && x.Value.Items.Any(item => _viablePotentialItems.Contains(item.name))).Select(x => x.Key.Replace("_", " ").Replace("-", " "))
              .ToList();
        else
            viablePlacements = Ref.Settings.Placements.Where(x => x.Value.Items.All(x => !x.IsObtained())
            && x.Value.Items.Any(item => _viableUsefulItems.Contains(item.name))).Select(x => x.Key.Replace("_", " ").Replace("-", " "))
              .ToList();
        return viablePlacements;
    } 

    #endregion
}
