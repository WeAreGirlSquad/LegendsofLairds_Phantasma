using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryBeast : MonoBehaviour
{
    [SerializeField] GameObject beastCamera;
    [SerializeField] Transform beastMouth;
    [SerializeField] GameObject fireBreath;
    GameObject attack;
    

    public void ToggleBeastCamera(bool toggle)
    {
        beastCamera.SetActive(toggle);
    }

    public void BreathFire()
    {
    
        attack = Instantiate(fireBreath, beastMouth);
    }

    public void StopAttack()
    {
        Destroy(attack);
    }

}
