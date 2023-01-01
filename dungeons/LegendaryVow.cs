using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LegendaryVow : MonoBehaviour
{
    string hash_player = "Player";
    bool inAction = false;
    float cooldown_action = 0.0f;
    int currentIndexDialogue = 0;
    [SerializeField] LegendaryQuestActor vowData;
    [SerializeField] int questId;
    Transform vowStone;

    private void Awake()
    {
        vowStone = transform.GetChild(0);
    }

    void ShowAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(true, false, "Take Vow", true);
    }

    void HideAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(false, false, "", false);
    }

    void StartDialogue()
    {
        inAction = true;
        Transform player = LegendaryCore.io.GetLocalPlayer();
        player.GetComponent<Animator>().SetBool("isPicking", true);
        LegendaryCore.io.SetChatData(vowData.avatar, vowData.actorname);
        LegendaryCore.io.SetChatData(true, true);
        if (vowData.dialogues.Length > 1)
        {
            LegendaryCore.io.Chat(vowData.dialogues[currentIndexDialogue], true);
        }
        else
        {
            LegendaryCore.io.Chat(vowData.dialogues[currentIndexDialogue]);
        }
    }

    public void NextStepDialogue()
    {
        if (currentIndexDialogue < vowData.dialogues.Length - 1)
        {
            currentIndexDialogue++;
            LegendaryCore.io.Chat(vowData.dialogues[currentIndexDialogue], true);
        }
        else
        {

            LegendaryCore.io.Chat(false);
            Reset();
            LegendaryQuestManager.io.InitQuest(questId);
            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.NOVA_YELLOW, transform);
            LegendaryCore.io.DestructWithEffect(gameObject);
            Transform player = LegendaryCore.io.GetLocalPlayer();
            player.GetComponent<Animator>().SetBool("isPicking", false);
        }
    }

    

    private void Reset()
    {
        currentIndexDialogue = 0;
        inAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            
                ShowAvailableActions();
                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.FLARES, vowStone);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
                if (LegendaryCore.io.isActionOnePressed)
                {

                    if (cooldown_action > Time.time)
                    {
                        return;
                    }
                    else
                    {
                        cooldown_action = Time.time + 2.16f;
                    }

                    if (!inAction)
                    {
                        other.transform.LookAt(transform.position);
                        DOTween.KillAll();
                        Sequence mySequence = DOTween.Sequence()
                        .Append(vowStone.DOMoveY(0.55f, 2f))
                        .Append(vowStone.DOLocalRotate(new Vector3(0,360,0), 2f, RotateMode.WorldAxisAdd).SetLoops(-1).SetEase(Ease.Linear))
                        ;
                        
                        HideAvailableActions();
                        StartDialogue();
                    }
                    else
                    {
                        NextStepDialogue();
                    }
                }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            Sequence mySequence = DOTween.Sequence()
                        .Append(vowStone.DOMoveY(0.2f, 2f))
                        ;
            //DOTween.KillAll(true);
            HideAvailableActions();
            Reset();
        }
    }



}
