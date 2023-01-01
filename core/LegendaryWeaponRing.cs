using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegendaryWeaponRing : MonoBehaviour
{
    /*
    AXE = 0
    SWORD = 1
    DAGGER = 2
    STAFF = 3
    HAMMER = 4
    BOW = 5
    SPEAR = 6
     */

    public static LegendaryWeaponRing io;

    public LegendaryWeaponRingItem[] weaponRingItems;

    [SerializeField] Transform slotAxe;
    [SerializeField] Transform slotSword;
    [SerializeField] Transform slotDagger;
    [SerializeField] Transform slotStaff;
    [SerializeField] Transform slotHammer;
    [SerializeField] Transform slotBow;
    [SerializeField] Transform slotSpear;



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

    private void Start()
    {
        LegendaryCore.io.UpdateWeaponRing();
    }

    public void WeaponRingInitialize()
    {
        
    }

    public Sprite GetWeaponSprite(int weaponRingItemIndex)
    {
        return weaponRingItems[weaponRingItemIndex].thumb;
    }


    public Transform SpawnWeapon(int weaponRingItemIndex)
    {
        //Debug.Log("new go");
        GameObject go = null;
        //Debug.Log("created");

        switch (weaponRingItemIndex)
        {
            case 1:
                go = Instantiate(weaponRingItems[1].prefab);
                break;
            case 0:
                go = Instantiate(weaponRingItems[0].prefab);
                break;
            case 4:
                go = Instantiate(weaponRingItems[4].prefab);
                break;
            case 3:
                go = Instantiate(weaponRingItems[3].prefab);
                break;
            case 6:
                go = Instantiate(weaponRingItems[6].prefab);
                break;
            case 2:
                go = Instantiate(weaponRingItems[2].prefab);
                break;
            case 5:
                go = Instantiate(weaponRingItems[5].prefab);
                break;
        }


        return go.transform;
    }


    public void WeaponRingDisable()
    {
        gameObject.SetActive(false);
    }



    public LegendaryWeaponRingItem GetWeaponItemFromRing(LegendaryWeaponType weaponType)
    {
        LegendaryWeaponRingItem returnRingItem = new LegendaryWeaponRingItem();

        switch(weaponType)
        {
            case LegendaryWeaponType.SWORD_1H:
                if(weaponRingItems[1] != null)
                {
                    returnRingItem = weaponRingItems[1];
                }
                break;
            case LegendaryWeaponType.AXE_1H:
                if (weaponRingItems[0] != null)
                {
                    returnRingItem = weaponRingItems[0];
                }
                break;
            case LegendaryWeaponType.HAMMER:
                if (weaponRingItems[4] != null)
                {
                    returnRingItem = weaponRingItems[4];
                }
                break;
            case LegendaryWeaponType.STAFF:
                if (weaponRingItems[3] != null)
                {
                    returnRingItem = weaponRingItems[3];
                }
                break;
            case LegendaryWeaponType.SPEAR:
                if (weaponRingItems[6] != null)
                {
                    returnRingItem = weaponRingItems[6];
                }
                break;
            case LegendaryWeaponType.DAGGER:
                if (weaponRingItems[2] != null)
                {
                    returnRingItem = weaponRingItems[2];
                }
                break;
            case LegendaryWeaponType.BOW:
                if (weaponRingItems[5] != null)
                {
                    returnRingItem = weaponRingItems[5];
                }
                break;
        }

        return returnRingItem;

    }

    public void RemoveWeaponFromRing(int index)
    {
        switch(index)
        {
            case 1:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.SWORD_1H, false);
                weaponRingItems[1] = null;
                break;
            case 0:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.AXE_1H, false);
                weaponRingItems[0] = null;
                break;
            case 4:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.HAMMER, false);
                weaponRingItems[4] = null;
                break;
            case 3:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.SPEAR, false);
                weaponRingItems[3] = null;
                break;
            case 6:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.SPEAR, false);
                weaponRingItems[6] = null;
                break;
            case 2:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.DAGGER, false);
                weaponRingItems[2] = null;
                break;
            case 5:
                LegendaryCore.io.SetWeaponAvailability(LegendaryWeaponType.BOW, false);
                weaponRingItems[5] = null;
                break;
        }
    }

    public void AddWeaponToRing(LegendaryWeaponRingItem weapon)
    {
        switch (weapon.itemType)
        {
            case LegendaryWeaponType.SWORD_1H:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[1] = weapon;
                break;
            case LegendaryWeaponType.AXE_1H:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[0] = weapon;
                break;
            case LegendaryWeaponType.HAMMER:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[4] = weapon;
                break;
            case LegendaryWeaponType.STAFF:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[3] = weapon;
                break;
            case LegendaryWeaponType.SPEAR:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[6] = weapon;
                break;
            case LegendaryWeaponType.DAGGER:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[2] = weapon;
                break;
            case LegendaryWeaponType.BOW:
                LegendaryCore.io.SetWeaponAvailability(weapon.itemType, true);
                weaponRingItems[5] = weapon;
                break;
        }
    }

}
