using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LegendaryItemCategory
{
    CONSUMABLE_ITEM,
    INGREDIENT,
    NATURAL_RESOURCE,
    LOOT,
    SPECIAL_ITEM,
    QUEST_ITEM,
    WEAPON_SWORD,
    WEAPON_AXE,
    WEAPON_DAGGER,
    WEAPON_STAFF,
    WEAPON_HAMMER,
    WEAPON_BOW,
    WEAPON_SPEAR,
    WEAPON_PART,
    QUILT,
    WEARABLE,
    FACEPAINT,
    RELIC,
    CURRENCY
}


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryItem", order = 1)]
public class LegendaryItem : ScriptableObject
{
    public int id;
    public Sprite thumb;
    public string itemTitle;
    public string itemDescription;
    public LegendaryItemCategory category;
    public LegendaryWeaponPart compositeCategory;
    string hexid;
    public bool isConsumable;
    public bool isEquipableWeapon;
    public bool isComposite;
    public bool isWearable;
    public bool isPaintable;
    public Color basecolor;
    public Texture2D tartan;
    public int hp;
    public int ph;
    public int power;
    public LegendaryWeaponRingItem weapon;

}
