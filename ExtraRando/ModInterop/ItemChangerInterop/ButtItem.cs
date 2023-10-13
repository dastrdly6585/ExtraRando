using ItemChanger;
using ItemChanger.Internal;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class ButtItem : AbstractItem
{
    #region Methods

    public override void GiveImmediate(GiveInfo info)
    {
        SoundManager manager = new(typeof(ExtraRando).Assembly, "ExtraRando.Resources.Sounds.");
        manager.PlayClipAtPoint("Smack_"+UnityEngine.Random.Range(1, 5), HeroController.instance.transform.position);
    }

    #endregion
}
