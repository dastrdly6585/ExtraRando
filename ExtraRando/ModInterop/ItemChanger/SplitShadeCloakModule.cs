using ItemChanger.Modules;
using KorzUtils.Enums;
using KorzUtils.Helper;
using Modding;
using MonoMod.Cil;
using System;

namespace ExtraRando.ModInterop.ItemChanger;

public class SplitShadeCloakModule : Module
{
    #region Properties

    public bool HasLeftCloak { get; set; }

    public bool HasRightCloak { get; set; }

    #endregion

    #region Event handler

    private void HeroController_HeroDash(ILContext il)
    {
        ILCursor iLCursor = new(il);
        iLCursor.Goto(0);
        if (iLCursor.TryGotoNext(MoveType.After,
            x => x.MatchLdstr("hasShadowDash"),
            x => x.MatchCallvirt<PlayerData>("GetBool")))
        {
            iLCursor.EmitDelegate<Func<bool, bool>>(x =>
            {
                // 0 down, 1 left, 2 right
                int direction = DetermineDirection();
                return (x && direction == 0) || (direction == 1 && HasLeftCloak) || (direction == 2 && HasRightCloak);
            });
        }
    }

    private bool ModHooks_GetPlayerBoolHook(string name, bool orig)
    {
        if (name == "leftShadeDash")
            return HasLeftCloak;
        else if (name == "rightShadeDash")
            return HasRightCloak;
        return orig;
    }

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        if (name == "leftShadeDash")
        {
            HasLeftCloak = orig;
            PlayerData.instance.SetBool(nameof(PlayerData.hasShadowDash), true);
        }
        else if (name == "rightShadeDash")
        { 
            HasRightCloak = orig;
            PlayerData.instance.SetBool(nameof(PlayerData.hasShadowDash), true);
        }
        return orig;
    }

    #endregion

    #region Public Methods

    public override void Initialize() 
    { 
        IL.HeroController.HeroDash += HeroController_HeroDash;
        ModHooks.GetPlayerBoolHook += ModHooks_GetPlayerBoolHook;
        ModHooks.SetPlayerBoolHook += ModHooks_SetPlayerBoolHook;
    }

    public override void Unload()
    {
        IL.HeroController.HeroDash -= HeroController_HeroDash;
        ModHooks.GetPlayerBoolHook -= ModHooks_GetPlayerBoolHook;
        ModHooks.SetPlayerBoolHook -= ModHooks_SetPlayerBoolHook;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Took the same logic as the original split dash from https://github.com/homothetyhk/HollowKnight.ItemChanger/blob/master/ItemChanger/Modules/SplitCloak.cs#LL101C9-L115C10
    /// </summary>
    private int DetermineDirection()
    {
        HeroController heroController = HeroController.instance;
        InputHandler inputHandler = InputHandler.Instance;
        if (!heroController.cState.onGround && inputHandler.inputActions.down.IsPressed && CharmHelper.EquippedCharm(CharmRef.Dashmaster)
                && !(inputHandler.inputActions.left.IsPressed || inputHandler.inputActions.right.IsPressed))
            return 0;
        if (heroController.wallSlidingL)
            return 2;
        else if (heroController.wallSlidingR)
            return 1;
        else if (inputHandler.inputActions.right.IsPressed)
            return 2;
        else if (inputHandler.inputActions.left.IsPressed)
            return 1;
        else if (heroController.cState.facingRight)
            return 2;
        else
            return 1;
    }

    #endregion
}
