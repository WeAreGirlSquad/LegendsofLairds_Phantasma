using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryMessage : MonoBehaviour
{
    [SerializeField] bool onExit = false;
    string player_hash = "Player";
    [SerializeField] string title;
    [TextArea(5, 5)] [SerializeField] string message;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(player_hash)) {

            if(!onExit)
            {
                LegendaryCore.io.ShowWorldMessage(title, message);
                GetComponent<BoxCollider>().enabled = false;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(player_hash)) {

            if (onExit)
            {
                LegendaryCore.io.ShowWorldMessage(title, message);
                GetComponent<BoxCollider>().enabled = false;
            }

        }
    }
}
