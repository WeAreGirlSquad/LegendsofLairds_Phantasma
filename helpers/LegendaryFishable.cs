using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryFishable : MonoBehaviour
{

    GameObject currentUser;
    float cooldown_action = 0.0f;
    bool inAction = false;
    string hash_player = "Player";
    [SerializeField] GameObject fishCam;

    void ShowAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(true, false, "Fish", true);
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
            inAction = false;
            fishCam.SetActive(false);
            //LegendaryCore.io.RunTransition();
            LegendaryCore.io.ToggleFishingMode(false);
            currentUser.GetComponent<LegendaryPlayer>().DeactivateTool();
            currentUser.GetComponent<LegendaryPlayer>().Fishing(false);
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
                inAction = true;
                fishCam.SetActive(true);
                currentUser = other.gameObject;
                currentUser.GetComponent<LegendaryPlayer>().Fishing(true);
                //transform.GetChild(0).gameObject.SetActive(true);
                //LegendaryCore.io.ToggleWorldHud(false);
                LegendaryCore.io.ToggleFishingMode(true);
                //LegendaryCraft.io.SetImpactPoint(impactPoint);
                LegendaryCraft.io.SetupCrafting(LegendaryCraftingType.FISHING);



                HideAvailableActions();

            }

        }
    }

    /*


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }


        if (other.gameObject.CompareTag("Player"))
        {
            LegendaryCore.io.RunTransition();
            //inUse = true;
            currentUser = other.gameObject;
            currentUser.GetComponent<LegendaryPlayer>().Fishing(true);
            //transform.GetChild(0).gameObject.SetActive(true);
            //LegendaryCore.io.ToggleWorldHud(false);
            LegendaryCore.io.ToggleFishingMode(true);
            //LegendaryCraft.io.SetImpactPoint(impactPoint);
            LegendaryCraft.io.SetupCrafting(LegendaryCraftingType.FISHING);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        } else
        {
            LegendaryCore.io.RunTransition();
            LegendaryCore.io.ToggleFishingMode(false);
            currentUser.GetComponent<LegendaryPlayer>().DeactivateTool();
            currentUser.GetComponent<LegendaryPlayer>().Fishing(false);
        }


    }
    */
}
