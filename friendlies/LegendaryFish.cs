using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryFish : MonoBehaviour
{
    [SerializeField] LegendaryItem reward;

    float cooldown = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WeaponHead_Spear"))
        {
            //LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_SPLASH);
            if (cooldown > Time.time)
            {
                return;
            }
            //Debug.Log("hit");
            cooldown = Time.time + 0.12f;

            // randomify
            float dice = Random.Range(0.0f, 1.0f);
            if (dice < 0.8f)
            {
                LegendaryCraft.io.StrikeFish(transform.position);
                LegendaryCore.io.SetItem(reward.itemTitle, reward.itemDescription, reward.thumb);
                LegendaryCore.io.ShowItem(1);
                LegendaryInventory.io.IncreaseItem(PhantaliaWorldItem.MEAT, 1);
                LegendaryInventory.io.AddLegendaryItem(reward);
                //Debug.Log(LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.MEAT));
                gameObject.SetActive(false);
            }

        }
    }
}
