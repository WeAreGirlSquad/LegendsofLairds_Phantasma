using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using QFSW.QC;
using QFSW.QC.Actions;

public enum LegendaryAudioType
{
    FOOTSTEP_LEFT_GRASS,
    FOOTSTEP_RIGHT_GRASS,
    FOOTSTEP_LEFT_STONE,
    FOOTSTEP_RIGHT_STONE,
    FOOTSTEP_LEFT_WATER,
    FOOTSTEP_RIGHT_WATER,
    SWIM_NORMAL,
    SWIM_FAST,
    JUMP,
    MALE_HURAH_1,
    MALE_HURAH_2,
    MALE_HURAH_3,
    FEMALE_HURAH_1,
    FEMALE_HURAH_2,
    FEMALE_HURAH_3,
    MALE_HIT_1,
    MALE_HIT_2,
    MALE_DEATH,
    FEMALE_HIT_1,
    FEMALE_HIT_2,
    FEMALE_DEATH,
    MALE_PUSH,
    MALE_PULL,
    FEMALE_PUSH,
    FEMALE_PULL,
    MALE_TIRED,
    FEMALE_TIRED,
    MALE_OK,
    FEMALE_OK,
    MALE_CANCEL,
    FEMALE_CANCEL,
    MALE_LEVEL_UP,
    FEMALE_LEVEL_UP,
    AMBIENT_LAND,
    AMBIENT_SEA,
    AMBIENT_ROCK,
    AMBIENT_DUNGEON,
    WATER_SPLASH,
    ITEM_WEAPON_PICKUP,
    WEAPON_SWING_1,
    BATTLE_THEME_FOE,
    BATTLE_THEME_GREATER_FOE,
    BATTLE_THEME_BOSS,
    BATTLE_VICTORY_INTRO,
    BATTLE_VICTORY_LOOP,
    BATTLE_DEFEAT,
    WORLD_THEME_SAILING,
    WORLD_THEME_FIELDS,
    BATTLE_LOADER_1,
    BATTLE_MONSTER_DEATH_DROP,
    BATTLE_VICTORY_STAMP,
    BATTLE_MENU_SELECT,
    BATTLE_MENU_CONFIRM,
    BATTLE_MENU_CANCEL,
    BATTLE_PLAYER_HURT,
    BATTLE_LOST,
    BASIC_MENU_SELECT,
    BASIC_MENU_CONFIRM,
    BASIC_MENU_CANCEL,
    WARP_SOUL,
    WARP_PHOENIX,
    ITEM_RECEIVED,
    ITEM_CRAFTING_COMPLETE,
    ITEM_CRAFTING_FAILED,
    ITEM_CRAFTING_HAMMER_STRIKE,
    ITEM_CRAFTING_COOKING,
    NOTIFICATION,
    HARVEST_WOOD,
    BLOOD_SQUISH,
    ARROW_RELEASE,
    WEAPON_RING_OPEN,
    WEAPON_RING_CLOSE,
    SHEATH,
    MENU_BUILD_NEXT,
    MENU_BUILD_PLACE,
    MOUNT_HORSE_FOOTSTEP_1,
    MOUNT_HORSE_FOOTSTEP_2,
    MOUNT_HORSE_NEIGH,
    MOUNT_HORSE_BREATH,
    MOUNT_EFFECT,
    GAMEMENU_ACTIVATE,
    GAMEMENU_DEACTIVATE,
    QUEST_UPDATE,
    TREASURE_RECEIVED,
    INVENTORY_ENTRY,
    WORLD_THEME_INTRO,
    START_GAME,
    CHARACTER_CREATE_THEME,
    HIT_BOX_1,
    CONSUME_FOOD,
    CONSUME_DRINK,
    DUNGEON_TRAP_GORE,
    WATER_STEP,
    WATER_STEP_2,
    WORLD_THEME_NIGHT,
    WEAPON_EQUIP,
}

public class LegendaryAudio : MonoBehaviour
{
    public static LegendaryAudio io;
    
    public AudioClip[] audioclips;
    [SerializeField] AudioMixer legendaryAudio;
    

    private void Awake()
    {
        if(io == null)
        {
            io = this;
        } 
        else
        {
            Destroy(this);
        }
    }

    public void PlayAmbient()
    {

    }

    public void CinemaCrossFade(bool toMovie)
    {
        if (toMovie)
        {
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerWorld", 1f, 0f));
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerCinema", 1f, 0.45f));
        }
        else
        {
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerWorld", 1f, 0.45f));
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerCinema", 1f, 0f));
        }
    }

    public void BattleCrossfade(bool toBattle)
    {
        if(toBattle)
        {
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerWorld", 1f, 0f));
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerBattle", 1f, 0.45f));
        } else
        {
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerWorld", 1f, 0.45f));
            StartCoroutine(FadeMixerGroup.StartFade(legendaryAudio, "AudioMixerBattle", 1f, 0f));
        }
    }

    public void PlayBattleTheme(LegendaryAudioType theme)
    {
        switch (theme)
        {
            case LegendaryAudioType.BATTLE_THEME_FOE:
                LegendaryCore.io.PlayBattleMusic(audioclips[5]);
                break;
            case LegendaryAudioType.BATTLE_THEME_GREATER_FOE:
                LegendaryCore.io.PlayBattleMusic(audioclips[54]);
                break;
            case LegendaryAudioType.BATTLE_VICTORY_LOOP:
                LegendaryCore.io.PlayBattleMusic(audioclips[6]);
                break;
            case LegendaryAudioType.BATTLE_VICTORY_INTRO:
                LegendaryCore.io.PlayBattleMusic(audioclips[7]);
                break;
            case LegendaryAudioType.BATTLE_LOST:
                LegendaryCore.io.PlayBattleMusic(audioclips[19]);
                break;
        }
    }

    [Command]
    public void PlayMusic(LegendaryAudioType theme)
    {
        switch(theme)
        {
            case LegendaryAudioType.WORLD_THEME_FIELDS:
                LegendaryCore.io.PlayWorldMusic(audioclips[3]);
                break;
            case LegendaryAudioType.WORLD_THEME_SAILING:
                LegendaryCore.io.PlayWorldMusic(audioclips[4]);
                break;
            case LegendaryAudioType.WORLD_THEME_INTRO:
                LegendaryCore.io.PlayWorldMusic(audioclips[42]);
                break;
            case LegendaryAudioType.CHARACTER_CREATE_THEME:
                LegendaryCore.io.PlayWorldMusic(audioclips[44]);
                break;
            case LegendaryAudioType.BATTLE_LOST:
                LegendaryCore.io.PlayWorldMusic(audioclips[19]);
                break;
            case LegendaryAudioType.WORLD_THEME_NIGHT:
                LegendaryCore.io.PlayWorldMusic(audioclips[55]);
                break;
        }
    }

    [Command]
    public void PlaySfx(LegendaryAudioType audiosfx)
    {
        switch(audiosfx)
        {
            case LegendaryAudioType.WATER_SPLASH:
                LegendaryCore.io.PlaySfx2(audioclips[0]);
                break;
            case LegendaryAudioType.ITEM_WEAPON_PICKUP:
                LegendaryCore.io.PlaySfx2(audioclips[1]);
                break;
            case LegendaryAudioType.WEAPON_SWING_1:
                LegendaryCore.io.PlaySfx3(audioclips[2]);
                break;
            case LegendaryAudioType.BATTLE_LOADER_1:
                LegendaryCore.io.PlaySfx4(audioclips[9]);
                break;
            case LegendaryAudioType.FEMALE_HURAH_1:
                //LegendaryCore.io.PlaySfx4(audioclips[10]);
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 0)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[10]);
                }
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 1)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[47]);
                }
                break;
            case LegendaryAudioType.FEMALE_HURAH_2:
                //LegendaryCore.io.PlaySfx4(audioclips[11]);
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 0)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[11]);
                }
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 1)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[48]);
                }
                break;
            case LegendaryAudioType.FEMALE_HIT_1:
                //LegendaryCore.io.PlaySfx4(audioclips[12]);
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 0)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[12]);
                }
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 1)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[45]);
                }
                break;
            case LegendaryAudioType.BATTLE_MONSTER_DEATH_DROP:
                LegendaryCore.io.PlaySfx4(audioclips[13]);
                break;
            case LegendaryAudioType.BATTLE_VICTORY_STAMP:
                LegendaryCore.io.PlaySfx4(audioclips[14]);
                break;
            case LegendaryAudioType.BATTLE_MENU_SELECT:
                LegendaryCore.io.PlaySfx4(audioclips[15]);
                break;
            case LegendaryAudioType.BATTLE_MENU_CONFIRM:
                LegendaryCore.io.PlaySfx4(audioclips[16]);
                break;
            case LegendaryAudioType.BATTLE_MENU_CANCEL:
                LegendaryCore.io.PlaySfx4(audioclips[17]);
                break;
            case LegendaryAudioType.BATTLE_PLAYER_HURT:
                if(LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 0)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[18]);
                }
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 1)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[45]);
                }
                break;
            case LegendaryAudioType.FEMALE_PUSH:
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 0)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[20]);
                }
                if (LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryActor>().gender == 1)
                {
                    LegendaryCore.io.PlaySfx3(audioclips[46]);
                }
                //LegendaryCore.io.PlaySfx4(audioclips[20]);
                break;
            case LegendaryAudioType.WARP_SOUL:
                LegendaryCore.io.PlaySfx4(audioclips[21]);
                break;
            case LegendaryAudioType.WARP_PHOENIX:
                LegendaryCore.io.PlaySfx4(audioclips[22]);
                break;
            case LegendaryAudioType.ITEM_RECEIVED:
                LegendaryCore.io.PlaySfx3(audioclips[1]);
                break;
            case LegendaryAudioType.BASIC_MENU_SELECT:
                LegendaryCore.io.PlaySfx4(audioclips[17]);
                break;
            case LegendaryAudioType.BASIC_MENU_CONFIRM:
                LegendaryCore.io.PlaySfx4(audioclips[17]);
                break;
            case LegendaryAudioType.BASIC_MENU_CANCEL:
                LegendaryCore.io.PlaySfx4(audioclips[17]);
                break;
            case LegendaryAudioType.ITEM_CRAFTING_HAMMER_STRIKE:
                LegendaryCore.io.PlaySfx4(audioclips[23]);
                break;
            case LegendaryAudioType.ITEM_CRAFTING_COOKING:
                LegendaryCore.io.PlaySfx4(audioclips[24]);
                break;
            case LegendaryAudioType.ITEM_CRAFTING_FAILED:
                LegendaryCore.io.PlaySfx4(audioclips[26]);
                break;
            case LegendaryAudioType.NOTIFICATION:
                LegendaryCore.io.PlaySfx4(audioclips[25]);
                break;
            case LegendaryAudioType.HARVEST_WOOD:
                LegendaryCore.io.PlaySfx4(audioclips[27]);
                break;
            case LegendaryAudioType.BLOOD_SQUISH:
                LegendaryCore.io.PlaySfx4(audioclips[28]);
                break;
            case LegendaryAudioType.ARROW_RELEASE:
                LegendaryCore.io.PlaySfx3(audioclips[29]);
                break;
            case LegendaryAudioType.WEAPON_RING_OPEN:
                LegendaryCore.io.PlaySfx4(audioclips[25]);
                break;
            case LegendaryAudioType.WEAPON_RING_CLOSE:
                LegendaryCore.io.PlaySfx4(audioclips[2]);
                break;
            case LegendaryAudioType.SHEATH:
                LegendaryCore.io.PlaySfx4(audioclips[32]);
                break;
            case LegendaryAudioType.MENU_BUILD_NEXT:
                LegendaryCore.io.PlaySfx4(audioclips[33]);
                break;
            case LegendaryAudioType.MENU_BUILD_PLACE:
                LegendaryCore.io.PlaySfx4(audioclips[34]);
                break;
            case LegendaryAudioType.MOUNT_HORSE_FOOTSTEP_1:
                LegendaryCore.io.PlaySfx2(audioclips[35]);
                break;
            case LegendaryAudioType.MOUNT_HORSE_FOOTSTEP_2:
                LegendaryCore.io.PlaySfx2(audioclips[36]);
                break;
            case LegendaryAudioType.MOUNT_HORSE_BREATH:
                LegendaryCore.io.PlaySfx2(audioclips[37]);
                break;
            case LegendaryAudioType.MOUNT_HORSE_NEIGH:
                LegendaryCore.io.PlaySfx2(audioclips[38]);
                break;
            case LegendaryAudioType.MOUNT_EFFECT:
                LegendaryCore.io.PlaySfx3(audioclips[39]);
                break;
            case LegendaryAudioType.GAMEMENU_ACTIVATE:
                LegendaryCore.io.PlaySfx3(audioclips[31]);
                break;
            case LegendaryAudioType.GAMEMENU_DEACTIVATE:
                LegendaryCore.io.PlaySfx3(audioclips[30]);
                break;
            case LegendaryAudioType.QUEST_UPDATE:
                LegendaryCore.io.PlaySfx3(audioclips[21]);
                break;
            case LegendaryAudioType.TREASURE_RECEIVED:
                LegendaryCore.io.PlaySfx3(audioclips[40]);
                break;
            case LegendaryAudioType.INVENTORY_ENTRY:
                LegendaryCore.io.PlaySfx3(audioclips[41]);
                break;
            case LegendaryAudioType.START_GAME:
                LegendaryCore.io.PlaySfx3(audioclips[43]);
                break;
            case LegendaryAudioType.HIT_BOX_1:
                LegendaryCore.io.PlaySfx2(audioclips[28]);
                break;
            case LegendaryAudioType.CONSUME_FOOD:
                LegendaryCore.io.PlaySfx3(audioclips[50]);
                break;
            case LegendaryAudioType.CONSUME_DRINK:
                LegendaryCore.io.PlaySfx3(audioclips[51]);
                break;
            case LegendaryAudioType.DUNGEON_TRAP_GORE:
                LegendaryCore.io.PlaySfx2(audioclips[52]);
                break;
            case LegendaryAudioType.WATER_STEP:
                LegendaryCore.io.PlaySfx2(audioclips[53]);
                break;
            case LegendaryAudioType.WATER_STEP_2:
                LegendaryCore.io.PlaySfx2(audioclips[57]);
                break;
            case LegendaryAudioType.WEAPON_EQUIP:
                LegendaryCore.io.PlaySfx3(audioclips[56]);
                break;
        }



    }



}
