using ItemChanger;
using KorzUtils.Helper;
using System;
using UnityEngine;

namespace ExtraRando.ModInterop.ItemChangerInterop;

[Serializable]
public class WrappedSprite : ISprite
{
    #region Constructors

    public WrappedSprite() { }

    public WrappedSprite(string key)
    {
        if (!string.IsNullOrEmpty(key))
            Key = key;
    }

    #endregion

    #region Properties

    public string Key { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public Sprite Value => SpriteHelper.CreateSprite<ExtraRando>("Sprites." + Key.Replace("/", ".").Replace("\\", "."));

    #endregion

    public ISprite Clone() => new WrappedSprite(Key);
}