using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class LegendaryGo : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform buttonGo;

    public void Confirm()
    {
        canvasGroup.DOFade(0f, 0.5f);
        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        yield return new WaitForSeconds(0.5f);
        LegendsofLairds_BIOS.io.Boot();
    }


}
