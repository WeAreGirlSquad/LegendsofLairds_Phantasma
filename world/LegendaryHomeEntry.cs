using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryHomeEntry : MonoBehaviour
{
    string playerHash = "Player";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerHash))
        {
            LegendaryCore.io.SwitchLayer(LegendaryLayer.INTERIOR);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerHash))
        {
            LegendaryCore.io.SwitchLayer(LegendaryLayer.EXTERIOR);
        }
    }


}
