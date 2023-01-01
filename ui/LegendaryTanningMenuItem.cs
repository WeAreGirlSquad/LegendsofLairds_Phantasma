using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryTanningMenuItem", order = 1)]
public class LegendaryTanningMenuItem : ScriptableObject
{
    public int id;
    public Sprite thumb;
    public string itemTitle;
    public string itemDescription;
    public bool isAvailable;
    public int leather;
    public int rope;
    public int dye;
    public Color basecolor;
    public Texture2D tartan;
}
