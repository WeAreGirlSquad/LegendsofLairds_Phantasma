using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryTrackableObject : MonoBehaviour
{
    public int questId;
    public LegendaryQuestObjective objective;

    private void OnDestroy()
    {
        if(gameObject.GetComponent<LegendaryEnemy>() != null)
        {
            LegendaryQuestManager.io.UpdateQuestObjective(questId, objective);
        }

        /*
        if (gameObject.GetComponent<LegendaryFriendly>() != null)
        {
            LegendaryQuestManager.io.UpdateQuestObjective(questId, objective);
        }
        */


        //Debug.Log("Trackable Obj dead.");
    }
}
