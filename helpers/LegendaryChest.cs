using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryChest : MonoBehaviour
{
    [SerializeField] GameObject weapon;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<LegendaryPlayer>().DeactivateWeapon();
        other.GetComponent<LegendaryPlayer>().ActivateWeapon(weapon.transform);
        gameObject.SetActive(false);
        //StartCoroutine(Unloader());
    }

    IEnumerator Unloader()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }

    // jingle 14
    // beam up ray
    // gold aura

}
