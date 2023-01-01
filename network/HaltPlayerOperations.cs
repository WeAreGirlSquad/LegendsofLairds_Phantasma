using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltPlayerOperations : MonoBehaviour
{
    private void OnEnable()
    {
        LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().InGameMenu(true);
        LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().InConsoleMode(true);
    }

    private void OnDisable()
    {
        LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().InGameMenu(false);
        LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().InConsoleMode(false);
    }
}
