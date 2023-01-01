using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryDestructable : MonoBehaviour
{
    [SerializeField] LegendaryItem reward;
    [SerializeField] PhantaliaWorldItem itemType;
    [SerializeField] int hitpoints = 5;

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
            hitpoints--;

            if(hitpoints < 0)
            {
                LegendaryCraft.io.StrikeRock(other.transform.position);
                LegendaryCore.io.SetItem(reward.itemTitle, reward.itemDescription, reward.thumb);
                LegendaryCore.io.ShowItem(1);
                LegendaryInventory.io.IncreaseItem(itemType, 1);
                LegendaryCore.io.PlayDestructEffect();
                LegendaryCore.io.PlaceDebris(itemType, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
