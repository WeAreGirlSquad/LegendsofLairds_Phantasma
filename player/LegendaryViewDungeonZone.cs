using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryViewDungeonZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LegendaryCore.io.SwitchCamera(LegendaryView.CLOSE);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LegendaryCore.io.SwitchCamera(LegendaryView.NORMAL);
        }
    }

}
