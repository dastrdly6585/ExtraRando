using ExtraRando.ModInterop.ItemChanger;
using ItemChanger;
using KorzUtils.Helper;
using Modding;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.Logging;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using RandomizerMod.Settings;
using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using System.Collections.Generic;
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

            builder.RemoveItemByName(ItemNames.Mothwing_Cloak);
            builder.RemoveItemByName(ItemNames.Left_Mothwing_Cloak);
            builder.RemoveItemByName(ItemNames.Right_Mothwing_Cloak);

            if (ExtraRando.Instance.Settings.ScarceItemPool)
            {
                builder.AddItemByName(ItemManager.Left_Cloak);
                builder.AddItemByName(ItemManager.Right_Cloak);
                if (builder.gs.DuplicateItemSettings.MothwingCloak)
                {
                    builder.AddItemByName(ItemManager.Left_Cloak);
                    builder.AddItemByName(ItemManager.Right_Cloak);
                }
            }
            else
            {
                builder.AddItemByName(ItemManager.Progressive_Left_Cloak, 2);
                builder.AddItemByName(ItemManager.Progressive_Right_Cloak, 2);
                if (builder.gs.DuplicateItemSettings.ShadeCloak)
                {
                    builder.AddItemByName(ItemManager.Progressive_Left_Cloak, 2);
                    builder.AddItemByName(ItemManager.Progressive_Right_Cloak, 2);
                }
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
        if (ExtraRando.Instance.Settings.RandomizeColoAccess)
        {
            builder.AddItemByName(ItemManager.Colo_Ticket_Bronze);
            builder.AddItemByName(ItemManager.Colo_Ticket_Silver);
            builder.AddItemByName(ItemManager.Colo_Ticket_Gold);

            builder.EditItemRequest(ItemManager.Colo_Ticket_Bronze, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Colo_Ticket_Bronze,
                    Pool = "Key",
                    PriceCap = 333
                };
            });
            builder.EditItemRequest(ItemManager.Colo_Ticket_Silver, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Colo_Ticket_Silver,
                    Pool = "Key",
                    PriceCap = 666
                };
            });
            builder.EditItemRequest(ItemManager.Colo_Ticket_Gold, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Colo_Ticket_Gold,
                    Pool = "Key",
                    PriceCap = 1000
                };
            });
        }
        if (ExtraRando.Instance.Settings.RandomizePantheonAccess)
        {
            builder.AddItemByName(ItemManager.Pantheon_Access_Master);
            builder.AddItemByName(ItemManager.Pantheon_Access_Artist);
            builder.AddItemByName(ItemManager.Pantheon_Access_Sage);
            builder.AddItemByName(ItemManager.Pantheon_Access_Knight);
            builder.AddItemByName(ItemManager.Pantheon_Access_Hallownest);

            builder.EditItemRequest(ItemManager.Pantheon_Access_Master, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Pantheon_Access_Master,
                    Pool = "Key",
                    PriceCap = 100
                };
            });
            builder.EditItemRequest(ItemManager.Pantheon_Access_Artist, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Pantheon_Access_Artist,
                    Pool = "Key",
                    PriceCap = 200
                };
            });
            builder.EditItemRequest(ItemManager.Pantheon_Access_Sage, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Pantheon_Access_Sage,
                    Pool = "Key",
                    PriceCap = 300
                };
            });
            builder.EditItemRequest(ItemManager.Pantheon_Access_Knight, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Pantheon_Access_Knight,
                    Pool = "Key",
                    PriceCap = 400
                };
            });
            builder.EditItemRequest(ItemManager.Pantheon_Access_Hallownest, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    Name = ItemManager.Pantheon_Access_Hallownest,
                    Pool = "Key",
                    PriceCap = 500
                };
            });

            builder.AddLocationByName(ItemManager.Pantheon_Master);
            builder.AddLocationByName(ItemManager.Pantheon_Artist);
            builder.AddLocationByName(ItemManager.Pantheon_Sage);
            builder.AddLocationByName(ItemManager.Pantheon_Knight);
            builder.AddLocationByName(ItemManager.Pantheon_Hallownest);
        }
        if (ExtraRando.Instance.Settings.RandomizeMarkers)
        {
            int totalItems = 0;
            List<ItemGroupBuilder> availablePools = new();
            foreach (StageBuilder stage in builder.Stages)
                foreach (ItemGroupBuilder itemGroup in stage.Groups.Where(x => x is ItemGroupBuilder).Select(x => x as ItemGroupBuilder))
                {
                    if (!availablePools.Contains(itemGroup))
                        availablePools.Add(itemGroup);
                    totalItems += itemGroup.Items.EnumerateWithMultiplicity().Count();
                }
            builder.AddItemByName(ItemManager.Scarab_Marker_Hint, 1 + totalItems / 100);
            builder.AddItemByName(ItemManager.Shell_Marker_Hint, 1 + totalItems / 100);
            builder.AddItemByName(ItemManager.Gleaming_Marker_Hint, 1 + totalItems / 100);
            builder.AddItemByName(ItemManager.Token_Marker_Hint, 1 + totalItems / 100);

            builder.EditItemRequest(ItemManager.Scarab_Marker_Hint, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    PriceCap = 50,
                    Name = ItemManager.Scarab_Marker_Hint,
                    Pool = "Map"
                };
            });
            builder.EditItemRequest(ItemManager.Shell_Marker_Hint, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    PriceCap = 50,
                    Name = ItemManager.Shell_Marker_Hint,
                    Pool = "Map"
                };
            });
            builder.EditItemRequest(ItemManager.Gleaming_Marker_Hint, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    PriceCap = 50,
                    Name = ItemManager.Gleaming_Marker_Hint,
                    Pool = "Map"
                };
            });
            builder.EditItemRequest(ItemManager.Token_Marker_Hint, info =>
            {
                info.getItemDef = () => new()
                {
                    MajorItem = false,
                    PriceCap = 50,
                    Name = ItemManager.Token_Marker_Hint,
                    Pool = "Map"
                };
            });
        }

    }

    private static void ApplyScarceItemPool(RequestBuilder builder)
    {
        if (!ExtraRando.Instance.Settings.Enabled)
            return;

        if (ExtraRando.Instance.Settings.ScarceItemPool)
        {
            List<ItemGroupBuilder> availablePools = new();
            foreach (StageBuilder stage in builder.Stages)
                foreach (ItemGroupBuilder itemGroup in stage.Groups.Where(x => x is ItemGroupBuilder).Select(x => x as ItemGroupBuilder))
                    if (!availablePools.Contains(itemGroup))
                        availablePools.Add(itemGroup);
            if (builder.gs.PoolSettings.Skills)
            {
                if (!builder.gs.CursedSettings.RemoveSpellUpgrades && !builder.gs.CursedSettings.ReplaceJunkWithOneGeo)
                {
                    builder.RemoveItemByName(ItemNames.Vengeful_Spirit);
                    builder.RemoveItemByName(ItemNames.Howling_Wraiths);
                    builder.RemoveItemByName(ItemNames.Desolate_Dive);
                    builder.RemoveItemByName(ItemNames.Abyss_Shriek);
                    builder.RemoveItemByName(ItemNames.Descending_Dark);
                    builder.RemoveItemByName(ItemNames.Shade_Soul);
                    builder.AddItemByName(ItemManager.Scream_Spell);
                    builder.AddItemByName(ItemManager.Fireball_Spell);
                    builder.AddItemByName(ItemManager.Dive_Spell);
                }

                if (!ExtraRando.Instance.Settings.SplitShadeCloak)
                    if (builder.gs.NoveltySettings.SplitCloak)
                    {
                        builder.RemoveItemByName(ItemNames.Left_Mothwing_Cloak);
                        builder.RemoveItemByName(ItemNames.Right_Mothwing_Cloak);
                        // Not really sure how the naming is handled so we just try to remove both
                        builder.RemoveItemByName(ItemNames.Split_Shade_Cloak);
                        builder.RemoveItemByName(ItemNames.Shade_Cloak);

                        // We can just utilize the packed split shade cloak items since the user will not be able to dash in the opposite direction anyway.
                        builder.AddItemByName(ItemManager.Left_Cloak);
                        builder.AddItemByName(ItemManager.Right_Cloak);
                        if (builder.gs.DuplicateItemSettings.MothwingCloak)
                        {
                            builder.AddItemByName(ItemManager.Left_Cloak);
                            builder.AddItemByName(ItemManager.Right_Cloak);
                        }
                    }
                    else
                    {
                        builder.RemoveItemByName(ItemNames.Mothwing_Cloak);
                        builder.RemoveItemByName(ItemNames.Shade_Cloak);

                        builder.AddItemByName(ItemManager.Cloak);
                        if (builder.gs.DuplicateItemSettings.MothwingCloak)
                            builder.AddItemByName(ItemManager.Cloak);
                    }
            }
            List<string> itemsToRemove = new();

            if (!builder.gs.CursedSettings.ReplaceJunkWithOneGeo)
            {
                if (builder.gs.PoolSettings.MaskShards)
                    switch (builder.gs.MiscSettings.MaskShards)
                    {
                        case MiscSettings.MaskShardType.FourShardsPerMask:
                            itemsToRemove.AddRange(Enumerable.Range(0, 2).Select(x => ItemNames.Full_Mask));
                            break;
                        case MiscSettings.MaskShardType.TwoShardsPerMask:
                            itemsToRemove.AddRange(Enumerable.Range(0, 4).Select(x => ItemNames.Double_Mask_Shard));
                            break;
                        default:
                            itemsToRemove.AddRange(Enumerable.Range(0, 8).Select(x => ItemNames.Mask_Shard));
                            break;
                    }

                if (builder.gs.PoolSettings.VesselFragments)
                    switch (builder.gs.MiscSettings.VesselFragments)
                    {
                        case MiscSettings.VesselFragmentType.ThreeFragmentsPerVessel:
                            itemsToRemove.Add(ItemNames.Full_Soul_Vessel);
                            break;
                        case MiscSettings.VesselFragmentType.TwoFragmentsPerVessel:
                            itemsToRemove.Add(ItemNames.Vessel_Fragment);
                            itemsToRemove.Add(ItemNames.Double_Vessel_Fragment);
                            break;
                        default:
                            itemsToRemove.AddRange(Enumerable.Range(0, 8).Select(x => ItemNames.Vessel_Fragment));
                            break;
                    }

                if (builder.gs.PoolSettings.CharmNotches || builder.gs.MiscSettings.SalubraNotches == MiscSettings.SalubraNotchesSetting.Randomized)
                    itemsToRemove.Add(ItemNames.Charm_Notch);
            }
            // If nail upgrades via RandoPlus are added, we remove one as well.
            if (availablePools.Any(x => x.Items.EnumerateDistinct().Contains("Nail_Upgrade")))
                itemsToRemove.Add("Nail_Upgrade");
            if (availablePools.Any(x => x.Items.EnumerateDistinct().Contains("Geo_Wallet")))
                itemsToRemove.Add("Geo_Wallet");
            if (builder.gs.PoolSettings.Keys)
            {
                builder.RemoveItemByName(ItemNames.Simple_Key);
                builder.AddItemByName(ItemManager.Key_Ring);
                builder.EditItemRequest(ItemManager.Key_Ring, info =>
                {
                    info.getItemDef = () => new()
                    {
                        MajorItem = true,
                        PriceCap = 500,
                        Name = ItemManager.Key_Ring,
                        Pool = "Key"
                    };
                });
            }

            while (itemsToRemove.Any())
            {
                ItemGroupBuilder selectedBuilder = availablePools.FirstOrDefault(x => x.Items.EnumerateWithMultiplicity().Contains(itemsToRemove[0]));
                selectedBuilder?.Items.Remove(itemsToRemove[0], 1);
                itemsToRemove.RemoveAt(0);
            }
        }
    }

    private static void ModifyLogic(GenerationSettings settings, LogicManagerBuilder builder)
    {
        if (!ExtraRando.Instance.Settings.Enabled)
            return;

        using Stream macroFile = ResourceHelper.LoadResource<ExtraRando>("Randomizer.Macros.json");
        builder.DeserializeJson(LogicManagerBuilder.JsonType.Macros, macroFile);
        using Stream waypointStream = ResourceHelper.LoadResource<ExtraRando>("Randomizer.Waypoints.json");
        builder.DeserializeJson(LogicManagerBuilder.JsonType.Waypoints, waypointStream);
        using Stream logicFile = ResourceHelper.LoadResource<ExtraRando>("Randomizer.Logic.json");
        builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, logicFile);

        if (ExtraRando.Instance.Settings.ScarceItemPool)
        {
            builder.AddItem(new MultiItem(ItemManager.Cloak, new RandomizerCore.TermValue[]
            {
                new(builder.GetTerm("LEFTDASH"), 2),
                new(builder.GetTerm("RIGHTDASH"), 2)
            }));
            builder.AddItem(new SingleItem(ItemManager.Left_Cloak, new(builder.GetTerm("LEFTDASH"), 2)));
            builder.AddItem(new SingleItem(ItemManager.Right_Cloak, new(builder.GetTerm("RIGHTDASH"), 2)));

            builder.AddItem(new MultiItem(ItemManager.Fireball_Spell, new RandomizerCore.TermValue[]
            {
                new(builder.GetTerm("FIREBALL"), 2),
                new(builder.GetTerm("SPELLS"), 2)

            }));
            builder.AddItem(new MultiItem(ItemManager.Dive_Spell, new RandomizerCore.TermValue[]
            {
                new(builder.GetTerm("QUAKE"), 2),
                new(builder.GetTerm("SPELLS"), 2)

            }));
            builder.AddItem(new MultiItem(ItemManager.Scream_Spell, new RandomizerCore.TermValue[]
            {
                new(builder.GetTerm("SCREAM"), 2),
                new(builder.GetTerm("SPELLS"), 2)

            }));
            builder.AddItem(new SingleItem(ItemManager.Key_Ring, new(builder.GetTerm("SIMPLE"), 4)));
        }
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
            builder.AddItem(new EmptyItem(ItemManager.Hot_Spring_Water));
        if (ExtraRando.Instance.Settings.RandomizeMarkers)
        {
            builder.AddItem(new EmptyItem(ItemManager.Gleaming_Marker_Hint));
            builder.AddItem(new EmptyItem(ItemManager.Scarab_Marker_Hint));
            builder.AddItem(new EmptyItem(ItemManager.Shell_Marker_Hint));
            builder.AddItem(new EmptyItem(ItemManager.Token_Marker_Hint));
        }
        if (ExtraRando.Instance.Settings.RandomizeColoAccess)
        {
            Term coloTerm = builder.GetOrAddTerm("COLO_KEY_1");
            builder.AddItem(new SingleItem(ItemManager.Colo_Ticket_Bronze, new(coloTerm, 1)));

            coloTerm = builder.GetOrAddTerm("COLO_KEY_2");
            builder.AddItem(new SingleItem(ItemManager.Colo_Ticket_Silver, new(coloTerm, 1)));

            coloTerm = builder.GetOrAddTerm("COLO_KEY_3");
            builder.AddItem(new SingleItem(ItemManager.Colo_Ticket_Gold, new(coloTerm, 1)));

            builder.DoLogicEdit(new("Defeated_Colosseum_1", "(ORIG) + COLO_KEY_1"));
            builder.DoLogicEdit(new("Defeated_Colosseum_2", "(ORIG) + COLO_KEY_2"));

            if (builder.LogicLookup.ContainsKey("The_Glory_of_Being_a_Fool-Colosseum"))
                builder.DoLogicEdit(new("The_Glory_of_Being_a_Fool-Colosseum", "Room_Colosseum_01[left1] + Can_Replenish_Geo + (ANYCLAW | (SPICYCOMBATSKIPS + WINGS)) + COMBAT[Colosseum_2] + COLO_KEY_3 + (READ ? TRUE)"));
        }
        if (ExtraRando.Instance.Settings.RandomizePantheonAccess)
        {
            Term pantheonTerm = builder.GetOrAddTerm("PANTHEON_KEY_1");
            builder.AddItem(new SingleItem(ItemManager.Pantheon_Access_Master, new(pantheonTerm, 1)));

            pantheonTerm = builder.GetOrAddTerm("PANTHEON_KEY_2");
            builder.AddItem(new SingleItem(ItemManager.Pantheon_Access_Artist, new(pantheonTerm, 1)));

            pantheonTerm = builder.GetOrAddTerm("PANTHEON_KEY_3");
            builder.AddItem(new SingleItem(ItemManager.Pantheon_Access_Sage, new(pantheonTerm, 1)));

            pantheonTerm = builder.GetOrAddTerm("PANTHEON_KEY_4");
            builder.AddItem(new SingleItem(ItemManager.Pantheon_Access_Knight, new(pantheonTerm, 1)));

            pantheonTerm = builder.GetOrAddTerm("PANTHEON_KEY_5");
            builder.AddItem(new SingleItem(ItemManager.Pantheon_Access_Hallownest, new(pantheonTerm, 1)));
        }
    }

    private static void CheckForNoLogic(GenerationSettings settings, LogicManagerBuilder builder)
    {
        if (!ExtraRando.Instance.Settings.Enabled || !ExtraRando.Instance.Settings.NoLogic)
            return;
        foreach (string key in builder.LogicLookup.Keys.ToList())
            builder.DoLogicEdit(new(key, "TRUE"));
    }

    private static void WriteSettings(LogArguments arg1, TextWriter textWriter)
    {
        textWriter.WriteLine("ExtraRando settings");
        using Newtonsoft.Json.JsonTextWriter jsonTextWriter = new(textWriter) { CloseOutput = false, };
        JsonUtil._js.Serialize(jsonTextWriter, ExtraRando.Instance.Settings);
        textWriter.WriteLine();
    }

    private static int RandoController_OnCalculateHash(RandoController arg1, int arg2)
    {
        if (!ExtraRando.Instance.Settings.Enabled)
            return 0;
        return !ExtraRando.Instance.Settings.NoLogic ? 1 : 24;
    }

    #endregion

    #region Methods

    internal static void Initialize()
    {
        RandoMenu.Initialize();
        RequestBuilder.OnUpdate.Subscribe(float.MaxValue - 1f, ApplyScarceItemPool);
        RequestBuilder.OnUpdate.Subscribe(1050f, ApplySettings);
        RCData.RuntimeLogicOverride.Subscribe(1050f, ModifyLogic);
        RCData.RuntimeLogicOverride.Subscribe(float.MaxValue, CheckForNoLogic);

        SettingsLog.AfterLogSettings += WriteSettings;
        RandoController.OnCalculateHash += RandoController_OnCalculateHash;

        if (ModHooks.GetMod("RandoSettingsManager") is Mod)
            HookRandoSettingsManager();
    }

    private static void HookRandoSettingsManager()
    {
        RandoSettingsManagerMod.Instance.RegisterConnection(new SimpleSettingsProxy<RandoSettings>(ExtraRando.Instance,
        RandoMenu.Instance.PassSettings,
        () => ExtraRando.Instance.Settings.Enabled ? ExtraRando.Instance.Settings : null));
    }

    #endregion
}
