using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "actor_00", menuName = "LegendaryObjects/LegendaryQuestActor", order = 1)]
public class LegendaryQuestActor : ScriptableObject
{
    public string actorname;
    public Sprite avatar;
    [TextArea(5,5)]
    public string[] dialogues;
    public LegendaryAiBehaviour ai;

}
