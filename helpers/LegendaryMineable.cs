using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryMineable : MonoBehaviour
{
    [SerializeField] LegendaryItem reward;
    [SerializeField] PhantaliaWorldItem itemType;

    float cooldown = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WeaponHead_Hammer"))
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_CRAFTING_HAMMER_STRIKE);
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
                LegendaryCraft.io.StrikeRock(other.transform.position);
                LegendaryCore.io.SetItem(reward.itemTitle, reward.itemDescription, reward.thumb);
                LegendaryCore.io.ShowItem(1);
                LegendaryInventory.io.IncreaseItem(itemType, 1);
                LegendaryInventory.io.AddLegendaryItem(reward);
                //Debug.Log(LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.WOOD));
            }

        }
    }
}
