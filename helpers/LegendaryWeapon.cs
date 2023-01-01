using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryWeapon : MonoBehaviour
{
    [SerializeField] LegendaryWeaponType weaponType;

    public LegendaryWeaponType GetWeaponType()
    {
        return weaponType;
    }
}
