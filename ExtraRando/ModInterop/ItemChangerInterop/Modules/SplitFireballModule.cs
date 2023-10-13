using ItemChanger.Modules;
using KorzUtils.Helper;
using Modding;

namespace ExtraRando.ModInterop.ItemChangerInterop.Modules;

/// <summary>
/// Controls whether fireball can be casted depending on the direction.
/// </summary>
public class SplitFireballModule : Module
{
    #region Properties

    public int LeftFireballLevel { get; set; }

    public int RightFireballLevel { get; set; }

    #endregion

    #region Eventhandler

    private string ModHooks_LanguageGetHook(string key, string sheetTitle, string orig)
    {
        if (key == "INV_DESC_SPELL_FIREBALL")
            orig += $"<br/>{LeftFireballLevel switch
            {
                0 => "You cannot cast this while facing left.",
                1 => "You've gathered enough power to cast vengeful spirit while facing right.",
                _ => "You've gathered enough power to cast shade soul while facing left.",
            }}<br/>{RightFireballLevel switch
            {
                0 => "You cannot cast this while facing right.",
                1 => "You've gathered enough power to cast vengeful spirit while facing right.",
                _ => "You've gathered enough power to cast shade soul while facing right.",
            }}";
        return orig;
    }

    private void IntCompare_OnEnter(On.HutongGames.PlayMaker.Actions.IntCompare.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntCompare self)
    {
        if (self.IsCorrectContext("Spell Control", null, "Has Fireball?"))
            self.integer1.Value = HeroController.instance.cState.facingRight ? RightFireballLevel : LeftFireballLevel;
        orig(self);
    }

    #endregion

    #region Methods

    public override void Initialize()
    {
        On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += IntCompare_OnEnter;
        ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
    }

    public override void Unload()
    {
        On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter -= IntCompare_OnEnter;
        ModHooks.LanguageGetHook -= ModHooks_LanguageGetHook;
    }

    #endregion
}
