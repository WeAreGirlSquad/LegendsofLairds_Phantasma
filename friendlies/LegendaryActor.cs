using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The LegendaryActor is the Battle Unit script
 * 
 * 
*/

public class LegendaryActor : MonoBehaviour
{
    public string actorName;
    public int actorLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int maxPh;
    public int currentPh;
    public LegendaryWeaponType weapon;
    public Sprite avatar;
    public bool isAlive = true;
    public int gender;
}
