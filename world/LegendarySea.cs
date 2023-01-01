using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LegendarySea : MonoBehaviour
{
    AudioSource oceanAudio;
    [SerializeField] GameObject seasplash;
    [SerializeField] AudioClip splash_sfx;


    private void Awake()
    {
        oceanAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LegendaryPlayer>().Swim();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oceanAudio.clip = splash_sfx;
            oceanAudio.Play();
            GameObject splash = Instantiate(seasplash, other.transform.position, Quaternion.identity);
            //Debug.Log("pos:" + other.transform.position);
            other.gameObject.GetComponent<LegendaryPlayer>().SeaIn();
            StartCoroutine(UnloadMe(splash));
        }

        if (other.CompareTag("Pushable"))
        {
            oceanAudio.clip = splash_sfx;
            oceanAudio.Play();
            GameObject splash = Instantiate(seasplash, other.transform.position, Quaternion.identity);
            //Debug.Log("pos:" + other.transform.position);
            //other.gameObject.GetComponent<LegendaryPlayer>().SeaIn();
            //other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(SinkMe(other.gameObject));
            StartCoroutine(UnloadMe(splash));
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            other.gameObject.GetComponent<LegendaryPlayer>().SeaOut();


        }
    }

    IEnumerator SinkMe(GameObject go)
    {
        //other.GetComponent<BoxCollider>().enabled = false;
        go.transform.DOMoveY(-2f, 1.5f);
        Destroy(go, 1.8f);
        yield return null;
    }

    IEnumerator UnloadMe(GameObject go)
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(go);
    }
}
