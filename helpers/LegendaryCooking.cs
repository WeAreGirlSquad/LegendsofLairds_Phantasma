using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryCooking : MonoBehaviour
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
            currentUser.GetComponent<LegendaryPlayer>().Cooking(inUse, -transform.forward);
            transform.GetChild(0).gameObject.SetActive(true);
            LegendaryCore.io.ToggleWorldHud(false);
            LegendaryCore.io.ToggleCookingMode(true);
            LegendaryCraft.io.SetImpactPoint(impactPoint);
            LegendaryCraft.io.SetupCrafting(LegendaryCraftingType.COOKING);
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
            LegendaryCore.io.ToggleCookingMode(false);
            LegendaryCore.io.ToggleWorldHud(true);

            other.gameObject.GetComponent<LegendaryPlayer>().DeactivateTool();
            LegendaryCore.io.FreeFromLock();
        }

    }
}
