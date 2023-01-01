using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using QFSW.QC;
using QFSW.QC.Actions;

public class LegendaryQuestManager : MonoBehaviour
{
    [SerializeField] Transform[] locations;
    [SerializeField] LegendaryQuest[] quests;
    [SerializeField] LegendaryQuestActor[] actors;
    [SerializeField] LegendaryStoryPoint[] storypoints;
    [SerializeField] GameObject[] spawnables;
    [SerializeField] GameObject treasureObject;

    int currentTrackedQuest = -1;
    List<GameObject> objectCache;

    List<LegendaryQuestObjective> objectives_discover;
    List<LegendaryQuestObjective> objectives_kill;

    public static LegendaryQuestManager io;
    private void Awake()
    {
        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }

        objectCache = new List<GameObject>();
        objectives_discover = new List<LegendaryQuestObjective>();
        objectives_kill = new List<LegendaryQuestObjective>();
    }

    private void Start()
    {

        //UpdateCurrentQuest(0);
        //LegendaryCore.io.UpdateQuestTracker(quests[currentTrackedQuest].title, quests[currentTrackedQuest].slug);
    }

    [Command]
    public void InitQuestManager()
    {
        UpdateCurrentQuest(0);
        LegendaryCore.io.UpdateQuestTracker(quests[currentTrackedQuest].title, quests[currentTrackedQuest].slug);
    }

    [Command]
    public void InitQuest(int id)
    {
        UpdateCurrentQuest(id);
        LegendaryCore.io.UpdateQuestTracker(quests[currentTrackedQuest].title, quests[currentTrackedQuest].slug);
    }

    public void UpdateCurrentQuest(int questid)
    {
        if(currentTrackedQuest == questid)
        {
            return;
        } else
        {
            currentTrackedQuest = questid;
        }

        if(quests[currentTrackedQuest].status == LegendaryQuestStatus.FINISHED_SUCCESS)
        {
            return;
        }

        foreach(GameObject cachedObject in objectCache) {
            Destroy(cachedObject);
        }
        objectives_discover.Clear();
        objectives_kill.Clear();

        currentTrackedQuest = questid;
        LegendaryCore.io.UpdateQuestTracker(quests[currentTrackedQuest].title, quests[currentTrackedQuest].slug);

        // spawn actor(s)
        for (int i = 0; i < quests[currentTrackedQuest].actors.Length; i++)
        {
            GameObject actor = Instantiate(spawnables[quests[currentTrackedQuest].actors[i]], locations[quests[currentTrackedQuest].actorlocations[i]].position, locations[quests[currentTrackedQuest].actorlocations[i]].rotation);
            objectCache.Add(actor);
            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GOLD_AURA, actor.transform);
            LegendaryQuestActor actor_ref = actors[quests[currentTrackedQuest].actorai[i]];
            Debug.Log(actor_ref);
            actor.GetComponent<LegendaryAI>().SetupIntelligentCharacter(actor_ref, actor_ref.ai);
            actor.AddComponent<LegendaryTrackableObject>();
            actor.GetComponent<LegendaryTrackableObject>().questId = currentTrackedQuest;
            if(actor.GetComponent<LegendaryEnemy>() != null)
            {
                actor.GetComponent<LegendaryTrackableObject>().objective = LegendaryQuestObjective.KILL;
                objectives_kill.Add(LegendaryQuestObjective.KILL);
            }
            if(actor.GetComponent<LegendaryFriendly>() != null)
            {
                actor.GetComponent<LegendaryTrackableObject>().objective = LegendaryQuestObjective.DISCOVER;
                objectives_discover.Add(LegendaryQuestObjective.DISCOVER);
            }
        }
        Debug.Log("setting area location");
        LegendaryCore.io.SetQuestMapObject(locations[quests[currentTrackedQuest].areaLocation].gameObject);
        LegendaryCore.io.EnableCompass(true);





    }

    public void UpdateQuestObjective(int id, LegendaryQuestObjective objective)
    {
        Debug.Log("updated quest " + id + " with objective " + objective);
        switch(objective)
        {
            case LegendaryQuestObjective.KILL:
                if(objectives_kill.Count > 0)
                {
                    objectives_kill.Remove(objective);
                }
                break;
            case LegendaryQuestObjective.DISCOVER:
                if (objectives_discover.Count > 0)
                {
                    objectives_discover.Remove(objective);
                }
                break;
        }

        if(CompleteQuest(id))
        {
            Debug.Log("complete quest success");
            LegendaryCore.io.ShowWorldMessage("Completed " + quests[currentTrackedQuest].title, quests[currentTrackedQuest].accomplished);
            GameObject treasure = Instantiate(treasureObject, locations[quests[currentTrackedQuest].treasureLocation]);
            Debug.Log("pos: " + treasure.transform.position);
            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GROUND_WIND, treasure.transform);
            treasure.transform.GetChild(0).GetComponent<LegendaryTreasure>().SetTreasure(quests[currentTrackedQuest].rewardItem);
            LegendaryCore.io.SetQuestMapObject(treasure);
            LegendaryCore.io.ToggleQuestBox(false);
            Debug.Log("completed quest " + id + " succesful");
            foreach (GameObject go in objectCache)
            {
                Destroy(go);
            }
            objectCache.Clear();
            quests[currentTrackedQuest].status = LegendaryQuestStatus.FINISHED_SUCCESS;
        }
    }

    public bool StartQuest(int id)
    {
        return false;
    }

    public LegendaryQuestStatus GetQuestStatus(int id)
    {
        return LegendaryQuestStatus.NOT_AVAILABLE;
    }

    public bool CompleteQuest(int id)
    {
        Debug.Log(" discovered: " + objectives_discover.Count);
        Debug.Log(" killed: " + objectives_kill.Count);

        if (objectives_kill.Count == 0 && objectives_discover.Count == 0)
        {
            return true;
        }
        return false;
    }

    public void SpawnActors(int id)
    {

    }

    public void RemoveActors(int id)
    {

    }


}
