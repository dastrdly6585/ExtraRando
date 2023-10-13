using ItemChanger;
using ItemChanger.Locations;
using KorzUtils.Helper;
using UnityEngine;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class GoldenGreyPrinceLocation : PlaceableLocation
{
    #region Eventhandler

    private void IntCompare_OnEnter(On.HutongGames.PlayMaker.Actions.IntCompare.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntCompare self)
    {
        if (self.IsCorrectContext("FSM", "Mighty_Zote_gold", "Check") && !Placement.AllObtained() && self.integer1.Value >= 10)
        {
            GetContainer(out GameObject obj, out string containerType);
            PlaceContainer(obj, containerType);
        }
        orig(self);
    }

    #endregion

    #region Methods

    public override void PlaceContainer(GameObject containerObject, string containerType)
    {
        Container.GetContainer(containerType)!.ApplyTargetContext(containerObject, 12.92f, 7.4f, 0f);
        if (!containerObject.activeSelf)
            containerObject.SetActive(true);
    }

    protected override void OnLoad() => On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += IntCompare_OnEnter;

    protected override void OnUnload() => On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter -= IntCompare_OnEnter; 

    #endregion
}
