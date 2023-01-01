using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryStructureItem", order = 1)]
public class LegendaryStructureItem : ScriptableObject
{
    public int id;
    public Sprite thumb;
    public string itemTitle;
    public string itemDescription;
    public bool isAvailable;
    public GameObject structure;
    public GameObject placeholder;
    public float constructionTime;
    public LegendaryCursorSize size;
}

/*
 public enum PhantaliaWorldItem
{
    IRON,
    CARBON,
    TIN,
    WOOD,
    ROCK,
    MEAT,
    LEAF,
    FRUIT,
    POWDER,
    FLUID,
    LEATHER,
    WOOL,
    GOLD,
    SILVER,
    COPPER,
    DARKIRON,
    BONES,
    DYE,
    ROPE,
}
*/