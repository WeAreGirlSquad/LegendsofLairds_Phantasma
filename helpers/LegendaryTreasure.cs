using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LegendaryTreasure : MonoBehaviour
{
    Animator animator;
    string hash_player = "Player";
    int anim_open_chest = Animator.StringToHash("Open_Chest");
    float intensity = 23.65f;
    [SerializeField] LegendaryItem treasure;
    [SerializeField] Light treasureLight;
    float cooldown_action = 0.0f;
    bool inAction = false;
    [SerializeField] Transform treasureCamera;
    [SerializeField] GameObject treasureFx;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTreasure(LegendaryItem treasureItem)
    {
        treasure = treasureItem;
    }

    public void Shine()
    {
        treasureFx.SetActive(true);
        LegendaryCore.io.SetTreasureItem(treasure.thumb, treasure.itemTitle, treasure.itemDescription);
        treasureLight.DOIntensity(intensity, 0.5f);
        StartCoroutine(DelayMoment(2.0f));
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.TREASURE_RECEIVED);
    }

    IEnumerator DelayMoment(float value)
    {
        yield return new WaitForSeconds(value);
        GiveTreasure();
        yield return new WaitForSeconds(value);
    }

    public void GiveTreasure()
    {
        LegendaryCore.io.willReceiveTreasureReward();
        LegendaryInventory.io.AddLegendaryItem(treasure);
        StartCoroutine(RemoveChest());
    }

    IEnumerator RemoveChest()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.INVENTORY_ENTRY);
        
        Destroy(transform.parent.gameObject, 4.0f);
        yield return new WaitForSeconds(3.5f);
        
        LegendaryCore.io.PlayDisappearEffect(transform);
        LegendaryCore.io.hasReceiveTreasureReward();
        
        //yield return null;
    }

    void ShowAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(true, false, "Open", true);
    }

    void HideAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(false, false, "", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            ShowAvailableActions();   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            HideAvailableActions();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            
            if (LegendaryCore.io.isActionOnePressed && !inAction)
            {
                if (cooldown_action > Time.time)
                {
                    return;
                }
                else
                {
                    cooldown_action = Time.time + 5.16f;
                }
                other.GetComponent<LegendaryPlayer>().Mount(false);
                treasureCamera.gameObject.SetActive(true);
                LegendaryCore.io.ToggleMiniMap(false);
                inAction = true;
                animator.SetTrigger(anim_open_chest);
                HideAvailableActions();

            }
            
        }
    }


}
