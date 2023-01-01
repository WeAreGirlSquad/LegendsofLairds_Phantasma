using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "actors_stats_00", menuName = "LegendaryObjects/LegendaryActorStats", order = 1)]
public class LegendaryActorStats : ScriptableObject
{
    public int iron;
    public int muscle;
    public int heart;
    public int guile;
    public int intuition;
}
