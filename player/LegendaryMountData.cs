using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LegendaryMountType
{
    HORSE,
    TURTLE,
    DOLPHIN,
    DRAGON,
    BEAR,
    WOLF,
    GOAT,
    THUNDERBIRD,
    TIGER
}

[CreateAssetMenu(fileName = "mountdata_item_00", menuName = "LegendaryObjects/LegendaryMountData", order = 1)]
public class LegendaryMountData : ScriptableObject
{
    public int id;
    public Sprite avatar;
    public LegendaryMountType category;
}
