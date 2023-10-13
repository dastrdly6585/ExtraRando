using ItemChanger;
using KorzUtils.Helper;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class PantheonEntranceItem : AbstractItem
{
    #region Properties

    public string FieldName { get; set; }

    #endregion

    #region Methods

    public override void GiveImmediate(GiveInfo info)
    {
        BossSequenceDoor.Completion doorCompletion = PlayerData.instance.GetVariable<BossSequenceDoor.Completion>(FieldName);
        doorCompletion.unlocked = true;
        PlayerData.instance.SetVariable(FieldName, doorCompletion);
        if (FieldName.EndsWith("4"))
            PDHelper.BossDoorCageUnlocked = true;
        else if (FieldName.EndsWith("5"))
            PDHelper.FinalBossDoorUnlocked = true;
    }

    #endregion
}
