using ItemChanger;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using KorzUtils.Helper;
using UnityEngine;

namespace ExtraRando.ModInterop.ItemChanger;

public class HotspringLocation : AutoLocation
{
    // Crossroads 30 Spa Region
    // Ruins_Bathhouse Spa Region
    // Room_Colosseum_02 Spa Region (1)
    // Deepnest_30
    // GG_Atrium Spa Region
    // GG_Atrium_Roof Spa Region

    #region Event handler

    private void ControlHotSpring(PlayMakerFSM fsm)
    {
        if (!Placement.AllObtained())
        {
            fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
            {
                Name = "Check for item",
                Actions = new HutongGames.PlayMaker.FsmStateAction[]
                {
                    new Lambda(() =>
                    {
                        if (!Placement.AllObtained())
                            Placement.GiveAll(new()
                            {
                                MessageType = MessageType.Corner,
                            });
                    })
                }
            });
            fsm.GetState("Stay").AdjustTransition("WAIT", "Check for item");
            fsm.GetState("Check for item").AddTransition("FINISHED", "Leave");
        }
        else
        {
            fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
            {
                Name = "Destroy",
                Actions = new HutongGames.PlayMaker.FsmStateAction[]
                {
                    new Lambda(() => GameObject.Destroy(fsm.gameObject))
                }
            });
            fsm.GetState("Init").AdjustTransition("FINISHED", "Destroy");
        }
    }

    #endregion

    #region Methods

    protected override void OnLoad()
    {
        Events.AddFsmEdit(sceneName, new FsmID("Spa Region"), ControlHotSpring);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(sceneName, new FsmID("Spa Region"), ControlHotSpring);
    } 

    #endregion
}
