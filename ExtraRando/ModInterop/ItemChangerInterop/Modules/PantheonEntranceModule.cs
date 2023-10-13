using ItemChanger.Modules;
using KorzUtils.Helper;
using Modding;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtraRando.ModInterop.ItemChangerInterop.Modules;

//public class PantheonEntranceModule : Module
//{
//    #region Members

//    [NonSerialized]
//    private bool _isUnlocked;

//    #endregion

//    #region Properties

//    public List<bool> PantheonUnlocks { get; set; } = new() { true, true, true, true, true };

//    #endregion

//    #region Eventhandler

//    //private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
//    //{
//    //    if (name.StartsWith("pantheonDoor"))
//    //        PantheonUnlocks[Convert.ToInt16(name.Last().ToString()) - 1] = orig;
//    //    return orig;
//    //}

//    //private void On_BossSequenceDoor_Start(On.BossSequenceDoor.orig_Start orig, BossSequenceDoor self)
//    //{
//    //    if (!string.IsNullOrEmpty(self.playerDataString))
//    //        _isUnlocked = PantheonUnlocks[Convert.ToInt16(self.playerDataString.Last().ToString()) - 1];
//    //    orig(self);
//    //    _isUnlocked = false;
//    //}

//    //private void IL_BossSequenceDoor_Start(ILContext il)
//    //{
//    //    ILCursor cursor = new(il);
//    //    cursor.Goto(0);

//    //    if (cursor.TryGotoNext(MoveType.After,
//    //        x => x.MatchCallvirt<BossSequenceDoor>("IsUnlocked")))
//    //    {
//    //        cursor.EmitDelegate<Func<bool, bool>>(x => _isUnlocked);
//    //        // We will never allow the function to enter the else case to normally execute the door sequence, as we execute that ourself.
//    //        if (cursor.TryGotoNext(MoveType.After,
//    //            x => x.MatchLdfld<BossSequenceDoor.Completion>("unlocked")))
//    //            cursor.EmitDelegate<Func<bool, bool>>(x => _isUnlocked);
//    //        else
//    //            LogHelper.Write<ExtraRando>("Failed to modify BossSequenceDoor_Start 2", KorzUtils.Enums.LogType.Error);
//    //    }
//    //    else
//    //        LogHelper.Write<ExtraRando>("Failed to modify BossSequenceDoor_Start", KorzUtils.Enums.LogType.Error);
//    //}

//    //private void BossDoorCage_Start(On.BossDoorCage.orig_Start orig, BossDoorCage self)
//    //{
//    //    orig(self);
//    //    if (self.gameObject.scene.name == "GG_Atrium" && PantheonUnlocks[3])
//    //        self.gameObject.SetActive(false);
//    //}

//    #endregion

//    public override void Initialize()
//    {
//        ModHooks.SetPlayerBoolHook += ModHooks_SetPlayerBoolHook;
//        On.BossSequenceDoor.Start += On_BossSequenceDoor_Start;
//        IL.BossSequenceDoor.Start += IL_BossSequenceDoor_Start;
//        On.BossDoorCage.Start += BossDoorCage_Start;
//        ModHooks.GetPlayerVariableHook += ModHooks_GetPlayerVariableHook;
//    }

//    private object ModHooks_GetPlayerVariableHook(Type type, string name, object value)
//    {
//        if (name.StartsWith("bossDoorStateTier"))
//        {
//            int pantheonNumber = int.Parse(name.Last().ToString());
//            (value as BossSequenceDoor.Completion).unlocked = PantheonUnlocks
//        }
//        return value;
//    }

//    public override void Unload()
//    {
//        ModHooks.SetPlayerBoolHook -= ModHooks_SetPlayerBoolHook;
//        On.BossSequenceDoor.Start -= On_BossSequenceDoor_Start;
//        IL.BossSequenceDoor.Start -= IL_BossSequenceDoor_Start;
//        On.BossDoorCage.Start -= BossDoorCage_Start;
//    }
//}
