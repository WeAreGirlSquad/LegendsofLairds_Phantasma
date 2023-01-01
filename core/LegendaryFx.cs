using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LegendaryVisualEffect
{
    GOLD_AURA,
    SLASH_1,
    SLASH_2,
    HIT_MONSTER_1,
    HIT_WHITE_SPARKS,
    PIERCE_1,
    GROUND_WIND,
    BIG_WIND,
    BEAM_GREEN,
    BEAM_YELLOW,
    GODRAY,
    NOVA_YELLOW,
    SMOKE,
    BLOOD_DEATH,
    FLARES
}

public class LegendaryFx : MonoBehaviour
{
    public static LegendaryFx io;
    public GameObject[] effects;


    private void Awake()
    {
        if (io == null)
        {
            io = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayEffect(LegendaryVisualEffect effectType, Transform fxtarget)
    {
        GameObject fx;

        switch (effectType)
        {
            case LegendaryVisualEffect.GOLD_AURA:
                fx = Instantiate(effects[0], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.SLASH_1:
                fx = Instantiate(effects[1], fxtarget.position, fxtarget.rotation);
                break;
            case LegendaryVisualEffect.SLASH_2:
                fx = Instantiate(effects[2], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.HIT_MONSTER_1:
                fx = Instantiate(effects[3], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.HIT_WHITE_SPARKS:
                fx = Instantiate(effects[4], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.PIERCE_1:
                fx = Instantiate(effects[5], fxtarget.position, fxtarget.rotation);
                break;
            case LegendaryVisualEffect.GROUND_WIND:
                fx = Instantiate(effects[6], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BIG_WIND:
                fx = Instantiate(effects[7], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BEAM_GREEN:
                fx = Instantiate(effects[8], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BEAM_YELLOW:
                fx = Instantiate(effects[9], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.GODRAY:
                fx = Instantiate(effects[10], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.NOVA_YELLOW:
                fx = Instantiate(effects[11], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.SMOKE:
                fx = Instantiate(effects[12], fxtarget.position, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BLOOD_DEATH:
                fx = Instantiate(effects[13], fxtarget.position, fxtarget.rotation);
                break;
            case LegendaryVisualEffect.FLARES:
                fx = Instantiate(effects[14], fxtarget.position, fxtarget.rotation);
                break;
        }
    }


    public void PlayEffect(LegendaryVisualEffect effectType, Vector3 location)
    {
        GameObject fx;

        switch(effectType)
        {
            case LegendaryVisualEffect.GOLD_AURA:
                fx = Instantiate(effects[0], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.SLASH_1:
                fx = Instantiate(effects[1], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.SLASH_2:
                fx = Instantiate(effects[2], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.HIT_MONSTER_1:
                fx = Instantiate(effects[3], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.HIT_WHITE_SPARKS:
                fx = Instantiate(effects[4], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.PIERCE_1:
                fx = Instantiate(effects[5], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.GROUND_WIND:
                fx = Instantiate(effects[6], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BIG_WIND:
                fx = Instantiate(effects[7], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BEAM_GREEN:
                fx = Instantiate(effects[8], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BEAM_YELLOW:
                fx = Instantiate(effects[9], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.GODRAY:
                fx = Instantiate(effects[10], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.NOVA_YELLOW:
                fx = Instantiate(effects[11], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.SMOKE:
                fx = Instantiate(effects[12], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.BLOOD_DEATH:
                fx = Instantiate(effects[13], location, Quaternion.identity);
                break;
            case LegendaryVisualEffect.FLARES:
                fx = Instantiate(effects[14], location, Quaternion.identity);
                break;
        }
    }



}
