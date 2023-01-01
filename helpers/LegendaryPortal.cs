using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryPortal : MonoBehaviour
{
    [SerializeField] GameObject portalObject;
    [SerializeField] GameObject warpObject;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            portalObject.SetActive(true);
            warpObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            portalObject.SetActive(false);
            warpObject.SetActive(false);
        }
    }
}
