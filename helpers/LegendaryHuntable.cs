using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryHuntable : MonoBehaviour
{
    [SerializeField] LegendaryItem reward;
    [SerializeField] int hitpoints = 1;

    float cooldown = 0f;
    bool isAlive = true;

    public bool IsHuntableAlive()
    {
        return isAlive;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(!isAlive)
        {
            return;
        }


        if (other.gameObject.CompareTag("WeaponHead_Arrow") || other.gameObject.CompareTag("WeaponHead_Spear"))
        {
            //LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_SPLASH);
            if (cooldown > Time.time)
            {
                return;
            }
            //Debug.Log("hit");
            cooldown = Time.time + 0.12f;
            
            if(other.gameObject.CompareTag("WeaponHead_Arrow"))
            {
                Destroy(other.gameObject, 0.2f);
            }

            // randomify
            float dice = Random.Range(0.0f, 1.0f);
            if (dice < 0.8f)
            {
                hitpoints -= 1;
                gameObject.GetComponent<Animator>().SetTrigger("HUNT_HIT");
                gameObject.GetComponent<LegendaryActor>().currentHP -= 1;

                if (hitpoints < 1)
                {
                    LegendaryCore.io.SetItem(reward.itemTitle, reward.itemDescription, reward.thumb);
                    LegendaryCore.io.ShowItem(1);
                    LegendaryInventory.io.IncreaseItem(PhantaliaWorldItem.MEAT, 1);
                    //Debug.Log(LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.MEAT));
                    //gameObject.SetActive(false);
                    gameObject.GetComponent<LegendaryActor>().isAlive = false;
                    gameObject.GetComponent<Animator>().SetTrigger("HUNT_DIE");
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    //gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    if(gameObject.GetComponent<LegendaryAnimal>() != null)
                    {
                        gameObject.GetComponent<LegendaryAnimal>().SetHitDelay();
                    }
                    
                    isAlive = false;
                }
                LegendaryCraft.io.StrikeAnimal(transform.position);
                
            }

        }
    }
}
