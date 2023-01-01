using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryHomeView : MonoBehaviour
{

    [SerializeField] GameObject homeCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            homeCamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            homeCamera.SetActive(false);
        }
    }
}
