using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendarySysconfig", order = 1)]
public class LegendarySysconfig : ScriptableObject
{
    // FLAGS
    public bool vsync = true;

    // LEVELS
    public int quality = 5;
}
