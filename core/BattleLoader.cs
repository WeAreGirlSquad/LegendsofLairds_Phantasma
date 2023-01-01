using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour
{

    public void BattleSequenceFinished()
    {
        gameObject.SetActive(false);
    }

}
