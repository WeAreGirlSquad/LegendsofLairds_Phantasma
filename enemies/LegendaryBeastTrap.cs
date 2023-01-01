using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryBeastTrap : MonoBehaviour
{
    [SerializeField] GameObject beast;
    bool isActiveTrap = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(isActiveTrap)
            {
                return;
            }

            isActiveTrap = true;

            if (beast.GetComponent<LegendaryActor>().isAlive)
            {
                beast.SetActive(true);
                LegendaryBattle.io.CreateBattle(other.gameObject, beast);
                beast.GetComponent<BoxCollider>().enabled = false;
            }

            if (beast.GetComponent<CapsuleCollider>() != null)
            {
                beast.GetComponent<CapsuleCollider>().enabled = false;
            }

            Destroy(gameObject);

        }

    }
}
