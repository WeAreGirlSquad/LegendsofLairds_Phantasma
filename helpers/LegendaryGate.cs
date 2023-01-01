using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.WARP_SOUL);
        }
    }
}
