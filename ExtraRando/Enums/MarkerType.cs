using ExtraRando.ModInterop.ItemChanger;

namespace ExtraRando.Enums;

/// <summary>
/// Represents the type/behaviour of the marker hint from <see cref="PinItem"/>.
/// </summary>
public enum MarkerType
{
    /// <summary>
    /// Gives a hint about a junk item
    /// </summary>
    ScarabMarker,

    /// <summary>
    /// Gives a hint about a potential useful item.
    /// </summary>
    GleamingMarker,

    /// <summary>
    /// Gives a hint about a useful item.
    /// </summary>
    TokenMarker,

    /// <summary>
    /// Gives a random preview of any location (including one item)
    /// </summary>
    ShellMarker
}
