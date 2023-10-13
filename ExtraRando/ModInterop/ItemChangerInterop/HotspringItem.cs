using ItemChanger;
using System.Collections;
using UnityEngine;

namespace ExtraRando.ModInterop.ItemChangerInterop;

public class HotspringItem : AbstractItem
{
    #region Methods

    public override void GiveImmediate(GiveInfo info) => HeroController.instance.StartCoroutine(Replenish());
    
    private IEnumerator Replenish()
    {
        float passedTime = 0f;
        float healthTime = 0f;
        float soulTime = 0f;
        bool glowUp = false;

        tk2dSprite heroSprite = HeroController.instance.GetComponent<tk2dSprite>();
        Color phasingColor = new(Color.white.r - Color.cyan.r, Color.white.g - Color.cyan.g, Color.white.b - Color.cyan.b);
        while (passedTime < 30f)
        {
            passedTime += Time.deltaTime;
            healthTime += Time.deltaTime;
            soulTime += Time.deltaTime;
            if (healthTime >= 1.5f)
            {
                healthTime = 0f;
                HeroController.instance.AddHealth(1);
            }
            if (soulTime >= 0.2f)
            {
                soulTime = 0f;
                HeroController.instance.AddMPChargeSpa(4);
            }
            Color currentColor = heroSprite.color;
            if (glowUp)
            {
                heroSprite.color = new(currentColor.r + phasingColor.r * Time.deltaTime, currentColor.g + phasingColor.g * Time.deltaTime, currentColor.b + phasingColor.b * Time.deltaTime);
                glowUp = !(heroSprite.color.r >= 1);
            }
            else
            {
                heroSprite.color = new(currentColor.r - phasingColor.r * Time.deltaTime, currentColor.g - phasingColor.g * Time.deltaTime, currentColor.b - phasingColor.b * Time.deltaTime);
                glowUp = heroSprite.color.r <= Color.cyan.r;
            }
            yield return null;
            if (GameManager.instance != null && GameManager.instance.IsGamePaused())
                yield return new WaitUntil(() => GameManager.instance == null || !GameManager.instance.IsGamePaused());
            if (heroSprite is null)
            {
                if (HeroController.instance is null)
                    break;
                heroSprite = HeroController.instance.GetComponent<tk2dSprite>();
            }
        }
        if (heroSprite != null)
            heroSprite.color = Color.white;
    }

    #endregion
}
