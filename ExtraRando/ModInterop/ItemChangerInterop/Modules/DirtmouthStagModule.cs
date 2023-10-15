using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Modules;
using KorzUtils.Data;
using KorzUtils.Helper;
using Modding;
using UnityEngine;

namespace ExtraRando.ModInterop.ItemChangerInterop.Modules;

public class DirtmouthStagModule : Module
{
    #region Properties

    public bool HasDirtmouthKey { get; set; }

    #endregion

    #region Eventhandler

    private void ModifyOutsideDoor(PlayMakerFSM fsm)
    {
        if (fsm.GetState("Init")?.GetFirstAction<PlayerDataBoolTest>()?.boolName.Value == "openedTownBuilding" != true)
            return;
        fsm.AddState("Stag Door Control", () =>
        {
            bool shouldDoorOpen = HasDirtmouthKey && PDHelper.OpenedTownBuilding;
            fsm.FsmVariables.FindFsmGameObject(shouldDoorOpen ? "closed" :"open").Value.SetActive(false);
        }, FsmTransitionData.FromTargetState("Opened").WithEventName("OPENED"));
        fsm.GetState("Init").AdjustTransition(FsmTransitionData.FromTargetState("Stag Door Control").WithEventName("OPENED"));
        fsm.GetState("Opened").AddActions(() => fsm.FsmVariables.FindFsmGameObject("open").Value.SetActive(true));
    }

    private void ModifyInsideDoor(PlayMakerFSM fsm)
    {
        fsm.AddState("Shadow Realm", () => { }, null);
        fsm.AddState("Control Door", () =>
        {
            if (HasDirtmouthKey && PDHelper.OpenedTownBuilding)
                fsm.SendEvent("OPENED");
            else if (!HasDirtmouthKey)
                fsm.SendEvent("BANISH");
        }, FsmTransitionData.FromTargetState("Shadow Realm").WithEventName("BANISH"), FsmTransitionData.FromTargetState("Opened").WithEventName("OPENED"),
        FsmTransitionData.FromTargetState("Activate Pause").WithEventName("ACTIVATE"));

        fsm.GetState("Init").AdjustTransitions("Control Door");
    }

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        try
        {
            if (name == "Dirtmouth_Stag_Key")
            {
                HasDirtmouthKey = true;
                if (PDHelper.OpenedTownBuilding)
                    GameObject.Find("Stag_station").LocateMyFSM("Check Opened").SendEvent("OPENED");
            }
        }
        catch (System.Exception exception)
        {
            LogHelper.Write("An error occured while trying to setup dirtmouth stag", exception);
        }
        return orig;
    }

    private bool ModHooks_GetPlayerBoolHook(string name, bool orig) => name == nameof(PlayerData.openedCrossroads)
            ? orig && HasDirtmouthKey
            : orig;

    #endregion

    public override void Initialize()
    {
        Events.AddFsmEdit(new("Stag_station", "Check Opened"), ModifyOutsideDoor);
        Events.AddFsmEdit(new("Station Door", "Control"), ModifyInsideDoor);
        ModHooks.SetPlayerBoolHook += ModHooks_SetPlayerBoolHook;
        ModHooks.GetPlayerBoolHook += ModHooks_GetPlayerBoolHook;
    }

    public override void Unload()
    {
        Events.RemoveFsmEdit(new("Stag_station", "Check Opened"), ModifyOutsideDoor);
        ModHooks.SetPlayerBoolHook -= ModHooks_SetPlayerBoolHook;
        ModHooks.GetPlayerBoolHook -= ModHooks_GetPlayerBoolHook;
        Events.RemoveFsmEdit(new("Station Door", "Control"), ModifyInsideDoor);
    }
}
