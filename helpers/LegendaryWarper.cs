using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryWarper : MonoBehaviour
{
    [SerializeField] Transform destination;
    GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<LegendaryPlayer>().Mount(false);
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.WARP_PHOENIX);
            LegendaryCore.io.WarpObject(player, destination);
            //other.gameObject.transform.position = destination.position;
        }
    }



}
