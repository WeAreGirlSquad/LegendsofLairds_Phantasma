using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryAqua : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<LegendaryPlayer>().inAquaticBody(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<LegendaryPlayer>().inAquaticBody(false);
        }
    }
}
