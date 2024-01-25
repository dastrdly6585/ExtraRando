using ItemChanger;
using ItemChanger.Locations;
using KorzUtils.Helper;
using Modding;
using MonoMod.Cil;
using System;
using System.Linq;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class PantheonDoorLocation : AutoLocation
{
    #region Properties

    public int PantheonNumber { get; set; }

    #endregion

    #region Eventhandler

    private System.Collections.IEnumerator BossSequenceDoor_DoorUnlockSequence(On.BossSequenceDoor.orig_DoorUnlockSequence orig, BossSequenceDoor self)
    {
        yield return orig(self);
        if (self.playerDataString?.EndsWith(PantheonNumber.ToString()) == true)
        {
            if (!Placement.AllObtained())
            {
                HeroController.instance.RelinquishControl();
                Placement.GiveAll(new()
                {
                    FlingType = FlingType.DirectDeposit,
                    MessageType = MessageType.Any
                }, HeroController.instance.RegainControl);
            }
            bool unlocked = CheckIfDoorUnlocked(self.playerDataString);
            self.unlockedSet.SetActive(unlocked);
            self.lockSet.SetActive(!unlocked);
        }
    }

    private void BossSequenceDoor_Start(On.BossSequenceDoor.orig_Start orig, BossSequenceDoor self)
    {
        orig(self);
        if (self.playerDataString.EndsWith(PantheonNumber.ToString()))
        {
            if (!Placement.AllObtained())
            {
                if (CheckIfDoorUnlocked(self.playerDataString) && (Placement.Items.All(x => x.WasEverObtained())
                    || CheckIfPantheonAccessible(self)))
                    ItemHelper.SpawnShiny(self.transform.position - new UnityEngine.Vector3(5f, 0f, 3f), Placement);
            }
            else
            {
                self.doLockBreakSequence = false;
                ReflectionHelper.SetField(self, "doUnlockSequence", false);
            }
        }
    }

    #endregion

    protected override void OnLoad()
    {
        On.BossSequenceDoor.DoorUnlockSequence += BossSequenceDoor_DoorUnlockSequence;
        On.BossSequenceDoor.Start += BossSequenceDoor_Start;
        IL.BossSequenceDoor.OnTriggerEnter2D += BossSequenceDoor_OnTriggerEnter2D;
    }

    private void BossSequenceDoor_OnTriggerEnter2D(ILContext il)
    {
        ILCursor cursor = new(il);
        cursor.Goto(0);
        if (cursor.TryGotoNext(MoveType.After,
            x => x.MatchLdflda<BossSequenceDoor>("completion"),
            x => x.MatchLdcI4(1)))
        {
            cursor.EmitDelegate<Func<int, int>>(x =>
            {
                BossSequenceDoor.Completion completion = PlayerData.instance.GetVariable<BossSequenceDoor.Completion>("bossDoorStateTier" + PantheonNumber);
                return completion.unlocked ? 1 : 0;
            });
        }
        else
            LogHelper.Write<ExtraRando>("Failed to find match for BossSequenceDoor_OnTriggerEnter2D", KorzUtils.Enums.LogType.Error);
    }

    protected override void OnUnload()
    {
        On.BossSequenceDoor.DoorUnlockSequence -= BossSequenceDoor_DoorUnlockSequence;
        On.BossSequenceDoor.Start -= BossSequenceDoor_Start;
        IL.BossSequenceDoor.OnTriggerEnter2D -= BossSequenceDoor_OnTriggerEnter2D;
    }

    private bool CheckIfDoorUnlocked(string playerDataName)
    {
        if (string.IsNullOrEmpty(playerDataName))
            return false;
        return PlayerData.instance.GetVariable<BossSequenceDoor.Completion>(playerDataName).unlocked;
    }

    private bool CheckIfPantheonAccessible(BossSequenceDoor bossSequenceDoor)
     => bossSequenceDoor.bossSequence?.IsUnlocked() ?? false && (PantheonNumber < 4
            // Panth 4
            || (PantheonNumber == 4 && PlayerData.instance.GetVariable<BossSequenceDoor.Completion>("bossDoorStateTier1").completed
                && PlayerData.instance.GetVariable<BossSequenceDoor.Completion>("bossDoorStateTier2").completed && PlayerData.instance.GetVariable<BossSequenceDoor.Completion>("bossDoorStateTier3").completed)
            // Panth 5
            || (PantheonNumber == 5 && PDHelper.RoyalCharmState == 4));

}
