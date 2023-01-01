using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendarySmithing : MonoBehaviour
{
    [SerializeField] bool inUse = false;
    GameObject currentUser;
    [SerializeField] Transform impactPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(inUse)
        {
            return;
        }

        if(!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if(other.gameObject.CompareTag("Player"))
        {
            LegendaryCore.io.RunTransition();
            inUse = true;
            currentUser = other.gameObject;
            currentUser.GetComponent<LegendaryPlayer>().Crafting(inUse, -transform.forward);
            transform.GetChild(0).gameObject.SetActive(true);
            LegendaryCore.io.ToggleWorldHud(false);
            LegendaryCore.io.ToggleCraftingMode(true);
            LegendaryCraft.io.SetImpactPoint(impactPoint);
            LegendaryCraft.io.SetupCrafting(LegendaryCraftingType.SMITHING);
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
            LegendaryCore.io.ToggleCraftingMode(false);
            LegendaryCore.io.ToggleWorldHud(true);

            other.gameObject.GetComponent<LegendaryPlayer>().DeactivateTool();
            LegendaryCore.io.FreeFromLock();
        }


    }
}
