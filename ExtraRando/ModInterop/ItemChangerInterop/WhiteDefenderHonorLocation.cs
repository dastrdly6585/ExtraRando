using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Data;
using KorzUtils.Helper;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class WhiteDefenderHonorLocation : AutoLocation
{
    #region Eventhandler

    private void ModifyFsm(PlayMakerFSM fsm)
    {
        if (fsm.transform.parent?.name != "Dung Defender_Awake")
            return;
        fsm.AddState("Give Item", new List<FsmStateAction>() { 
            new Lambda(() =>
            {
                if (Placement.AllObtained())
                    fsm.SendEvent("CONVO_FINISH");
            }),
            new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new()
            {
                FlingType = FlingType.DirectDeposit,
                MessageType = MessageType.Any
            }, callback), "CONVO_FINISH") }, FsmTransitionData.FromTargetState("End").WithEventName("CONVO_FINISH"));
        fsm.GetState("Box Down").AdjustTransitions("Give Item");
    }

    public void PlaceReoccuringItems(Scene fsm)
    {
        if (!Placement.AllObtained() && PDHelper.DungDefenderLeft)
            ItemHelper.SpawnShiny(new(18.51f, 4.41f), Placement);
    }

    #endregion

    #region Methods

    protected override void OnLoad()
    {
        Events.AddFsmEdit(new("Head", "Conversation Control"), ModifyFsm);
        Events.AddSceneChangeEdit("Waterways_15", PlaceReoccuringItems);
        On.DeactivateIfPlayerdataFalse.OnEnable += DeactivateIfPlayerdataFalse_OnEnable;
    }

    private void DeactivateIfPlayerdataFalse_OnEnable(On.DeactivateIfPlayerdataFalse.orig_OnEnable orig, DeactivateIfPlayerdataFalse self)
    {
        // Let White Defender spawn at one dreamer.
        if (self.gameObject.name == "Dung Defender_Sleep" && PlayerData.instance.guardiansDefeated >= 1)
            return;
        orig(self);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(new("Head", "Conversation Control"), ModifyFsm);
        Events.RemoveSceneChangeEdit("Waterways_15", PlaceReoccuringItems);
        On.DeactivateIfPlayerdataFalse.OnEnable -= DeactivateIfPlayerdataFalse_OnEnable;
    }

    #endregion
}
