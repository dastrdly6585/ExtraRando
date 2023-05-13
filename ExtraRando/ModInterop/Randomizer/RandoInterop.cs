using ExtraRando.ModInterop.ItemChanger;
using ItemChanger;
using KorzUtils.Helper;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;
using System.IO;
using System.Linq;

namespace ExtraRando.ModInterop.Randomizer;

public static class RandoInterop
{
    #region Event handler

    private static void ApplySettings(RequestBuilder builder)
    {
        if (!ExtraRando.Instance.Settings.Enabled)
            return;
        if (ExtraRando.Instance.Settings.SplitShadeCloak && builder.gs.PoolSettings.Skills)
        {
            builder.RemoveItemByName(ItemNames.Shade_Cloak);
            builder.RemoveItemByName(ItemNames.Split_Shade_Cloak);
            builder.RemoveItemByName(ItemNames.Left_Mothwing_Cloak);
            builder.RemoveItemByName(ItemNames.Right_Mothwing_Cloak);

            builder.AddItemByName(ItemManager.Progressive_Left_Cloak, 2);
            builder.AddItemByName(ItemManager.Progressive_Right_Cloak, 2);
            if (builder.gs.DuplicateItemSettings.ShadeCloak)
            {
                builder.AddItemByName(ItemManager.Progressive_Left_Cloak, 2);
                builder.AddItemByName(ItemManager.Progressive_Right_Cloak, 2);
            }
        }
        if (ExtraRando.Instance.Settings.RandomizeHotSprings)
        {
            builder.AddItemByName(ItemManager.Hot_Spring_Water, 6);
            builder.EditItemRequest(ItemManager.Hot_Spring_Water, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Hot_Spring_Water,
                    Pool = "Soul",
                    PriceCap = 50
                };
            });

            builder.AddLocationByName(ItemManager.Colosseum_Hot_Spring);
            builder.AddLocationByName(ItemManager.Bathhouse_Hot_Spring);
            builder.AddLocationByName(ItemManager.Crossroads_Hot_Spring);
            builder.AddLocationByName(ItemManager.Deepnest_Hot_Spring);
            builder.AddLocationByName(ItemManager.Lower_Godhome_Hot_Spring);
            builder.AddLocationByName(ItemManager.Upper_Godhome_Hot_Spring);

        }
    }

    private static void ModifyLogic(GenerationSettings settings, LogicManagerBuilder builder)
    {
        if (!ExtraRando.Instance.Settings.Enabled)
            return;
        using Stream logicFile = ResourceHelper.LoadResource<ExtraRando>("Randomizer.Logic.json");
        builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, logicFile);

        if (ExtraRando.Instance.Settings.SplitShadeCloak && settings.PoolSettings.Skills)
        {
            Term leftDash = builder.GetOrAddTerm("LEFTDASH");
            Term rightDash = builder.GetOrAddTerm("RIGHTDASH");
            builder.AddItem(new SingleItem(ItemManager.Progressive_Left_Cloak, new(leftDash, 1)));
            builder.AddItem(new SingleItem(ItemManager.Progressive_Right_Cloak, new(rightDash, 1)));

            // Adjust macros since normally this will be considered true if ANY shade cloak has been obtained.
            builder.DoMacroEdit(new("LEFTSHADOWDASH", "LEFTDASH>1"));
            builder.DoMacroEdit(new("RIGHTSHADOWDASH", "RIGHTDASH>1"));
        }
        if (ExtraRando.Instance.Settings.RandomizeHotSprings)
        {
            using Stream waypointStream = ResourceHelper.LoadResource<ExtraRando>("Randomizer.Waypoints.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Waypoints, waypointStream);
            builder.AddItem(new EmptyItem(ItemManager.Hot_Spring_Water));
        }
    }

    private static void CheckForNoLogic(GenerationSettings settings, LogicManagerBuilder builder)
    {
        if (!ExtraRando.Instance.Settings.Enabled || !ExtraRando.Instance.Settings.NoLogic)
            return;
        foreach (string key in builder.LogicLookup.Keys.ToList())
            builder.DoLogicEdit(new(key, "TRUE"));
    }

    #endregion

    #region Methods

    internal static void Initialize()
    {
        RandoMenu.Initialize();
        RequestBuilder.OnUpdate.Subscribe(1050f, ApplySettings);
        RCData.RuntimeLogicOverride.Subscribe(1050f, ModifyLogic);
        RCData.RuntimeLogicOverride.Subscribe(float.MaxValue, CheckForNoLogic);
    }

    #endregion
}
