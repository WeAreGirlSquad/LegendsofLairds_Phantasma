using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryDialogue : MonoBehaviour
{
    [SerializeField] string text = "";
    // add avatar

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            LegendaryCore.io.Chat(text);
        }
    }


}
