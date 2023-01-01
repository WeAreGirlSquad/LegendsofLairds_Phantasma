using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryViewClimbZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LegendaryCore.io.ToggleClimbableCamera(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LegendaryCore.io.ToggleClimbableCamera(false);
        }
    }

}
