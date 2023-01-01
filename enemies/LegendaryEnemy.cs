using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryEnemy : MonoBehaviour
{
    [SerializeField] AudioClip attackBasic;
    [SerializeField] AudioClip attackMedium;
    [SerializeField] AudioClip attackHeavy;
    [SerializeField] AudioClip attackHit;
    [SerializeField] AudioClip deathcry;
    [SerializeField] LegendaryItem reward;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(gameObject.GetComponent<LegendaryActor>().isAlive)
            {
                LegendaryBattle.io.CreateBattle(other.gameObject, gameObject);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            
            if(gameObject.GetComponent<CapsuleCollider>() != null)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            
        }
    }

    public LegendaryItem GetReward()
    {
        return reward;
    }

    public void PlayBattleSound(int index)
    {
        switch(index)
        {
            case 0:
                LegendaryCore.io.PlaySfx4(attackBasic);
                break;
            case 1:
                LegendaryCore.io.PlaySfx4(attackMedium);
                break;
            case 2:
                LegendaryCore.io.PlaySfx4(attackHeavy);
                break;
            case 3:
                LegendaryCore.io.PlaySfx4(attackHit);
                break;
            case 4:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MONSTER_DEATH_DROP);
                break;
            case 5:
                LegendaryCore.io.PlaySfx4(deathcry);
                break;
        }
    }

}
