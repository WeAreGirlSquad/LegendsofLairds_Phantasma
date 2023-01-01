using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleVictory : MonoBehaviour
{
    public void PlayStamp()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_VICTORY_STAMP);
    }
  
}
