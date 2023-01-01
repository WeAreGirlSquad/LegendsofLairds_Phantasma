using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryMount : MonoBehaviour
{

    float breath_cooldown = 0.0f;

    public void FootStep(int step)
    {
        switch (step)
        {
            case 1:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_HORSE_FOOTSTEP_1);
                break;
            case 2:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_HORSE_FOOTSTEP_2);
                break;
        }
    }

    public void AnimalSound(int index)
    {

        
        switch(index)
        {
            case 1:
                if(breath_cooldown < Time.time)
                {
                    LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_HORSE_BREATH);
                    breath_cooldown = Time.time + 2.456f;
                }
                
                break;
            case 2:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_HORSE_NEIGH);
                break;
        }
    }


}
