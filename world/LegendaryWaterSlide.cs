using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryWaterSlide : MonoBehaviour
{
    [SerializeField] bool slidePath = true;

    private Transform currentObject = null;
    void Update()
    {
        if(slidePath && currentObject != null)
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(slidePath)
            {
                other.GetComponent<LegendaryPlayer>().Slide(true, transform);
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_SPLASH);
            }

            if(currentObject == null)
            {
                currentObject = other.transform;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(slidePath)
            {
                other.GetComponent<LegendaryPlayer>().Slide(false, transform);
                currentObject = null;
                other.GetComponent<LegendaryPlayer>().FallDown();
            } else
            {
                other.GetComponent<LegendaryPlayer>().FallDown();
            }
            
        }
    }


}
