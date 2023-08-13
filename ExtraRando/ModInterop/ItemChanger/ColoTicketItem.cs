using HutongGames.PlayMaker.Actions;
using ItemChanger;
using KorzUtils.Data;
using KorzUtils.Helper;
using IC = ItemChanger;

namespace ExtraRando.ModInterop.ItemChanger;

public class ColoTicketItem : AbstractItem
{
    #region Properties

    public string Trial { get; set; }

    #endregion

    #region Eventhandler

    private void BlockColoAccess(PlayMakerFSM self)
    {
        try
        {
            CallMethodProper actionReference = self.GetState("Unpaid").GetFirstAction<CallMethodProper>();
            self.AddState("Unworthy", () => actionReference.gameObject.GameObject.Value.GetComponent<DialogueBox>()
                .StartConversation($"Unworthy ({Trial})", "Minor_NPC"),
                FsmTransitionData.FromTargetState("Anim End").WithEventName("CONVO_FINISH"));

            self.AddState("Box Up", self.GetState("Box Up 2").Actions,
                FsmTransitionData.FromTargetState("Unworthy").WithEventName("FINISHED"));

            self.AddState("Has Ticket?", () => self.SendEvent(IsObtained() ? "OPEN" : "FINISHED"),
                FsmTransitionData.FromTargetState("Box Up YN").WithEventName("OPEN"),
                FsmTransitionData.FromTargetState("Box Up").WithEventName("FINISHED"));

            // Set up transition
            self.GetState("State Check").AdjustTransition("OPEN", "Has Ticket?");
        }
        catch (System.Exception exception)
        {
            LogHelper.Write<ExtraRando>("Couldn't modify colo trial board ", exception);
        }
    }

    private void ShowColoPreview(ref string value)
    {
        if (Trial == "Bronze")
        {
            if (IC.Internal.Ref.Settings.Placements.ContainsKey(LocationNames.Charm_Notch_Colosseum)
                && IC.Internal.Ref.Settings.Placements[LocationNames.Charm_Notch_Colosseum].Items.Count > 0)
                value = "You are not worthy to enter the arena yet. If you want " +
                    IC.Internal.Ref.Settings.Placements[LocationNames.Charm_Notch_Colosseum].Items[0].GetPreviewName(IC.Internal.Ref.Settings.Placements[LocationNames.Charm_Notch_Colosseum])
                    + " you have to be a little... richer. (Translation: Only fools with a ticket have access, so get lost.)";
            else
                value = "You are not worthy to enter the arena yet.";
        }
        else if (Trial == "Silver")
        {
            if (IC.Internal.Ref.Settings.Placements.ContainsKey(LocationNames.Pale_Ore_Colosseum)
                && IC.Internal.Ref.Settings.Placements[LocationNames.Pale_Ore_Colosseum].Items.Count > 0)
                value = "You are not worthy to enter the arena yet. If you want " +
                    IC.Internal.Ref.Settings.Placements[LocationNames.Pale_Ore_Colosseum].Items[0].GetPreviewName(IC.Internal.Ref.Settings.Placements[LocationNames.Pale_Ore_Colosseum])
                    + " you have to be a little... richer. (Translation: Only fools with a ticket have access, so get lost.)";
            else
                value = "You are not worthy to enter the arena yet.";
        }
        else
        {
            if (IC.Internal.Ref.Settings.Placements.ContainsKey("The_Glory_of_Being_a_Fool-Colosseum")
                && IC.Internal.Ref.Settings.Placements["The_Glory_of_Being_a_Fool-Colosseum"].Items.Count > 0)
                value = "You are not worthy to enter the arena yet. If you want " +
                        IC.Internal.Ref.Settings.Placements["The_Glory_of_Being_a_Fool-Colosseum"].Items[0].GetPreviewName(IC.Internal.Ref.Settings.Placements["The_Glory_of_Being_a_Fool-Colosseum"])
                        + " you have to be a little... richer. (Translation: Only fools with a ticket have access, so get lost.)";
            else
                value = "You are not worthy to enter the arena yet. Come back when you are a little... richer. (Translation: Only fools with a ticket have access, so get lost.)";
        }
    }

    #endregion

    #region Methods

    protected override void OnLoad()
    {
        Events.AddFsmEdit(new(Trial + " Trial Board", "Conversation Control"), BlockColoAccess);
        Events.AddLanguageEdit(new LanguageKey("Minor_NPC", $"Unworthy ({Trial})"), ShowColoPreview);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(new(Trial + " Trial Board", "Conversation Control"), BlockColoAccess);
        Events.RemoveLanguageEdit(new LanguageKey("Minor_NPC", $"Unworthy ({Trial})"), ShowColoPreview);
    }

    public override void GiveImmediate(GiveInfo info) { }

    #endregion
}
