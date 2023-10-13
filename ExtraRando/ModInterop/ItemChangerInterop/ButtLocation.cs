using ItemChanger;
using ItemChanger.Locations;
using KorzUtils.Data;
using KorzUtils.Helper;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class ButtLocation : AutoLocation
{
    #region Eventhandler

    private void ModifyButt(PlayMakerFSM fsm)
    {
        fsm.AddState("Give Items", () =>
        {
            if (Placement.AllObtained())
                return;
            Placement.GiveAll(new()
            {
                MessageType = MessageType.Corner,
                Transform = fsm.transform,
                Container = "Butt",
                FlingType = FlingType.DirectDeposit
            });
        }, FsmTransitionData.FromTargetState("Idle").WithEventName("FINISHED"));
        fsm.GetState("Hit").AdjustTransition("FINISHED", "Give Items");
    }

    #endregion

    #region Methods

    protected override void OnLoad() => Events.AddFsmEdit(new("Big Caterpillar Tail", "Control"), ModifyButt);
    
    protected override void OnUnload() => Events.RemoveFsmEdit(new("Big Caterpillar Tail", "Control"), ModifyButt);
    
    #endregion
}
