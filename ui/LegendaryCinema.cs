using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using QFSW.QC;
using QFSW.QC.Actions;
public enum LEGENDARY_CUTSCENE
{
    NONE,
    OLD_TRAILER
}



public class LegendaryCinema : MonoBehaviour
{
    public static LegendaryCinema io;
    [SerializeField] GameObject cinemaObject;
    [SerializeField] CanvasGroup cinemaCanvasGroup;
    [SerializeField] VideoPlayer legendaryVideoPlayer;
    [SerializeField] VideoClip[] videos;

    Transform localPlayer;

    private void Awake()
    {
        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }
    }

    [Command]
    public void PlayCinematic(LEGENDARY_CUTSCENE cutscene)
    {
        localPlayer = LegendaryCore.io.GetLocalPlayer();
        cinemaObject.SetActive(true);
        LegendaryCore.io.ToggleWorldHud(false);
        switch (cutscene)
        {
            case LEGENDARY_CUTSCENE.OLD_TRAILER:
                legendaryVideoPlayer.clip = videos[0];
                break;
        }

        legendaryVideoPlayer.loopPointReached += EndofPlayback;

        StartCoroutine(StartPlayback());
    }

    void EndofPlayback(VideoPlayer vp)
    {
        StartCoroutine(StopPlayBack());
    }

    IEnumerator StopPlayBack()
    {
        legendaryVideoPlayer.Stop();
        LegendaryAudio.io.CinemaCrossFade(false);
        cinemaCanvasGroup.DOFade(0.0f, 0.4f);
        yield return new WaitForSeconds(0.4f);
        cinemaObject.SetActive(false);
        LegendaryCore.io.ToggleWorldHud(true);
        localPlayer.GetComponent<LegendaryPlayer>().enabled = true;
        //Time.timeScale = 1.0f;
        yield return null;
    }

    IEnumerator StartPlayback()
    {
        legendaryVideoPlayer.Prepare();
        while(!legendaryVideoPlayer.isPrepared)
        {
            yield return null;
        }
        //Time.timeScale = 0.0f;
        localPlayer.GetComponent<LegendaryPlayer>().enabled = false;
        cinemaCanvasGroup.DOFade(1.0f, 0.4f);
        LegendaryAudio.io.CinemaCrossFade(true);
        yield return new WaitForSeconds(0.3f);
        legendaryVideoPlayer.Play();
        yield return null;
    }

}
