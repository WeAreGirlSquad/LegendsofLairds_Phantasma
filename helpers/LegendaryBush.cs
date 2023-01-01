using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryBush : MonoBehaviour
{
    string hash_player = "Player";
    float cooldown_action = 0.0f;
    bool inAction = false;

    [SerializeField] PhantaliaWorldItem bushReward;
    [SerializeField] LegendaryItem reward;

    
    void ShowAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(true, false, "Pick", true);
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
                LegendaryCore.io.ToggleMiniMap(false);
                LegendaryInventory.io.IncreaseItem(bushReward, 1);
                LegendaryInventory.io.AddLegendaryItem(reward);
                LegendaryCore.io.SetItem(reward.itemTitle, reward.itemDescription, reward.thumb);
                LegendaryCore.io.ShowItem(1);
                inAction = true;
                
                HideAvailableActions();
                GetComponent<BoxCollider>().enabled = false;
                //gameObject.SetActive(false); // should respawn
            }

        }
    }
}
