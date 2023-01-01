using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryWall : MonoBehaviour
{

    float cooldown_grab;

    private void Awake()
    {
        cooldown_grab = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(cooldown_grab > Time.time)
        {
            return;
        }

        Debug.Log("entered.");

        if(other.CompareTag("Player"))
        {
            other.GetComponent<LegendaryPlayer>().Climbable(transform);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("exit");
            other.GetComponent<LegendaryPlayer>().unClimb();
            cooldown_grab = Time.time + 0.01f;
        }

    }

}
