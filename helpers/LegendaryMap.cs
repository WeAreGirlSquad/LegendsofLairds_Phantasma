using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegendaryMap : MonoBehaviour
{
    [SerializeField] float playerZ;
    [SerializeField] RectTransform playerMarker;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerZ = LegendaryCore.io.GetLocalPlayer().rotation.eulerAngles.y;

        Vector3 eulerRotation = playerMarker.rotation.eulerAngles;
        playerMarker.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, -playerZ);

    }
}
