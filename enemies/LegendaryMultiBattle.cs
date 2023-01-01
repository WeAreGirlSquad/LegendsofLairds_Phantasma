using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryMultiBattle : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    string hash_player = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(hash_player))
        {
            GetComponent<BoxCollider>().enabled = false;
            LegendaryBattle.io.CreateMultiBattle(enemies, other.gameObject, false);
        }
        
    }
}
