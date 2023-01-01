using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryWeaponRingItem", order = 1)]
public class LegendaryWeaponRingItem : ScriptableObject
{
    public int id;
    public Sprite thumb;
    public string itemTitle;
    public string itemDescription;
    public LegendaryWeaponType itemType;
    public GameObject prefab;

}
