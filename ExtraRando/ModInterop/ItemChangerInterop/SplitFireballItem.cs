using ExtraRando.ModInterop.ItemChangerInterop.Modules;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.UIDefs;
using KorzUtils.Helper;
using System;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class SplitFireballItem : AbstractItem
{
    #region Properties

    /// <summary>
    /// Gets or sets the module that controls the fireball casts.
    /// </summary>
    public SplitFireballModule ControlModule { get; set; }

    /// <summary>
    /// Gets or sets the direction that this item should enable/upgrade.
    /// </summary>
    public bool IsLeft { get; set; }

    /// <summary>
    /// Flags the item for the scarced item setting to combine both vs and ss in one direction to a single item.
    /// </summary>
    public bool Merged { get; set; }

    #endregion

    /// <inheritdoc/>
    protected override void OnLoad() => ControlModule = ItemChangerMod.Modules.GetOrAdd<SplitFireballModule>();

    /// <inheritdoc/>
    public override void GiveImmediate(GiveInfo info)
    {
        BigUIDef itemScreen = UIDef as BigUIDef;
        if (IsLeft)
        {
            ControlModule.LeftFireballLevel += Merged ? 2 : 1;
            if (ControlModule.LeftFireballLevel == 1)
            {
                itemScreen.name = new BoxedString("Left Vengeful Spirit");
                itemScreen.bigSprite = new ItemChangerSprite("Prompts.Fireball1");
                itemScreen.sprite = new ItemChangerSprite("ShopIcons.Fireball1");
                itemScreen.descOne = new LanguageString("Prompts", "GET_FIREBALL_1");
                itemScreen.descTwo = new LanguageString("Prompts", "GET_FIREBALL_2");
            }
            else if (!Merged)
            {
                itemScreen.name = new BoxedString("Left Shade Soul");
                itemScreen.bigSprite = new ItemChangerSprite("Prompts.Fireball2");
                itemScreen.sprite = new ItemChangerSprite("ShopIcons.Fireball2");
                itemScreen.descOne = new LanguageString("Prompts", "GET_FIREBALL2_1");
                itemScreen.descTwo = new LanguageString("Prompts", "GET_FIREBALL2_2");
            }
        }
        else
        { 
            ControlModule.RightFireballLevel += Merged ? 2 : 1;
            
            if (ControlModule.RightFireballLevel == 1)
            {
                itemScreen.name = new BoxedString("Right Vengeful Spirit");
                itemScreen.bigSprite = new ItemChangerSprite("Prompts.Fireball1");
                itemScreen.sprite = new ItemChangerSprite("ShopIcons.Fireball1");
                itemScreen.descOne = new LanguageString("Prompts", "GET_FIREBALL_1");
                itemScreen.descTwo = new LanguageString("Prompts", "GET_FIREBALL_2");
            }
            else if (!Merged)
            {
                itemScreen.name = new BoxedString("Right Shade Soul");
                itemScreen.bigSprite = new ItemChangerSprite("Prompts.Fireball2");
                itemScreen.sprite = new ItemChangerSprite("ShopIcons.Fireball2");
                itemScreen.descOne = new LanguageString("Prompts", "GET_FIREBALL2_1");
                itemScreen.descTwo = new LanguageString("Prompts", "GET_FIREBALL2_2");
            }
        }
        PDHelper.HasSpell = true;
        PDHelper.FireballLevel = Math.Max(ControlModule.LeftFireballLevel, ControlModule.RightFireballLevel);
    }

    /// <inheritdoc/>
    public override bool Redundant() => IsLeft ? ControlModule.LeftFireballLevel >= 2 : ControlModule.RightFireballLevel >= 2;
}
