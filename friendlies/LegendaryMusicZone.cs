using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryMusicZone : MonoBehaviour
{
    [SerializeField] LegendaryAudioType music;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LegendaryAudio.io.PlayMusic(music);
            LegendaryCore.io.SetMusicZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LegendaryCore.io.SetMusicZone(false);
            if (LegendaryCore.io.IsDaytimeNow())
            {
                LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_SAILING);
            } else
            {
                LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_NIGHT);
            }
        }
    }

}
