using ExtraRando.ModInterop.ItemChangerInterop.Modules;
using ItemChanger;
using ItemChanger.Modules;
using ItemChanger.UIDefs;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class SplitDashItem : AbstractItem
{
    #region Properties

    public int Stages { get; set; }

    public bool IsLeft { get; set; }

    #endregion

    public override void GiveImmediate(GiveInfo info)
    {
        if (IsLeft)
        {
            if (!PlayerData.instance.GetBool("canDashLeft") && Stages == 1)
            {
                UIDef = Finder.GetItem(ItemNames.Left_Mothwing_Cloak)!.UIDef;
                PlayerData.instance.SetBool("canDashLeft", true);
            }
            else
            {
                UIDef = Finder.GetItem(ItemManager.Progressive_Left_Cloak)!.UIDef;
                if (!PlayerData.instance.GetBool("canDashLeft"))
                    PlayerData.instance.SetBool("canDashLeft", true);
                PlayerData.instance.SetBool("leftShadeDash", true);
            }
        }
        else
        {
            if (!PlayerData.instance.GetBool("canDashRight") && Stages == 1)
            {
                UIDef = Finder.GetItem(ItemNames.Right_Mothwing_Cloak)!.UIDef;
                PlayerData.instance.SetBool("canDashRight", true);
            }
            else
            {
                UIDef = Finder.GetItem(ItemManager.Progressive_Right_Cloak)!.UIDef;
                if (!PlayerData.instance.GetBool("canDashRight"))
                    PlayerData.instance.SetBool("canDashRight", true);
                PlayerData.instance.SetBool("rightShadeDash", true);
            }
        }
        if (Stages == 2)
            (UIDef as BigUIDef)!.sprite = new WrappedSprite("Dash");
    }

    protected override void OnLoad()
    {
        ItemChangerMod.Modules.GetOrAdd(typeof(SplitCloak));
        ItemChangerMod.Modules.GetOrAdd(typeof(SplitShadeCloakModule));
    }

    public override bool Redundant() => PlayerData.instance.GetBool(IsLeft ? "leftShadeDash" : "rightShadeDash");
}
