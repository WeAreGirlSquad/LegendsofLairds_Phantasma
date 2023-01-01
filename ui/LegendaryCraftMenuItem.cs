using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum LegendaryWeaponPart
{
    DAGGER_BLADE,
    SWORD_BLADE,
    AXE_BLADE,
    HAMMER_HEAD,
    STAFF_HEAD,
    ARROW_TIP,
    SPEAR_TIP,
    WOODEN_GRIP,
    IRON_GRIP,
    STEEL_GRIP,
    WOODEN_POMMEL,
    IRON_POMMEL,
    STEEL_POMMEL,
    WOODEN_GUARD,
    IRON_GUARD,
    STEEL_GUARD,
}

[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryCraftMenuItem", order = 1)]
public class LegendaryCraftMenuItem : ScriptableObject
{
    public int id;
    public Sprite thumb;
    public string itemTitle;
    public string itemDescription;
    public bool isAvailable;
    public int iron;
    public int carbon;
    public int tin;
    public LegendaryWeaponPart compositeCategory;
    public int power;
}

