using ExtraRando.ModInterop.ItemChangerInterop.Modules;
using ItemChanger;
using ItemChanger.Items;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class DirtmouthStagItem : BoolItem
{
    protected override void OnLoad()
    {
        base.OnLoad();
        ItemChangerMod.Modules.GetOrAdd<DirtmouthStagModule>();
    }
}
