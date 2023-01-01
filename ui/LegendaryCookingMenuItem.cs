using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryCookingMenuItem", order = 1)]
public class LegendaryCookingMenuItem : ScriptableObject
{
    public int id;
    public Sprite thumb;
    public string itemTitle;
    public string itemDescription;
    public bool isAvailable;
    public int meat;
    public int leaf;
    public int fruit;
    public int powder;
    public int recover_hp;
    public int recover_ph;
}

