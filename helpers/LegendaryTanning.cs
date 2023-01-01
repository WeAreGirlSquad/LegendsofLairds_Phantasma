using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryTanning : MonoBehaviour
{
    [SerializeField] bool inUse = false;
    GameObject currentUser;
    [SerializeField] Transform impactPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (inUse)
        {
            return;
        }

        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            LegendaryCore.io.RunTransition();
            inUse = true;
            currentUser = other.gameObject;
            currentUser.GetComponent<LegendaryPlayer>().Tanning(inUse, transform.forward);
            transform.GetChild(0).gameObject.SetActive(true);
            LegendaryCore.io.ToggleWorldHud(false);
            LegendaryCore.io.ToggleTanningMode(true);
            LegendaryCraft.io.SetImpactPoint(impactPoint);
            LegendaryCraft.io.SetupCrafting(LegendaryCraftingType.TANNING);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LegendaryCore.io.RunTransition();
            if (inUse)
            {
                inUse = false;

            }
            transform.GetChild(0).gameObject.SetActive(false);
            LegendaryCore.io.ToggleTanningMode(false);
            LegendaryCore.io.ToggleWorldHud(true);

            other.gameObject.GetComponent<LegendaryPlayer>().DeactivateTool();
            LegendaryCore.io.FreeFromLock();
        }

    }
}

