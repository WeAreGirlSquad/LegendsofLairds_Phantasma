using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.DUNGEON_TRAP_GORE);
            LegendaryCore.io.KillPlayerByTrap();
            gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }
}
