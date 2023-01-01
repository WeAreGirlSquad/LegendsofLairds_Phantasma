using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryHealer : MonoBehaviour
{
    float healtime = 10f;
    LegendaryPlayer player;

    string hash_player = "Player";
    int anim_camp = Animator.StringToHash("isCamping");
    bool inAction = false;
    float cooldown_action = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(HealActor());
    }

    private void OnEnable()
    {
        player = LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>();
    }

    void ShowAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(true, false, "Rest", true);
    }

    void HideAvailableActions()
    {
        LegendaryCore.io.ToggleInteractionButtons(false, false, "", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hash_player) && !inAction)
        {
            ShowAvailableActions();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            if (LegendaryCore.io.isActionOnePressed)
            {
                if(inAction)
                {
                    return;
                }


                if (cooldown_action > Time.time)
                {
                    return;
                }
                else
                {
                    cooldown_action = Time.time + 2.16f;
                }
                HideAvailableActions();
                inAction = true;
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().InGameMenu(true);
                StartCoroutine(HealActor());
                // start

            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hash_player))
        {
            HideAvailableActions();
        }
    }




    IEnumerator HealActor()
    {
        LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetBool(anim_camp, true);
        yield return new WaitForSeconds(5);
        LegendaryCore.io.AddHealth(2);
        yield return new WaitForSeconds(5);
        LegendaryCore.io.AddHealth(2);
        yield return new WaitForSeconds(5);
        LegendaryCore.io.AddHealth(2);
        yield return new WaitForSeconds(5);
        LegendaryCore.io.AddHealth(2);
        LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetBool(anim_camp, false);
        yield return new WaitForSeconds(5);
        LegendaryCore.io.AddHealth(2);
        //LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetBool(anim_camp, false);
        Destroy(transform.GetChild(2).gameObject);
        LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().InGameMenu(false);
        yield return null;
    }

}
