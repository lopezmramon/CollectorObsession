using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public enum ColorKey
{
    BACKGROUND_COLOR, PLAYER_TOP, PLAYER_TOP_TRAIL, PLAYER_TOP_RESET_RING, PLAYER_TOP_CHEVRON, PLAYER_TOP_DUST, PLAYER_BOTTOM, PLAYER_BOTTOM_TRAIL, PLAYER_BOTTOM_RESET_RING, PLAYER_BOTTOM_CHEVRON, PLAYER_BOTTOM_DUST, INACTIVE_SWITCH, ACTIVE_SWITCH, ACTIVE_SWITCH_LIGHT, EXITS, SPIKES, PLATFORM, PLATFORM_PATH, GROUND_TILE, ONE_WAY_TILE, FILL_IN_TILE, BREAKAWAY_TILE, TINTED_TILE, ROTATOR, REVERSER, INVERTER, FONT_COLOR
}

/// <summary>
/// Generic wrapper because unity don't really like generics
/// </summary>
[Serializable]
public class Sample : GenericEnumCollection<ColorKey, Color> {}
public class EnumCollectionSample : MonoBehaviour
{
    public Sample SampleCollection;

    public string a;
    public ColorKey Key;

    [EnumFilter]
    public ColorKey ColorKey;

    [EnumFilter]
    public ColorKey[] AColorKey;

    public string[] strings;
}
