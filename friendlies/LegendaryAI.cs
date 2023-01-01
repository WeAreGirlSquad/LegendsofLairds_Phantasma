using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum LegendaryIntelligence
{
    INACTIVE,
    ACTIVE,
    HOSTILE
}

public enum LegendaryAiBehaviour
{
    IDLE,
    CROUCHING,
    COOKING,
    SLEEPING,
    HURT,
    CAMPING,
    PARANOID
}

public class LegendaryAI : MonoBehaviour
{
    // init
    Animator aiAnimator;
    LegendaryIntelligence mode;
    LegendaryActor actorstats;

    // dynamic
    [SerializeField] LegendaryQuestActor data;
    [SerializeField] LegendaryAiBehaviour behaviour;

    // caches
    string hash_player = "Player";

    int anim_emotion_yes = Animator.StringToHash("NOD_YES");
    int anim_emotion_no = Animator.StringToHash("NOD_NO");
    int anim_emotion_shock = Animator.StringToHash("EMO_SHOCK");
    int anim_emotion_joy = Animator.StringToHash("EMO_JOY");

    int anim_action_give = Animator.StringToHash("ACTION_GIVE");
    int anim_action_pickup = Animator.StringToHash("ACTION_PICKUP");

    int anim_ai_type = Animator.StringToHash("AI_TYPE");
    int anim_isMoving = Animator.StringToHash("isMoving");
    int anim_isAlive = Animator.StringToHash("isAlive");

    bool inAction = false;
    float cooldown_action = 0.0f;

    [SerializeField] bool independentNpc = false; 

    // indexes
    int currentIndexDialogue = 0;

    bool discovered = false;

    public void SetupIntelligentCharacter(LegendaryQuestActor initdata, LegendaryAiBehaviour aibehaviour)
    {
        data = initdata;
        behaviour = aibehaviour;
        switch (behaviour)
        {
            case LegendaryAiBehaviour.CAMPING:
                aiAnimator.SetInteger("AI_TYPE", 2);
                break;
            case LegendaryAiBehaviour.CROUCHING:
                aiAnimator.SetInteger("AI_TYPE", 4);
                break;
            case LegendaryAiBehaviour.SLEEPING:
                aiAnimator.SetInteger("AI_TYPE", 1);
                break;
            case LegendaryAiBehaviour.COOKING:
                aiAnimator.SetInteger("AI_TYPE", 3);
                break;
            case LegendaryAiBehaviour.HURT:
                aiAnimator.SetInteger("AI_TYPE", 5);
                break;
            case LegendaryAiBehaviour.PARANOID:
                aiAnimator.SetInteger("AI_TYPE", 6);
                break;

        }
        //Debug.Log("Init AI");
    }

    private void Awake()
    {
        aiAnimator = gameObject.GetComponent<Animator>();
        actorstats = gameObject.GetComponent<LegendaryActor>();
    }

    void Start()
    {
        mode = LegendaryIntelligence.ACTIVE;
        if(independentNpc)
        {
            if(data != null)
            {
                behaviour = data.ai;

                switch (behaviour)
                {
                    case LegendaryAiBehaviour.CAMPING:
                        aiAnimator.SetInteger("AI_TYPE", 2);
                        break;
                    case LegendaryAiBehaviour.CROUCHING:
                        aiAnimator.SetInteger("AI_TYPE", 4);
                        break;
                    case LegendaryAiBehaviour.SLEEPING:
                        aiAnimator.SetInteger("AI_TYPE", 1);
                        break;
                    case LegendaryAiBehaviour.COOKING:
                        aiAnimator.SetInteger("AI_TYPE", 3);
                        break;
                    case LegendaryAiBehaviour.HURT:
                        aiAnimator.SetInteger("AI_TYPE", 5);
                        break;
                    case LegendaryAiBehaviour.PARANOID:
                        aiAnimator.SetInteger("AI_TYPE", 6);
                        break;

                }
            }
        }

    }

    void ShowAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(true, false, "Talk", true);
    }

    void HideAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(false, false, "", false);
    }

    void Update()
    {
        if(mode == LegendaryIntelligence.INACTIVE)
        {
            return;
        }
    }

    void StartDialogue()
    {
        inAction = true;
        //Debug.Log(data.dialogues.Length);
        //Debug.Log(currentIndexDialogue);
        LegendaryCore.io.SetChatData(data.avatar, data.actorname);
        LegendaryCore.io.SetChatData(true, true);
        if(data.dialogues.Length > 1)
        {
            LegendaryCore.io.Chat(data.dialogues[currentIndexDialogue], true);
        } else
        {
            LegendaryCore.io.Chat(data.dialogues[currentIndexDialogue]);
        }
        
        //
    }

    public void NextStepDialogue()
    {
        if(currentIndexDialogue < data.dialogues.Length -1 )
        {
            currentIndexDialogue++;
            LegendaryCore.io.Chat(data.dialogues[currentIndexDialogue], true);
            //Debug.Log("next step");
            // data.dialogues[currentIndexDialogue];
        } else
        {
            LegendaryCore.io.Chat(false);
            if(!discovered)
            {
                LegendaryQuestManager.io.UpdateQuestObjective(gameObject.GetComponent<LegendaryTrackableObject>().questId, gameObject.GetComponent<LegendaryTrackableObject>().objective);
                discovered = true;
            }
            
            //LegendaryCore.io.Chat(bool end);
            Reset();
        }
    }

    private void Reset()
    {
        currentIndexDialogue = 0;
        inAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(hash_player))
        {
            if(mode == LegendaryIntelligence.ACTIVE)
            {
                ShowAvailableActions();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(hash_player))
        {
            if(mode == LegendaryIntelligence.ACTIVE)
            {
                if(LegendaryCore.io.isActionOnePressed)
                {
                    //Debug.Log("Got Keypress");
                    if (cooldown_action > Time.time)
                    {
                        return;
                    } else
                    {
                        cooldown_action = Time.time + 2.16f;
                    }

                    if(!inAction)
                    {
                        //Debug.Log("Got start dialogue");
                        HideAvailableActions();
                        StartDialogue();
                    } else
                    {
                        NextStepDialogue();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            HideAvailableActions();
            if (mode == LegendaryIntelligence.ACTIVE)
            {
                Reset();
            }
        }
    }
}
