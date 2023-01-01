using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryHarvestable : MonoBehaviour
{
    [SerializeField] LegendaryItem reward;

    float cooldown = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("WeaponHead_Axe"))
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.HARVEST_WOOD);
            if(cooldown > Time.time)
            {
                return;
            }
            //Debug.Log("hit");
            cooldown = Time.time + 0.12f;
            
            // randomify
            float dice = Random.Range(0.0f, 1.0f);
            if(dice < 0.8f)
            {
                LegendaryCore.io.HarvestableHit(transform);
                LegendaryCore.io.SetItem(reward.itemTitle, reward.itemDescription, reward.thumb);
                LegendaryCore.io.ShowItem(1);
                LegendaryInventory.io.IncreaseItem(PhantaliaWorldItem.WOOD, 1);
                LegendaryInventory.io.AddLegendaryItem(reward);
                //Debug.Log(LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.WOOD));
            }
            
        }
    }

}
