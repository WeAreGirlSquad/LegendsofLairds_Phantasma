using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryStoryPoint", order = 1)]
public class LegendaryStoryPoint : ScriptableObject
{
    public int id;
    public LegendaryItem[] requiredItem;
    public LegendaryItem[] rewardItem;

}
