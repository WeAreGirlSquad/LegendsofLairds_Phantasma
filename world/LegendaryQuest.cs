using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LegendaryQuestObjective
{
    GATHER,
    KILL,
    ESCORT,
    DISCOVER,

}

public enum LegendaryQuestStatus
{
    NOT_AVAILABLE,
    AVAILABLE,
    ACCEPTED,
    FINISHED_SUCCESS,
    FINISHED_FAILURE,
    INVISIBLE
}

[CreateAssetMenu(fileName = "item_00", menuName = "LegendaryObjects/LegendaryQuest", order = 1)]
public class LegendaryQuest : ScriptableObject
{
    public int id;
    public LegendaryQuestObjective objective;
    public LegendaryQuestStatus status;
    public string title;
    public string slug;
    [TextArea(10,10)] public string description;

    [TextArea(5, 5)] public string accomplished;
    [TextArea(5, 5)] public string failed;

    public int areaLocation;
    public int[] actors;
    public int[] actorlocations;
    public int[] actorai;

    public int treasureLocation;
    public LegendaryItem rewardItem;
}
