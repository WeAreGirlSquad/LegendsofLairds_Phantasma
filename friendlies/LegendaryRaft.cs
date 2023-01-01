using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryRaft : MonoBehaviour
{

    public float cooldownRaft;
    public Transform pole;

    private void Awake()
    {
        cooldownRaft = Time.time;
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            if (cooldownRaft < Time.time)
            {
                other.GetComponent<LegendaryPlayer>().SetRaft(gameObject);
                other.GetComponent<LegendaryPlayer>().Sail();
                cooldownRaft = Time.time + 0.25f;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cooldownRaft < Time.time)
            {
                other.GetComponent<LegendaryPlayer>().StopSailing();
                cooldownRaft = Time.time + 0.25f;
            }
               
        }
    }

}
