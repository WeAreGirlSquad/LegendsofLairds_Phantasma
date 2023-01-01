using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using QFSW.QC;
using QFSW.QC.Actions;

public enum LegendaryBattleState
{
    AVAILABLE,
    START,
    WAIT_FOR_PLAYER,
    PLAYER_TURN,
    ENEMY_TURN,
    WON,
    LOST
} 

public enum LegendaryBattleMenu
{
    NONE,
    MENU,
    SUBMENU,
    SELECTOR
}

public enum LegendaryBattleAction
{
    IRON,
    MUSCLE,
    HEART,
}

public enum LegendaryBattleSkill
{
    IRON_ATTACK,
    IRON_ATTACK_ACCURATE,
    IRON_ATTACK_CRITICAL,
    MUSCLE_BLOCK,
    MUSCLE_DODGE,
    MUSCLE_RICOCHET,
    PHA_ATTACK,
    PHA_PROTECT,
    PHA_HEAL
}


public class LegendaryBattle : MonoBehaviour
{
    /*
     (battle ground)
     Constist of minimal 6 placement units (Vector3)

        in the center for short reach ([2],[3]) SWORD_1H, AXE_1H, SWORD_2H, AXE_2H
        at the end is for ranged weapons/pha ([0],[5]) BOW, PHA
        at the middle for medium reach ([1], [4]) SPEAR, STAFF, HAMMER
    */
    [SerializeField] RectTransform canvasRect;
    [SerializeField] LegendaryBattleState state;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject enemyObject;
    Vector3 battleArenaCenter;
    [SerializeField] GameObject battleCamera;
    [SerializeField] GameObject victoryCamera;
    [SerializeField] GameObject defeatCamera;
    [SerializeField] GameObject battlePP;
    [SerializeField] Transform battleVictory;
    [SerializeField] Transform battleDefeat;
    [SerializeField] Transform battleHud;
    LegendaryActor playerStats;
    LegendaryActor enemyStats;
    [SerializeField] Transform BattleMenu;
    [SerializeField] Transform BattleSubMenu;
    [SerializeField] Transform BattleEnemySelector;

    [SerializeField] RectTransform BattleMenu_option1;
    [SerializeField] RectTransform BattleMenu_option2;
    [SerializeField] RectTransform BattleMenu_option3;

    [SerializeField] RectTransform BattleSubMenu_option1;
    [SerializeField] RectTransform BattleSubMenu_option2;
    [SerializeField] RectTransform BattleSubMenu_option3;

    [SerializeField] TextMeshProUGUI BattleSubMenu_option1_text;
    [SerializeField] TextMeshProUGUI BattleSubMenu_option2_text;
    [SerializeField] TextMeshProUGUI BattleSubMenu_option3_text;

    [SerializeField] Image enemyHp;
    [SerializeField] Image playerHp;
    [SerializeField] Image playerPha;
    [SerializeField] TextMeshProUGUI playerHpAmount;
    [SerializeField] TextMeshProUGUI playerPhaAmount;
    [SerializeField] Image enemyAvatar;
    [SerializeField] Image playerAvatar;
    [SerializeField] Image playerWeapon;
    [SerializeField] TextMeshProUGUI playerWeaponDescription;

    // Multi Battle
    [SerializeField] Image enemy1_Hp;
    [SerializeField] Image enemy1_Avatar;
    [SerializeField] Image enemy2_Hp;
    [SerializeField] Image enemy2_Avatar;
    [SerializeField] Image enemy3_Hp;
    [SerializeField] Image enemy3_Avatar;
    LegendaryActor enemy1_Stats;
    LegendaryActor enemy2_Stats;
    LegendaryActor enemy3_Stats;
    [SerializeField] GameObject enemy1_Icon;
    [SerializeField] GameObject enemy2_Icon;
    [SerializeField] GameObject enemy3_Icon;


    List<GameObject> multibattle_enemies;
    Transform currentMultiBattleEnemy;



    [SerializeField] Image battleVictorySprite;
    [SerializeField] Image battleVictorySpriteShadow;
    [SerializeField] TextMeshProUGUI battleVictoryName;
    [SerializeField] TextMeshProUGUI battleVictoryLevel;

    [SerializeField] TextMeshProUGUI battleVictoryXpLarge;
    [SerializeField] TextMeshProUGUI battleVictoryXp;
    [SerializeField] TextMeshProUGUI battleVictoryKcal;
    [SerializeField] TextMeshProUGUI battleVictoryReward;
    [SerializeField] GameObject go_battleVictoryBonusXP;
    [SerializeField] TextMeshProUGUI battleVictoryBonusXP;

    float beginBattle_time = 0.0f;
    float endbattle_time = 0.0f;

    LegendaryBattleAction currentBattleAction;
    LegendaryBattleSkill currentBattleSkill;

    [SerializeField] DishookItem discordBattles;

    private int currentSelection = 0;
    private int lastSelection = 0;
    LegendaryBattleMenu currentMenu = LegendaryBattleMenu.NONE;

    int anim_battle_atk = Animator.StringToHash("Battle_ATK_0");
    int anim_battle_atk_accurate = Animator.StringToHash("Battle_ATK_1");
    int anim_battle_atk_critical = Animator.StringToHash("Battle_ATK_2");
    int anim_battle_msc_block = Animator.StringToHash("Battle_MSC_0");
    int anim_battle_msc_dodge = Animator.StringToHash("Battle_MSC_1");
    int anim_battle_msc_riposte = Animator.StringToHash("Battle_MSC_2");
    int anim_battle_pha_blast = Animator.StringToHash("Battle_BLAST_0");
    int anim_battle_pha_cast = Animator.StringToHash("Battle_CAST_1");
    int anim_battle_pha_heal = Animator.StringToHash("Battle_HEAL_2");
    int anim_battle_miss = Animator.StringToHash("Battle_MISS");
    int anim_battle_hit = Animator.StringToHash("Battle_HIT_0");
    int anim_battle_die = Animator.StringToHash("Battle_DIE");
    int anim_battle_def = Animator.StringToHash("Battle_DEF_0");
    int anim_battle_victory = Animator.StringToHash("Battle_VICTORY");
    int anim_battle_revive = Animator.StringToHash("Battle_REVIVE");

    public static LegendaryBattle io;

    float inputCooldown = 0f;

    private void OnEnable()
    {
        currentSelection = 0;
        multibattle_enemies = new List<GameObject>();
    }

    void SwitchSelection(int direction)
    {
        if(direction != 0)
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_SELECT);
        }
        
        currentSelection -= direction;
        if(currentSelection < 0)
        {
            currentSelection = 2;
        }
        if(currentSelection > 2)
        {
            currentSelection = 0;
        }

        if(currentMenu == LegendaryBattleMenu.MENU)
        {
            BattleMenu_option1.parent.GetComponent<Image>().enabled = false;
            BattleMenu_option2.parent.GetComponent<Image>().enabled = false;
            BattleMenu_option3.parent.GetComponent<Image>().enabled = false;

            switch(currentSelection)
            {
                case 0:
                    BattleMenu_option1.parent.GetComponent<Image>().enabled = true;
                    currentBattleAction = LegendaryBattleAction.IRON;
                    BattleSubMenu_option1_text.text = "Strike";
                    BattleSubMenu_option2_text.text = "Perfect Aim";
                    BattleSubMenu_option3_text.text = "Charge";
                    break;
                case 1:
                    BattleMenu_option2.parent.GetComponent<Image>().enabled = true;
                    currentBattleAction = LegendaryBattleAction.MUSCLE;
                    BattleSubMenu_option1_text.text = "Block";
                    BattleSubMenu_option2_text.text = "Dodge";
                    BattleSubMenu_option3_text.text = "Vengeance";
                    break;
                case 2:
                    currentBattleAction = LegendaryBattleAction.HEART;
                    BattleSubMenu_option1_text.text = "Energy Blast";
                    BattleSubMenu_option2_text.text = "Spirit Shield";
                    BattleSubMenu_option3_text.text = "Healing Wind";
                    BattleMenu_option3.parent.GetComponent<Image>().enabled = true;
                    break;
            }

        }

        if (currentMenu == LegendaryBattleMenu.SUBMENU)
        {
            BattleSubMenu_option1.parent.GetComponent<Image>().enabled = false;
            BattleSubMenu_option2.parent.GetComponent<Image>().enabled = false;
            BattleSubMenu_option3.parent.GetComponent<Image>().enabled = false;

            switch (currentSelection)
            {
                case 0:
                    BattleSubMenu_option1.parent.GetComponent<Image>().enabled = true;
                    switch(currentBattleAction)
                    {
                        case LegendaryBattleAction.IRON:
                            currentBattleSkill = LegendaryBattleSkill.IRON_ATTACK;
                            break;
                        case LegendaryBattleAction.MUSCLE:
                            currentBattleSkill = LegendaryBattleSkill.MUSCLE_BLOCK;
                            break;
                        case LegendaryBattleAction.HEART:
                            currentBattleSkill = LegendaryBattleSkill.PHA_ATTACK;
                            break;
                    }
                    break;
                case 1:
                    BattleSubMenu_option2.parent.GetComponent<Image>().enabled = true;
                    switch (currentBattleAction)
                    {
                        case LegendaryBattleAction.IRON:
                            currentBattleSkill = LegendaryBattleSkill.IRON_ATTACK_ACCURATE;
                            break;
                        case LegendaryBattleAction.MUSCLE:
                            currentBattleSkill = LegendaryBattleSkill.MUSCLE_DODGE;
                            break;
                        case LegendaryBattleAction.HEART:
                            currentBattleSkill = LegendaryBattleSkill.PHA_PROTECT;
                            break;
                    }
                    break;
                case 2:
                    BattleSubMenu_option3.parent.GetComponent<Image>().enabled = true;
                    switch (currentBattleAction)
                    {
                        case LegendaryBattleAction.IRON:
                            currentBattleSkill = LegendaryBattleSkill.IRON_ATTACK_CRITICAL;
                            break;
                        case LegendaryBattleAction.MUSCLE:
                            currentBattleSkill = LegendaryBattleSkill.MUSCLE_RICOCHET;
                            break;
                        case LegendaryBattleAction.HEART:
                            currentBattleSkill = LegendaryBattleSkill.PHA_HEAL;
                            break;
                    }
                    break;
            }

        }

        //Debug.Log("selection value" + currentSelection);

        
    }

    void SetSelectorOnEnemy()
    {
        if(currentBattleSkill == LegendaryBattleSkill.PHA_PROTECT || currentBattleSkill == LegendaryBattleSkill.PHA_HEAL)
        {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(playerObject.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
            BattleEnemySelector.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        } else
        {
            if(enemyObject != null)
            {
                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(enemyObject.transform.position);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
                BattleEnemySelector.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
            } else
            {
                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(currentMultiBattleEnemy.transform.position);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
                BattleEnemySelector.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
            }
            
        }


    }

    void ConfirmSelection()
    {
        if(currentMenu != LegendaryBattleMenu.NONE)
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_CONFIRM);
            

            switch(currentMenu)
            {
                case LegendaryBattleMenu.MENU:
                    currentMenu = LegendaryBattleMenu.SUBMENU;
                    currentSelection = 0;
                    SwitchSelection(0);
                    BattleSubMenu.gameObject.SetActive(true);
                    break;
                case LegendaryBattleMenu.SUBMENU:
                    currentMenu = LegendaryBattleMenu.SELECTOR;
                    SetSelectorOnEnemy();
                    BattleEnemySelector.gameObject.SetActive(true);
                    break;
                case LegendaryBattleMenu.SELECTOR:
                    BattleSubMenu.gameObject.SetActive(false);
                    BattleEnemySelector.gameObject.SetActive(false);
                    BattleMenu.gameObject.SetActive(false);
                    currentMenu = LegendaryBattleMenu.NONE;
                    state = LegendaryBattleState.PLAYER_TURN;
                    break;
            }
        }
        
    }

    void CancelSelection()
    {
        if(currentMenu == LegendaryBattleMenu.SUBMENU)
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_CANCEL);
            BattleSubMenu.gameObject.SetActive(false);
            currentMenu = LegendaryBattleMenu.MENU;
        }
        if(currentMenu == LegendaryBattleMenu.SELECTOR)
        {
            BattleEnemySelector.gameObject.SetActive(false);
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_CANCEL);
            BattleSubMenu.gameObject.SetActive(true);
            currentMenu = LegendaryBattleMenu.SUBMENU;

        }
        
    }

    [Command]
    public void KillPlayerInBattle()
    {
        StartCoroutine(BeginBattleLost());
    }

    public void BattleInput(bool action, bool action2, float movement )
    {
        if(!BattleMenu.gameObject.activeSelf)
        {
            return;
        }

        if(inputCooldown > Time.time)
        {
            return;
        }
        else
        {

            if (action)
            {
                ConfirmSelection();
                inputCooldown = Time.time + 0.12f;
                return;
            }

            if(action2)
            {
                CancelSelection();
                inputCooldown = Time.time + 0.12f;
                return;
            }


            if(!action && !action2 && currentMenu != LegendaryBattleMenu.NONE && currentMenu != LegendaryBattleMenu.SELECTOR)
            {
                int s = (int)movement;
                SwitchSelection(s);
                inputCooldown = Time.time + 0.12f;
            }
     
        }
    }

    
    public void CreateMultiBattle(GameObject[] enemies, GameObject player, bool sequence)
    {
        // Max. three enemies
        go_battleVictoryBonusXP.SetActive(false);
        player.GetComponent<LegendaryPlayer>().Mount(false);
        LegendaryCore.io.WeaponRingDisable();
        LegendaryCore.io.ToggleWorldHud(false);
        LegendaryCore.io.ToggleMiniMap(false);
        LegendaryCore.io.SwitchCamera(LegendaryView.NORMAL);
        LegendaryCore.io.BattleSequenceBegin();
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_LOADER_1);
        playerObject = player;
        multibattle_enemies.Clear();
        
        //enemyObject = enemy;

        LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_THEME_GREATER_FOE);
        LegendaryAudio.io.BattleCrossfade(true);
        battleArenaCenter = player.transform.position;
        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GOLD_AURA, battleArenaCenter);

        foreach(GameObject multiEnemy in enemies)
        {
            multibattle_enemies.Add(Instantiate(multiEnemy));
        }

        if(multibattle_enemies.Count == 2)
        {
            multibattle_enemies[0].transform.position = battleArenaCenter + (new Vector3(-1, 0, 1) * 3);
            multibattle_enemies[1].transform.position = battleArenaCenter + (new Vector3(-1, 0, 0f) * 3);
            multibattle_enemies[0].transform.LookAt(playerObject.transform);
            multibattle_enemies[1].transform.LookAt(playerObject.transform);
            currentMultiBattleEnemy = multibattle_enemies[0].transform;
            enemy1_Stats = multibattle_enemies[0].GetComponent<LegendaryActor>();
            enemy2_Stats = multibattle_enemies[1].GetComponent<LegendaryActor>();
            enemy3_Stats = null;
            enemy1_Hp.fillAmount = ((float)enemy1_Stats.currentHP / (float)enemy1_Stats.maxHP);
            enemy2_Hp.fillAmount = ((float)enemy2_Stats.currentHP / (float)enemy2_Stats.maxHP);
            enemy1_Avatar.sprite = enemy1_Stats.avatar;
            enemy2_Avatar.sprite = enemy2_Stats.avatar;
            enemy1_Icon.SetActive(true);
            enemy2_Icon.SetActive(true);
            enemy3_Icon.SetActive(false);

        }

        if (multibattle_enemies.Count == 3)
        {
            multibattle_enemies[0].transform.position = battleArenaCenter + (new Vector3(-1, 0, 1) * 3);
            multibattle_enemies[1].transform.position = battleArenaCenter + (new Vector3(-1, 0, 0) * 3);
            multibattle_enemies[2].transform.position = battleArenaCenter + (new Vector3(-1, 0, 2) * 3);
            multibattle_enemies[0].transform.LookAt(playerObject.transform);
            multibattle_enemies[1].transform.LookAt(playerObject.transform);
            multibattle_enemies[2].transform.LookAt(playerObject.transform);
            currentMultiBattleEnemy = multibattle_enemies[0].transform;
            enemy1_Stats = multibattle_enemies[0].GetComponent<LegendaryActor>();
            enemy2_Stats = multibattle_enemies[1].GetComponent<LegendaryActor>();
            enemy3_Stats = multibattle_enemies[2].GetComponent<LegendaryActor>();
            enemy1_Hp.fillAmount = ((float)enemy1_Stats.currentHP / (float)enemy1_Stats.maxHP);
            enemy2_Hp.fillAmount = ((float)enemy2_Stats.currentHP / (float)enemy2_Stats.maxHP);
            enemy3_Hp.fillAmount = ((float)enemy3_Stats.currentHP / (float)enemy3_Stats.maxHP);
            enemy1_Avatar.sprite = enemy1_Stats.avatar;
            enemy2_Avatar.sprite = enemy2_Stats.avatar;
            enemy3_Avatar.sprite = enemy3_Stats.avatar;
            enemy1_Icon.SetActive(true);
            enemy2_Icon.SetActive(true);
            enemy3_Icon.SetActive(true);
        }


        // configure Player
        playerObject.transform.LookAt(currentMultiBattleEnemy);
        playerObject.GetComponent<LegendaryPlayer>().Battle(true);
        playerStats = playerObject.GetComponent<LegendaryActor>();
        playerObject.GetComponent<Animator>().SetLayerWeight(2, 1);

        playerHpAmount.text = playerStats.currentHP.ToString();
        playerPha.fillAmount = ((float)playerStats.currentPh / (float)playerStats.maxPh);
        playerPhaAmount.text = playerStats.currentPh.ToString();
        playerAvatar.sprite = playerStats.avatar;

        if (playerObject.GetComponent<LegendaryPlayer>().isUnarmed())
        {
            playerWeapon.enabled = false;
            playerWeaponDescription.enabled = false;
        }
        else
        {
            playerWeapon.enabled = true;
            playerWeaponDescription.enabled = true;
            playerWeapon.sprite = LegendaryWeaponRing.io.GetWeaponSprite(LegendaryCore.io.GetWeaponIndex());
            switch (LegendaryCore.io.GetWeaponIndex())
            {
                case 0:
                    playerWeaponDescription.text = "AXE";
                    break;
                case 1:
                    playerWeaponDescription.text = "SWORD";
                    break;
                case 2:
                    playerWeaponDescription.text = "DAGGER";
                    break;
                case 3:
                    playerWeaponDescription.text = "STAFF";
                    break;
                case 4:
                    playerWeaponDescription.text = "HAMMER";
                    break;
                case 5:
                    playerWeaponDescription.text = "BOW";
                    break;
                case 6:
                    playerWeaponDescription.text = "SPEAR";
                    break;
            }
        }

        battleVictorySprite.sprite = playerStats.avatar;
        battleVictorySpriteShadow.sprite = playerStats.avatar;
        battleVictoryName.text = playerStats.actorName;
        battleVictoryLevel.text = "LEVEL " + playerStats.actorLevel;

        // Set Scenery
        DOTween.To(() => battlePP.GetComponent<PostProcessVolume>().weight, x => battlePP.GetComponent<PostProcessVolume>().weight = x, 1f, 4f);
        battleCamera.SetActive(true);
        beginBattle_time = Time.time;
        DOTween.To(() => battleHud.GetComponent<CanvasGroup>().alpha, x => battleHud.GetComponent<CanvasGroup>().alpha = x, 1f, 2f);

        // Start Battle
        battleHud.gameObject.SetActive(true);
        state = LegendaryBattleState.START;
        StartCoroutine(WaitForMultiTurnComplete());

    }

    IEnumerator WaitForMultiTurnComplete()
    {
        yield return new WaitForSeconds(1.5f);
        state = LegendaryBattleState.WAIT_FOR_PLAYER;
        yield return new WaitForSeconds(2);
        BattleMenu.gameObject.SetActive(true);
        int currentAmountEnemies = multibattle_enemies.Count;
        currentMenu = LegendaryBattleMenu.MENU;

        while (state != LegendaryBattleState.WON && state != LegendaryBattleState.LOST)
        {

            if (state == LegendaryBattleState.WAIT_FOR_PLAYER)
            {
                if (!BattleMenu.gameObject.activeSelf)
                {
                    currentMenu = LegendaryBattleMenu.MENU;
                    currentSelection = 0;
                    SwitchSelection(0);
                    BattleMenu.gameObject.SetActive(true);
                }
            }

            if (state == LegendaryBattleState.PLAYER_TURN)
            {
                switch (currentBattleSkill)
                {
                    case LegendaryBattleSkill.IRON_ATTACK:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                        yield return new WaitForSeconds(0.8f);
                        if (isWeaponSlashing())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.SLASH_1, playerObject.transform.position + Vector3.up);
                        }
                        else
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.PIERCE_1, playerObject.transform);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, currentMultiBattleEnemy.transform.position + Vector3.up);
                            currentMultiBattleEnemy.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            currentMultiBattleEnemy.GetComponent<LegendaryActor>().currentHP -= ApplyMeleeDamage();

                        }
                        break;
                    case LegendaryBattleSkill.IRON_ATTACK_ACCURATE:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_atk_accurate);
                        yield return new WaitForSeconds(0.8f);
                        if (!playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.BOW))
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.SLASH_1, playerObject.transform.position + Vector3.up);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, currentMultiBattleEnemy.transform.position + Vector3.up);
                            currentMultiBattleEnemy.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            currentMultiBattleEnemy.GetComponent<LegendaryActor>().currentHP -= ApplyMeleeDamage();

                        }
                        break;
                    case LegendaryBattleSkill.IRON_ATTACK_CRITICAL:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_atk_critical);
                        yield return new WaitForSeconds(0.8f);
                        if (!playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.BOW))
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.SLASH_1, playerObject.transform.position + Vector3.up);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, currentMultiBattleEnemy.transform.position + Vector3.up);
                            currentMultiBattleEnemy.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            currentMultiBattleEnemy.GetComponent<LegendaryActor>().currentHP -= ApplyMeleeDamage();
                        }
                        break;
                    case LegendaryBattleSkill.MUSCLE_BLOCK:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_msc_block);
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case LegendaryBattleSkill.MUSCLE_DODGE:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_msc_dodge);
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case LegendaryBattleSkill.MUSCLE_RICOCHET:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_msc_riposte);
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case LegendaryBattleSkill.PHA_ATTACK:
                        if (PlayerHasRequiredSpiritPower())
                        {
                            playerStats.currentPh -= GetPlayerRequiredSpiritPower();
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_pha_blast);
                            yield return new WaitForSeconds(0.8f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.NOVA_YELLOW, currentMultiBattleEnemy.transform.position + Vector3.up);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, currentMultiBattleEnemy.transform.position + Vector3.up);
                            currentMultiBattleEnemy.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            currentMultiBattleEnemy.GetComponent<LegendaryActor>().currentHP -= ApplySpiritDamage();
                        }
                        break;
                    case LegendaryBattleSkill.PHA_PROTECT:
                        if (PlayerHasRequiredSpiritPower())
                        {
                            playerStats.currentPh -= GetPlayerRequiredSpiritPower();
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_pha_cast);
                            yield return new WaitForSeconds(0.8f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GROUND_WIND, playerObject.transform.position);
                        }
                        yield return new WaitForSeconds(0.2f);
                        break;
                    case LegendaryBattleSkill.PHA_HEAL:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_pha_heal);
                        yield return new WaitForSeconds(0.8f);
                        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GOLD_AURA, playerObject.transform.position + Vector3.up);
                        if (playerStats.currentHP < playerStats.maxHP)
                        {
                            LegendaryCore.io.AddHealth(5 * playerStats.actorLevel);
                        }
                        break;

                }

                playerPha.DOFillAmount(((float)playerStats.currentPh / (float)playerStats.maxPh), 0.5f);
                playerHp.DOFillAmount(((float)playerStats.currentHP / (float)playerStats.maxHP), 0.5f);
                playerHpAmount.text = playerStats.currentHP.ToString();
                playerPhaAmount.text = playerStats.currentPh.ToString();

                if(enemy1_Stats != null)
                {
                    enemy1_Hp.DOFillAmount(((float)enemy1_Stats.currentHP / (float)enemy1_Stats.maxHP), 0.5f);
                }

                if (enemy2_Stats != null)
                {
                    enemy2_Hp.DOFillAmount(((float)enemy2_Stats.currentHP / (float)enemy2_Stats.maxHP), 0.5f);
                }

                if (enemy3_Stats != null)
                {
                    enemy3_Hp.DOFillAmount(((float)enemy3_Stats.currentHP / (float)enemy3_Stats.maxHP), 0.5f);

                }

                if (enemy1_Stats != null && enemy1_Stats.currentHP < 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_die);
                    multibattle_enemies[0].GetComponent<LegendaryActor>().isAlive = false;
                    //multibattle_enemies.Remove(multibattle_enemies[0]);
                    if(enemy2_Stats != null && enemy2_Stats.currentHP > 1)
                    {
                        currentMultiBattleEnemy = multibattle_enemies[1].transform;
                        playerObject.transform.LookAt(currentMultiBattleEnemy);
                    } else
                    {
                        if(enemy3_Stats != null && enemy3_Stats.currentHP > 1)
                        {
                            currentMultiBattleEnemy = multibattle_enemies[2].transform;
                            playerObject.transform.LookAt(currentMultiBattleEnemy);
                        } else
                        {
                            state = LegendaryBattleState.WON;
                        }
                    }

                }

                if (enemy2_Stats != null && enemy2_Stats.currentHP < 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_die);
                    multibattle_enemies[1].GetComponent<LegendaryActor>().isAlive = false;
                    //multibattle_enemies.Remove(multibattle_enemies[1]);
                    if (enemy1_Stats != null && enemy1_Stats.currentHP > 1)
                    {
                        currentMultiBattleEnemy = multibattle_enemies[0].transform;
                        playerObject.transform.LookAt(currentMultiBattleEnemy);
                    }
                    else
                    {
                        if (enemy3_Stats != null && enemy3_Stats.currentHP > 1)
                        {
                            currentMultiBattleEnemy = multibattle_enemies[2].transform;
                            playerObject.transform.LookAt(currentMultiBattleEnemy);
                        }
                        else
                        {
                            state = LegendaryBattleState.WON;
                        }
                    }
                }

                if (enemy3_Stats != null && enemy3_Stats.currentHP < 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_die);
                    multibattle_enemies[2].GetComponent<LegendaryActor>().isAlive = false;
                    //multibattle_enemies.Remove(multibattle_enemies[2]);
                    if (enemy1_Stats != null && enemy1_Stats.currentHP > 1)
                    {
                        currentMultiBattleEnemy = multibattle_enemies[0].transform;
                        playerObject.transform.LookAt(currentMultiBattleEnemy);
                    }
                    else
                    {
                        if (enemy2_Stats != null && enemy2_Stats.currentHP > 1)
                        {
                            currentMultiBattleEnemy = multibattle_enemies[1].transform;
                            playerObject.transform.LookAt(currentMultiBattleEnemy);
                        }
                        else
                        {
                            state = LegendaryBattleState.WON;
                        }
                    }
                }
                

                if(multibattle_enemies.Count > 0)
                {
                    if(enemy1_Stats.currentHP > 1 || enemy2_Stats.currentHP > 1  || enemy3_Stats.currentHP > 1)

                    state = LegendaryBattleState.ENEMY_TURN;
                } else
                {
                    state = LegendaryBattleState.WON;
                }

            }

            if (state == LegendaryBattleState.ENEMY_TURN)
            {


                switch (currentBattleSkill)
                {
                    case LegendaryBattleSkill.MUSCLE_BLOCK:
                        if (RollDiceHitOrMiss())
                        {
                            if(enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy1_Stats.damage;
                                Debug.Log("enemy damaged player: " + enemy1_Stats.damage);
                                LegendaryCore.io.ShowHitData(enemy1_Stats.damage, playerObject);
                            }

                            if(enemy2_Stats.isAlive)
                            {
                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy2_Stats.damage;
                                Debug.Log("enemy damaged player: " + enemy2_Stats.damage);
                                LegendaryCore.io.ShowHitData(enemy2_Stats.damage, playerObject);
                            }

                            if(enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy3_Stats.damage;
                                Debug.Log("enemy damaged player: " + enemy3_Stats.damage);
                                LegendaryCore.io.ShowHitData(enemy3_Stats.damage, playerObject);
                            }

                        }
                        else
                        {
                            // show miss

                            if (enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                                playerStats.currentHP -= enemy1_Stats.damage / 2;
                                Debug.Log("enemy damaged player: " + enemy1_Stats.damage / 2);
                                LegendaryCore.io.ShowHitData(enemy1_Stats.damage / 2, playerObject);
                            }

                            if (enemy2_Stats.isAlive)
                            {
                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                                playerStats.currentHP -= enemy2_Stats.damage / 2;
                                Debug.Log("enemy damaged player: " + enemy2_Stats.damage / 2);
                                LegendaryCore.io.ShowHitData(enemy2_Stats.damage / 2, playerObject);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                                playerStats.currentHP -= enemy3_Stats.damage / 2;
                                Debug.Log("enemy damaged player: " + enemy3_Stats.damage / 2);
                                LegendaryCore.io.ShowHitData(enemy3_Stats.damage / 2, playerObject);
                            }

                        }
                        break;
                    case LegendaryBattleSkill.MUSCLE_DODGE:
                        if (RollDiceHitOrMiss())
                        {

                            if (enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy1_Stats.damage * 2;
                                Debug.Log("enemy damaged player: " + enemy1_Stats.damage * 2);
                                LegendaryCore.io.ShowHitData(enemy1_Stats.damage * 2, playerObject);
                            }

                            if (enemy2_Stats.isAlive)
                            {
                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy2_Stats.damage * 2;
                                Debug.Log("enemy damaged player: " + enemy2_Stats.damage * 2);
                                LegendaryCore.io.ShowHitData(enemy2_Stats.damage * 2, playerObject);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy3_Stats.damage * 2;
                                Debug.Log("enemy damaged player: " + enemy3_Stats.damage * 2);
                                LegendaryCore.io.ShowHitData(enemy3_Stats.damage * 2, playerObject);
                            }

                        }
                        else
                        {
                            // show miss
                            if (enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);

                            }

                            if (enemy2_Stats.isAlive)
                            {
                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                            }

                        }
                        break;
                    case LegendaryBattleSkill.MUSCLE_RICOCHET:
                        if (RollDiceHitOrMiss())
                        {
                            if(enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy1_Stats.damage * 2;
                                Debug.Log("enemy damaged player: " + enemy1_Stats.damage * 2);
                                LegendaryCore.io.ShowHitData(enemy1_Stats.damage * 2, playerObject);
                            }

                            if(enemy2_Stats.isAlive)
                            {
                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy2_Stats.damage * 2;
                                Debug.Log("enemy damaged player: " + enemy2_Stats.damage * 2);
                                LegendaryCore.io.ShowHitData(enemy2_Stats.damage * 2, playerObject);
                            }

                            if(enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy3_Stats.damage * 2;
                                Debug.Log("enemy damaged player: " + enemy3_Stats.damage * 2);
                                LegendaryCore.io.ShowHitData(enemy3_Stats.damage * 2, playerObject);
                            }

                        }
                        else
                        {
                            

                            if (enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            if (enemy2_Stats.isAlive)
                            {
                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);

                            // show miss
                            
                            if (RollDiceHitOrMiss())
                            {
                                if (enemy1_Stats.isAlive)
                                {
                                    LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, multibattle_enemies[0].transform.position + Vector3.up);
                                    multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                    enemy1_Stats.currentHP -= enemy1_Stats.damage;
                                    Debug.Log("enemy damaged by riposte: " + enemy1_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy1_Stats.damage, multibattle_enemies[0]);
                                }

                                if (enemy2_Stats.isAlive)
                                {
                                    LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, multibattle_enemies[1].transform.position + Vector3.up);
                                    multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                    enemy2_Stats.currentHP -= enemy2_Stats.damage;
                                    Debug.Log("enemy damaged by riposte: " + enemy2_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy2_Stats.damage, multibattle_enemies[1]);
                                }

                                if (enemy3_Stats != null && enemy3_Stats.isAlive)
                                {
                                    LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, multibattle_enemies[2].transform.position + Vector3.up);
                                    multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                    enemy3_Stats.currentHP -= enemy3_Stats.damage;
                                    Debug.Log("enemy damaged by riposte: " + enemy3_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy3_Stats.damage, multibattle_enemies[2]);
                                }

                            }
                            yield return new WaitForSeconds(0.2f);

                        }
                        break;
                    case LegendaryBattleSkill.PHA_PROTECT:
                        if (RollDiceHitOrMiss())
                        {
                            if (enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            if (enemy2_Stats.isAlive)
                            {

                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);

                            if (enemy1_Stats.isAlive)
                            {
                                if (playerStats.currentPh > enemy1_Stats.damage)
                                {
                                    playerStats.currentPh -= enemy1_Stats.damage;
                                    Debug.Log("enemy damaged player, but took damage as Pha: " + enemy1_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy1_Stats.damage, playerObject);
                                }
                                else
                                {
                                    playerStats.currentHP -= enemy1_Stats.damage;
                                    Debug.Log("enemy damaged player, but took damage as Hp: " + enemy1_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy1_Stats.damage, playerObject);
                                }
                            }

                            if (enemy2_Stats.isAlive)
                            {
                                if (playerStats.currentPh > enemy2_Stats.damage)
                                {
                                    playerStats.currentPh -= enemy2_Stats.damage;
                                    Debug.Log("enemy damaged player, but took damage as Pha: " + enemy2_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy2_Stats.damage, playerObject);
                                }
                                else
                                {
                                    playerStats.currentHP -= enemy1_Stats.damage;
                                    Debug.Log("enemy damaged player, but took damage as Hp: " + enemy1_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy1_Stats.damage, playerObject);
                                }
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                if (playerStats.currentPh > enemy3_Stats.damage)
                                {
                                    playerStats.currentPh -= enemy3_Stats.damage;
                                    Debug.Log("enemy damaged player, but took damage as Pha: " + enemy3_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy3_Stats.damage, playerObject);
                                }
                                else
                                {
                                    playerStats.currentHP -= enemy3_Stats.damage;
                                    Debug.Log("enemy damaged player, but took damage as Hp: " + enemy3_Stats.damage);
                                    LegendaryCore.io.ShowHitData(enemy3_Stats.damage, playerObject);
                                }
                            }

                        }
                        else
                        {
                            if (enemy1_Stats.isAlive)
                            {
                                multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            if (enemy2_Stats.isAlive)
                            {

                                multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                                yield return new WaitForSeconds(0.2f);

                            }
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);

                        }
                        break;
                    default:
                        yield return new WaitForSeconds(1.8f);
                        if (enemy1_Stats.isAlive)
                        {
                            multibattle_enemies[0].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                        }

                        if (enemy2_Stats.isAlive)
                        {

                            multibattle_enemies[1].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                        }

                        if (enemy3_Stats != null && enemy3_Stats.isAlive)
                        {
                            multibattle_enemies[2].GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                        }

                        if (RollDiceHitOrMiss())
                        {
                            Debug.Log("enemy damaged player.");
                            if (enemy1_Stats.isAlive)
                            {
                                
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy1_Stats.damage;
                                LegendaryCore.io.ShowHitData(enemy1_Stats.damage, playerObject);
                            }

                            if (enemy2_Stats.isAlive)
                            {
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy2_Stats.damage;
                                LegendaryCore.io.ShowHitData(enemy2_Stats.damage, playerObject);
                            }

                            if (enemy3_Stats != null && enemy3_Stats.isAlive)
                            {
                                yield return new WaitForSeconds(0.2f);
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                                playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                playerStats.currentHP -= enemy3_Stats.damage;
                                LegendaryCore.io.ShowHitData(enemy3_Stats.damage, playerObject);
                            }
                        }
                        else
                        {
                            Debug.Log("enemy hit, but missed the player.");
                            yield return new WaitForSeconds(0.2f);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                            playerStats.currentPh += 1;
                        }
                        break;
                }

                playerPha.DOFillAmount(((float)playerStats.currentPh / (float)playerStats.maxPh), 0.5f);
                playerHp.DOFillAmount(((float)playerStats.currentHP / (float)playerStats.maxHP), 0.5f);
                playerHpAmount.text = playerStats.currentHP.ToString();
                playerPhaAmount.text = playerStats.currentPh.ToString();

                if (enemy1_Stats != null)
                {
                    enemy1_Hp.DOFillAmount(((float)enemy1_Stats.currentHP / (float)enemy1_Stats.maxHP), 0.5f);
                }

                if (enemy2_Stats != null)
                {
                    enemy2_Hp.DOFillAmount(((float)enemy2_Stats.currentHP / (float)enemy2_Stats.maxHP), 0.5f);
                }

                if (enemy3_Stats != null)
                {
                    enemy3_Hp.DOFillAmount(((float)enemy3_Stats.currentHP / (float)enemy3_Stats.maxHP), 0.5f);

                }
                /*
                if (enemy1_Stats.isAlive)
                {
                    enemy1_Hp.DOFillAmount(((float)enemy1_Stats.currentHP / (float)enemy1_Stats.maxHP), 0.5f);

                }

                if (enemy2_Stats.isAlive)
                {
                    enemy2_Hp.DOFillAmount(((float)enemy2_Stats.currentHP / (float)enemy2_Stats.maxHP), 0.5f);

                }

                if (enemy3_Stats != null && enemy3_Stats.isAlive)
                {
                    enemy3_Hp.DOFillAmount(((float)enemy3_Stats.currentHP / (float)enemy3_Stats.maxHP), 0.5f);

                }*/

                //Dishook.Send(enemyStats.actorName + " hits " + playerStats.actorName + " with damage(" + enemyStats.damage + ")", discordBattles);

                if (playerStats.currentHP < 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    playerObject.GetComponent<Animator>().SetTrigger(anim_battle_die);
                    state = LegendaryBattleState.LOST;
                    Dishook.Send(enemyStats.actorName + " kills " + playerStats.actorName + " with damage(" + enemyStats.damage + "). Sadly, the Lander " + LegendsofLairds_BIOS.io.GetPlayerElrondAccount().Address + " perished.", discordBattles);
                }
                else
                {
                    state = LegendaryBattleState.WAIT_FOR_PLAYER;
                    yield return new WaitForSeconds(2);
                }



            }

            yield return null;
        }

        endbattle_time = Time.time;

        // victory or defeat
        switch (state)
        {
            case LegendaryBattleState.WON:
                StartCoroutine(BeginMultiBattleVictory());
                break;
            case LegendaryBattleState.LOST:
                StartCoroutine(BeginBattleLost());
                break;
        }


    }

    IEnumerator BeginMultiBattleVictory()
    {
        /*
            determine bonus,
            determine XP
            determine KCAL
            determine Reward
         
         */




        int bonus_xp = 0;
        float calcxp_1 = (float)enemy1_Stats.maxHP / (float)playerStats.maxHP * 100.0f;
        float calcxp_2 = (float)enemy2_Stats.maxHP / (float)playerStats.maxHP * 100.0f;
        float calcxp_3 = 0;
        if (enemy3_Stats != null)
        {
            calcxp_3 = (float)enemy3_Stats.maxHP / (float)playerStats.maxHP * 100.0f;
        }

        float calcxp = calcxp_1 + calcxp_2 + calcxp_3;
        Debug.Log(" 1: " + calcxp_1);
        Debug.Log(" 2: " + calcxp_2);
        Debug.Log(" 3: " + calcxp_3);
        int xp = (int)calcxp;
        float kcal = calcxp * 0.17773177f;

        float battleTimeTotal = endbattle_time - beginBattle_time;
        if (battleTimeTotal < 60.0f)
        {
            bonus_xp += 100 * playerStats.actorLevel;
        }

        battleVictoryXp.text = (xp + bonus_xp) + " XP";
        battleVictoryKcal.text = kcal.ToString("F2") + " MXE";
        battleVictoryXpLarge.text = xp + " XP";
        // make reward different

        if(multibattle_enemies.Count == 2)
        {
            battleVictoryReward.text = multibattle_enemies[0].GetComponent<LegendaryEnemy>().GetReward().itemTitle + "," + multibattle_enemies[1].GetComponent<LegendaryEnemy>().GetReward().itemTitle ;
        } 
        if(multibattle_enemies.Count == 3)
        {
            battleVictoryReward.text = multibattle_enemies[0].GetComponent<LegendaryEnemy>().GetReward().itemTitle + ", " + multibattle_enemies[1].GetComponent<LegendaryEnemy>().GetReward().itemTitle + ", " + multibattle_enemies[2].GetComponent<LegendaryEnemy>().GetReward().itemTitle;
        }

                                   
        battleVictoryBonusXP.text = "+" + bonus_xp + " XP";



        //Debug.Log(" elapsed time: " + ((endbattle_time - beginBattle_time) / 60));
        //Debug.Log(" elapsed time: " + (endbattle_time - beginBattle_time));

        DOTween.To(() => battleHud.GetComponent<CanvasGroup>().alpha, x => battleHud.GetComponent<CanvasGroup>().alpha = x, 0f, 2f);
        yield return new WaitForSeconds(0.3f);

        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_victory);
        yield return new WaitForSeconds(0.3f);
        if(enemyObject != null)
        {
            if (enemyObject.GetComponent<LegendaryBeast>() != null)
            {
                enemyObject.GetComponent<LegendaryBeast>().ToggleBeastCamera(false);
            }
        }

        victoryCamera.SetActive(true);

        LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_VICTORY_INTRO);
        battleVictory.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        battleHud.gameObject.SetActive(false);


        LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_VICTORY_LOOP);

        yield return new WaitForSeconds(6);

        victoryCamera.SetActive(false);
        DOTween.To(() => battlePP.GetComponent<PostProcessVolume>().weight, x => battlePP.GetComponent<PostProcessVolume>().weight = x, 0f, 4f);

        playerObject.GetComponent<Animator>().SetLayerWeight(2, 0);
        playerObject.GetComponent<LegendaryPlayer>().Battle(false);
        playerObject.GetComponent<LegendaryPlayer>().HudUpdates();
        state = LegendaryBattleState.AVAILABLE;
        battleCamera.SetActive(false);
        battleVictory.gameObject.SetActive(false);
        LegendaryCore.io.ToggleWorldHud(true);
        LegendaryAudio.io.BattleCrossfade(false);
        foreach(GameObject enemy in multibattle_enemies)
        {
            LegendaryCore.io.KillEnemy(enemy);
        }
        LegendaryCore.io.AddEarnings(kcal, LegendaryCurrencySymbol.KCAL);
        LegendaryCore.io.AddExperience(xp + bonus_xp);
    }




    public void CreateBattle(GameObject player, GameObject enemy)
    {
        go_battleVictoryBonusXP.SetActive(false);
        player.GetComponent<LegendaryPlayer>().Mount(false);
        LegendaryCore.io.WeaponRingDisable();
        LegendaryCore.io.ToggleWorldHud(false);
        LegendaryCore.io.ToggleMiniMap(false);
        LegendaryCore.io.SwitchCamera(LegendaryView.NORMAL);
        LegendaryCore.io.BattleSequenceBegin();
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_LOADER_1);
        enemy2_Icon.SetActive(false);
        enemy3_Icon.SetActive(false);
        playerObject = player;
        enemyObject = enemy;
        if(enemyObject.GetComponent<LegendaryBeast>() == null)
        {
            LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_THEME_FOE);
        } else
        {
            enemyObject.GetComponent<LegendaryBeast>().ToggleBeastCamera(true);
            LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_THEME_GREATER_FOE);
        }

        
        LegendaryAudio.io.BattleCrossfade(true);
        battleArenaCenter = player.transform.position;
        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GOLD_AURA, battleArenaCenter);
        enemyObject.transform.position = battleArenaCenter + (Vector3.left * 3);
        enemyObject.transform.LookAt(playerObject.transform);
        playerObject.transform.LookAt(enemyObject.transform);
        playerObject.GetComponent<LegendaryPlayer>().Battle(true);
        playerStats = playerObject.GetComponent<LegendaryActor>();
        enemyStats = enemyObject.GetComponent<LegendaryActor>();
        playerObject.GetComponent<Animator>().SetLayerWeight(2, 1);
        DOTween.To(() => battlePP.GetComponent<PostProcessVolume>().weight, x => battlePP.GetComponent<PostProcessVolume>().weight = x, 1f, 4f);
        
        enemyHp.fillAmount = ((float)enemyStats.currentHP / (float)enemyStats.maxHP);
        playerHpAmount.text = playerStats.currentHP.ToString();
        playerPha.fillAmount = ((float)playerStats.currentPh / (float)playerStats.maxPh);
        playerPhaAmount.text = playerStats.currentPh.ToString();
        enemyAvatar.sprite = enemyStats.avatar;
        playerAvatar.sprite = playerStats.avatar;
        // GetWeaponSprite
        if(playerObject.GetComponent<LegendaryPlayer>().isUnarmed())
        {
            playerWeapon.enabled = false;
            playerWeaponDescription.enabled = false;
        } else
        {
            playerWeapon.enabled = true;
            playerWeaponDescription.enabled = true;
            playerWeapon.sprite = LegendaryWeaponRing.io.GetWeaponSprite(LegendaryCore.io.GetWeaponIndex());
            switch (LegendaryCore.io.GetWeaponIndex())
            {
                case 0:
                    playerWeaponDescription.text = "AXE";
                    break;
                case 1:
                    playerWeaponDescription.text = "SWORD";
                    break;
                case 2:
                    playerWeaponDescription.text = "DAGGER";
                    break;
                case 3:
                    playerWeaponDescription.text = "STAFF";
                    break;
                case 4:
                    playerWeaponDescription.text = "HAMMER";
                    break;
                case 5:
                    playerWeaponDescription.text = "BOW";
                    break;
                case 6:
                    playerWeaponDescription.text = "SPEAR";
                    break;
            }
        }

        battleVictorySprite.sprite = playerStats.avatar;
        battleVictorySpriteShadow.sprite = playerStats.avatar;
        battleVictoryName.text = playerStats.actorName;
        battleVictoryLevel.text = "LEVEL " + playerStats.actorLevel;

        battleHud.gameObject.SetActive(true);
        SetupBattle();
        state = LegendaryBattleState.START;
        Dishook.Send("a battle begins between " + enemyStats.actorName + " and " + playerStats.actorName + " from " + LegendsofLairds_BIOS.io.GetPlayerElrondAccount().Address, discordBattles);
    }

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

        inputCooldown = Time.time;
        state = LegendaryBattleState.AVAILABLE;
    }

    private int ApplyMeleeDamage()
    {
        int appliedDamage = 0;
        LegendaryWeaponType weaponType = playerObject.GetComponent<LegendaryPlayer>().GetCurrentPlayerWeaponType();
        LegendaryActorStats weaponStats = playerObject.GetComponent<LegendaryPlayer>().GetActorStats();

        switch(weaponType)
        {
            case LegendaryWeaponType.SWORD_1H:
                appliedDamage = playerStats.damage * weaponStats.iron;
                break;
            case LegendaryWeaponType.AXE_1H:
                appliedDamage = playerStats.damage * weaponStats.muscle;
                break;
            case LegendaryWeaponType.HAMMER:
                appliedDamage = playerStats.damage * weaponStats.muscle;
                break;
            case LegendaryWeaponType.UNARMED:
                appliedDamage = playerStats.damage * weaponStats.heart;
                break;
            case LegendaryWeaponType.SPEAR:
                appliedDamage = playerStats.damage * weaponStats.iron;
                break;
            case LegendaryWeaponType.BOW:
                appliedDamage = playerStats.damage * weaponStats.guile;
                break;
            case LegendaryWeaponType.STAFF:
                appliedDamage = playerStats.damage * weaponStats.intuition;
                break;
            case LegendaryWeaponType.DAGGER:
                appliedDamage = playerStats.damage * weaponStats.guile;
                break;
        }

        if(enemyObject != null)
        {
            LegendaryCore.io.ShowHitData(appliedDamage, enemyObject);
        } else
        {
            LegendaryCore.io.ShowHitData(appliedDamage, currentMultiBattleEnemy.gameObject);
        }
        
        Debug.Log("applied melee damage by player: " + appliedDamage);
        return appliedDamage;
    }

    private bool PlayerHasRequiredSpiritPower()
    {
        int requiredPha = 0;
        LegendaryWeaponType weaponType = playerObject.GetComponent<LegendaryPlayer>().GetCurrentPlayerWeaponType();
        LegendaryActorStats weaponStats = playerObject.GetComponent<LegendaryPlayer>().GetActorStats();
        switch (weaponType)
        {
            case LegendaryWeaponType.SWORD_1H:
                requiredPha = playerStats.actorLevel * 10;
                break;
            case LegendaryWeaponType.AXE_1H:
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.HAMMER:
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.UNARMED:
                requiredPha = playerStats.actorLevel * 3;
                break;
            case LegendaryWeaponType.SPEAR:
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.BOW:
                requiredPha = playerStats.actorLevel * 10;
                break;
            case LegendaryWeaponType.STAFF:
                requiredPha = playerStats.actorLevel * 3;
                break;
            case LegendaryWeaponType.DAGGER:
                requiredPha = playerStats.actorLevel * 5;
                break;
        }

        if (playerStats.currentPh > requiredPha)
        {
            Debug.Log("required pha points by player: " + requiredPha);
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GetPlayerRequiredSpiritPower()
    {
        int requiredPha = 0;

        LegendaryWeaponType weaponType = playerObject.GetComponent<LegendaryPlayer>().GetCurrentPlayerWeaponType();
        LegendaryActorStats weaponStats = playerObject.GetComponent<LegendaryPlayer>().GetActorStats();

        switch (weaponType)
        {
            case LegendaryWeaponType.SWORD_1H:
                requiredPha = playerStats.actorLevel * 10;
                break;
            case LegendaryWeaponType.AXE_1H:
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.HAMMER:
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.UNARMED:
                requiredPha = playerStats.actorLevel * 3;
                break;
            case LegendaryWeaponType.SPEAR:
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.BOW:
                requiredPha = playerStats.actorLevel * 10;
                break;
            case LegendaryWeaponType.STAFF:
                requiredPha = playerStats.actorLevel * 3;
                break;
            case LegendaryWeaponType.DAGGER:
                requiredPha = playerStats.actorLevel * 5;
                break;
        }

        return requiredPha;

    }

    private int ApplySpiritDamage()
    {
        int appliedDamage = 0;
        int requiredPha = 0;

        if(playerStats.currentPh < 1)
        {
            return appliedDamage;
        }

        LegendaryWeaponType weaponType = playerObject.GetComponent<LegendaryPlayer>().GetCurrentPlayerWeaponType();
        LegendaryActorStats weaponStats = playerObject.GetComponent<LegendaryPlayer>().GetActorStats();

        switch (weaponType)
        {
            case LegendaryWeaponType.SWORD_1H:
                appliedDamage = playerStats.damage - weaponStats.iron;
                requiredPha = playerStats.actorLevel * 10;
                break;
            case LegendaryWeaponType.AXE_1H:
                appliedDamage = playerStats.damage - weaponStats.muscle * 2;
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.HAMMER:
                appliedDamage = playerStats.damage - weaponStats.muscle * 2;
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.UNARMED:
                appliedDamage = playerStats.damage * weaponStats.heart;
                requiredPha = playerStats.actorLevel * 3;
                break;
            case LegendaryWeaponType.SPEAR:
                appliedDamage = playerStats.damage - weaponStats.iron * 2;
                requiredPha = playerStats.actorLevel * 15;
                break;
            case LegendaryWeaponType.BOW:
                appliedDamage = playerStats.damage - weaponStats.guile * 2;
                requiredPha = playerStats.actorLevel * 10;
                break;
            case LegendaryWeaponType.STAFF:
                appliedDamage = playerStats.damage * weaponStats.intuition;
                requiredPha = playerStats.actorLevel * 3;
                break;
            case LegendaryWeaponType.DAGGER:
                requiredPha = playerStats.actorLevel * 5;
                appliedDamage = playerStats.damage - weaponStats.guile;
                break;
        }

        if(playerStats.currentPh > requiredPha )
        {
            if(enemyObject != null)
            {
                LegendaryCore.io.ShowHitData(appliedDamage, enemyObject);
            } else
            {
                LegendaryCore.io.ShowHitData(appliedDamage, currentMultiBattleEnemy.gameObject);
            }
            
            Debug.Log("applied spirit damage by player: " + appliedDamage);
            return appliedDamage;
        } else
        {
            if(enemyObject != null)
            {
                LegendaryCore.io.ShowHitData(0, enemyObject);
            } else
            {
                LegendaryCore.io.ShowHitData(0, currentMultiBattleEnemy.gameObject);
            }
            
            return 0;
        }

    }

    bool isWeaponSlashing()
    {
        bool isBow = playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.BOW);
        bool isSpear = playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.DAGGER);
        bool isDagger = playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.SPEAR);

        if(isBow || isSpear || isDagger)
        {
            return false;
        }

        return true;
    }

    IEnumerator WaitForTurnComplete()
    {
        yield return new WaitForSeconds(1.5f);
        DOTween.To(() => battleHud.GetComponent<CanvasGroup>().alpha, x => battleHud.GetComponent<CanvasGroup>().alpha = x, 1f, 2f);

        state = LegendaryBattleState.WAIT_FOR_PLAYER;

        yield return new WaitForSeconds(2);
        BattleMenu.gameObject.SetActive(true);
        currentMenu = LegendaryBattleMenu.MENU;

        while (state != LegendaryBattleState.WON && state != LegendaryBattleState.LOST)
        {
            if(state == LegendaryBattleState.WAIT_FOR_PLAYER)
            {
                if(!BattleMenu.gameObject.activeSelf)
                {
                    currentMenu = LegendaryBattleMenu.MENU;
                    currentSelection = 0;
                    SwitchSelection(0);
                    BattleMenu.gameObject.SetActive(true);
                    
                }
            }


            if(state == LegendaryBattleState.PLAYER_TURN)
            {
                Debug.Log("applied battle skill: " + currentBattleSkill);
                switch(currentBattleSkill)
                {
                    case LegendaryBattleSkill.IRON_ATTACK:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                        yield return new WaitForSeconds(0.8f);
                        if (isWeaponSlashing())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.SLASH_1, playerObject.transform.position + Vector3.up);
                        }
                        else
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.PIERCE_1, playerObject.transform);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if(RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, enemyObject.transform.position + Vector3.up);
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            enemyStats.currentHP -= ApplyMeleeDamage();
                            
                        } 
                        break;
                    case LegendaryBattleSkill.IRON_ATTACK_ACCURATE:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_atk_accurate);
                        yield return new WaitForSeconds(0.8f);
                        if (!playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.BOW))
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.SLASH_1, playerObject.transform.position + Vector3.up);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, enemyObject.transform.position + Vector3.up);
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            enemyStats.currentHP -= ApplyMeleeDamage();
                            
                        }
                        break;
                    case LegendaryBattleSkill.IRON_ATTACK_CRITICAL:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_atk_critical);
                        yield return new WaitForSeconds(0.8f);
                        if (!playerObject.GetComponent<LegendaryPlayer>().CheckForWeapon(LegendaryWeaponType.BOW))
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.SLASH_1, playerObject.transform.position + Vector3.up);
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, enemyObject.transform.position + Vector3.up);
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            enemyStats.currentHP -= ApplyMeleeDamage();
                        }
                        break;
                    case LegendaryBattleSkill.MUSCLE_BLOCK:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_msc_block);
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case LegendaryBattleSkill.MUSCLE_DODGE:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_msc_dodge);
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case LegendaryBattleSkill.MUSCLE_RICOCHET:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_msc_riposte);
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case LegendaryBattleSkill.PHA_ATTACK:
                        if(PlayerHasRequiredSpiritPower())
                        {
                            playerStats.currentPh -= GetPlayerRequiredSpiritPower();
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_pha_blast);
                            yield return new WaitForSeconds(0.8f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.NOVA_YELLOW, enemyObject.transform.position + Vector3.up);
                        } 
                        yield return new WaitForSeconds(0.2f);
                        if (RollDiceHitOrMiss())
                        {
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, enemyObject.transform.position + Vector3.up);
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            enemyStats.currentHP -= ApplySpiritDamage();
                        }
                        break;
                    case LegendaryBattleSkill.PHA_PROTECT:
                        if (PlayerHasRequiredSpiritPower())
                        {
                            playerStats.currentPh -= GetPlayerRequiredSpiritPower();
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_pha_cast);
                            yield return new WaitForSeconds(0.8f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GROUND_WIND, playerObject.transform.position);
                        }
                        yield return new WaitForSeconds(0.2f);
                        break;
                    case LegendaryBattleSkill.PHA_HEAL:
                        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_pha_heal);
                        yield return new WaitForSeconds(0.8f);
                        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GOLD_AURA, playerObject.transform.position + Vector3.up);
                        if(playerStats.currentHP < playerStats.maxHP)
                        {
                            LegendaryCore.io.AddHealth(5 * playerStats.actorLevel);
                        }
                        break;

                }

                playerPha.DOFillAmount(((float)playerStats.currentPh / (float)playerStats.maxPh), 0.5f);
                playerHp.DOFillAmount(((float)playerStats.currentHP / (float)playerStats.maxHP), 0.5f);
                playerHpAmount.text = playerStats.currentHP.ToString();
                playerPhaAmount.text = playerStats.currentPh.ToString();
                enemyHp.DOFillAmount(((float)enemyStats.currentHP / (float)enemyStats.maxHP), 0.5f);
                
                // Dishook.Send(playerStats.actorName + " hits " + enemyStats.actorName + " with damage(" + playerStats.damage + ")", discordBattles);

                if (enemyStats.currentHP < 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_die);
                    endbattle_time = Time.time;
                    state = LegendaryBattleState.WON;
                    enemyObject.GetComponent<LegendaryActor>().isAlive = false;
                    Dishook.Send(playerStats.actorName + " from " + LegendsofLairds_BIOS.io.GetPlayerElrondAccount().Address+ " kills " + enemyStats.actorName + " with damage(" + playerStats.damage + "). Another foe bites the dust. Victory!", discordBattles);
                } 
                else
                {
                    state = LegendaryBattleState.ENEMY_TURN;
                }
                
            }

            if (state == LegendaryBattleState.ENEMY_TURN)
            {
                
                
                switch(currentBattleSkill)
                {
                    case LegendaryBattleSkill.MUSCLE_BLOCK:
                        if(RollDiceHitOrMiss())
                        {
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            playerStats.currentHP -= enemyStats.damage;
                            Debug.Log("enemy damaged player: " + enemyStats.damage);
                            LegendaryCore.io.ShowHitData(enemyStats.damage, playerObject);

                        } 
                        else
                        {
                            // show miss
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                            playerStats.currentHP -= enemyStats.damage / 2;
                            Debug.Log("enemy damaged player: " + enemyStats.damage / 2);
                            LegendaryCore.io.ShowHitData(enemyStats.damage / 2, playerObject);
                        }
                        break;
                    case LegendaryBattleSkill.MUSCLE_DODGE:
                        if (RollDiceHitOrMiss())
                        {
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            playerStats.currentHP -= enemyStats.damage * 2;
                            Debug.Log("enemy damaged player: " + enemyStats.damage * 2);
                            LegendaryCore.io.ShowHitData(enemyStats.damage * 2, playerObject);
                        }
                        else
                        {
                            // show miss
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                        }
                        break;
                    case LegendaryBattleSkill.MUSCLE_RICOCHET:
                        if (RollDiceHitOrMiss())
                        {
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            playerStats.currentHP -= enemyStats.damage * 2;
                            Debug.Log("enemy damaged player: " + enemyStats.damage * 2);
                            LegendaryCore.io.ShowHitData(enemyStats.damage * 2, playerObject);
                        }
                        else
                        {
                            // show miss
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                            yield return new WaitForSeconds(0.2f);
                            if (RollDiceHitOrMiss())
                            {
                                LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_MONSTER_1, enemyObject.transform.position + Vector3.up);
                                enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                                enemyStats.currentHP -= enemyStats.damage;
                                Debug.Log("enemy damaged by riposte: " + enemyStats.damage);
                                LegendaryCore.io.ShowHitData(enemyStats.damage, enemyObject);
                            }
                            yield return new WaitForSeconds(0.2f);
                            
                        }
                        break;
                    case LegendaryBattleSkill.PHA_PROTECT:
                        if (RollDiceHitOrMiss())
                        {
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            if(playerStats.currentPh > enemyStats.damage)
                            {
                                playerStats.currentPh -= enemyStats.damage;
                                Debug.Log("enemy damaged player, but took damage as Pha: " + enemyStats.damage);
                                LegendaryCore.io.ShowHitData(enemyStats.damage, playerObject);
                            } else
                            {
                                playerStats.currentHP -= enemyStats.damage;
                                Debug.Log("enemy damaged player, but took damage as Hp: " + enemyStats.damage);
                                LegendaryCore.io.ShowHitData(enemyStats.damage, playerObject);
                            }

                        }
                        else
                        {
                            // show miss
                            enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                            yield return new WaitForSeconds(0.2f);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                        }
                        break;
                    default:
                        
                        yield return new WaitForSeconds(1.8f);
                        enemyObject.GetComponent<Animator>().SetTrigger(anim_battle_atk);
                        if (RollDiceHitOrMiss())
                        {
                            Debug.Log("enemy damaged player.");
                            yield return new WaitForSeconds(0.2f);
                            LegendaryFx.io.PlayEffect(LegendaryVisualEffect.HIT_WHITE_SPARKS, playerObject.transform.position + Vector3.up);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_hit);
                            playerStats.currentHP -= enemyStats.damage;
                            LegendaryCore.io.ShowHitData(enemyStats.damage, playerObject);
                        } else
                        {
                            Debug.Log("enemy hit, but missed the player.");
                            yield return new WaitForSeconds(0.2f);
                            playerObject.GetComponent<Animator>().SetTrigger(anim_battle_miss);
                            playerStats.currentPh += 1;
                        }
                        break;
                }

                playerPha.DOFillAmount(((float)playerStats.currentPh / (float)playerStats.maxPh), 0.5f);
                playerHp.DOFillAmount(((float)playerStats.currentHP / (float)playerStats.maxHP), 0.5f);
                playerHpAmount.text = playerStats.currentHP.ToString();
                playerPhaAmount.text = playerStats.currentPh.ToString();
                enemyHp.DOFillAmount(((float)enemyStats.currentHP / (float)enemyStats.maxHP), 0.5f);

                //Dishook.Send(enemyStats.actorName + " hits " + playerStats.actorName + " with damage(" + enemyStats.damage + ")", discordBattles);

                if (playerStats.currentHP < 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    playerObject.GetComponent<Animator>().SetTrigger(anim_battle_die);
                    state = LegendaryBattleState.LOST;                    
                    Dishook.Send(enemyStats.actorName + " kills " + playerStats.actorName + " with damage(" + enemyStats.damage + "). Sadly, the Lander perished.", discordBattles);
                }
                else
                {
                    state = LegendaryBattleState.WAIT_FOR_PLAYER;
                    yield return new WaitForSeconds(2);
                }

                

            }

            yield return null;
        }

        // victory or defeat
        
        switch (state)
        {
            case LegendaryBattleState.WON:
                StartCoroutine(BeginBattleVictory());
                break;
            case LegendaryBattleState.LOST:
                StartCoroutine(BeginBattleLost());
                break;
        }


    } 

    IEnumerator BeginBattleLost()
    {
        DOTween.To(() => battleHud.GetComponent<CanvasGroup>().alpha, x => battleHud.GetComponent<CanvasGroup>().alpha = x, 0f, 2f);
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(0.3f);
        if(enemyObject != null)
        {
            if (enemyObject.GetComponent<LegendaryBeast>() != null)
            {
                enemyObject.GetComponent<LegendaryBeast>().ToggleBeastCamera(false);
            }
        }

        defeatCamera.SetActive(true);
        LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_LOST); ;
        battleDefeat.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        battleHud.gameObject.SetActive(false);
        yield return new WaitForSeconds(13);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    IEnumerator BeginBattleVictory()
    {
        /*
            determine bonus,
            determine XP
            determine KCAL
            determine Reward
         
         */
        int bonus_xp = 0;
        float calcxp = (float) enemyStats.maxHP / (float ) playerStats.maxHP * 100.0f;
        int xp = (int) calcxp;
        float kcal = calcxp * 0.17773177f; 

        float battleTimeTotal = endbattle_time - beginBattle_time;
        if(battleTimeTotal < 60.0f )
        {
            bonus_xp += 100 * playerStats.actorLevel;
        }

        battleVictoryXp.text = (xp + bonus_xp) + " XP";
        battleVictoryKcal.text = kcal.ToString("F2") + " MXE";
        battleVictoryXpLarge.text = xp + " XP";
        battleVictoryReward.text = enemyObject.GetComponent<LegendaryEnemy>().GetReward().itemTitle + " x1";
        battleVictoryBonusXP.text = "+" + bonus_xp + " XP";

        

        //Debug.Log(" elapsed time: " + ((endbattle_time - beginBattle_time) / 60));
        //Debug.Log(" elapsed time: " + (endbattle_time - beginBattle_time));

        DOTween.To(() => battleHud.GetComponent<CanvasGroup>().alpha, x => battleHud.GetComponent<CanvasGroup>().alpha = x, 0f, 2f);
        yield return new WaitForSeconds(0.3f);

        playerObject.GetComponent<Animator>().SetTrigger(anim_battle_victory);
        yield return new WaitForSeconds(0.3f);
        if (enemyObject.GetComponent<LegendaryBeast>() != null)
        {
            enemyObject.GetComponent<LegendaryBeast>().ToggleBeastCamera(false);
        }
        victoryCamera.SetActive(true);

        LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_VICTORY_INTRO);
        battleVictory.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        battleHud.gameObject.SetActive(false);


        LegendaryAudio.io.PlayBattleTheme(LegendaryAudioType.BATTLE_VICTORY_LOOP);

        yield return new WaitForSeconds(6);

        victoryCamera.SetActive(false);
        DOTween.To(() => battlePP.GetComponent<PostProcessVolume>().weight, x => battlePP.GetComponent<PostProcessVolume>().weight = x, 0f, 4f);

        playerObject.GetComponent<Animator>().SetLayerWeight(2, 0);
        playerObject.GetComponent<LegendaryPlayer>().Battle(false);
        playerObject.GetComponent<LegendaryPlayer>().HudUpdates();
        state = LegendaryBattleState.AVAILABLE;
        battleCamera.SetActive(false);
        battleVictory.gameObject.SetActive(false);
        LegendaryCore.io.ToggleWorldHud(true);
        LegendaryAudio.io.BattleCrossfade(false);
        LegendaryCore.io.KillEnemy(enemyObject);
        LegendaryCore.io.AddEarnings(kcal, LegendaryCurrencySymbol.KCAL);
        LegendaryCore.io.AddExperience(xp + bonus_xp);
    }

    private void Start()
    {
        
    }

    private void SetupBattle()
    {        
        battleCamera.SetActive(true);
        beginBattle_time = Time.time;
        BattleTurn();
    }

    private void DeterminePositions()
    {

    }

    private void GetPlayerBattleChoice()
    {

    }

    public void StartBattle()
    {

    }

    
    private void EndBattle()
    {

    }

    public bool isBattleCompleted()
    {
        return false;
    }



    private void Victory()
    {

    }

    private void Defeat()
    {

    }

    private void BattleTurn()
    {
        StartCoroutine(WaitForTurnComplete());
    }

    private void NextBattleTurn()
    {

    }

    private void PlayCombatantTurn()
    {

    }

    private void UpdateCombatants()
    {

    }

    private void WaitForParty()
    {

    }

    private bool RollDiceHitOrMiss()
    {
        float dice = Random.Range(0.0f, 1.0f);
        if(dice < 0.8)
        {
            Debug.Log("Hit!");
            return true;
        } else
        {
            Debug.Log("Miss!");
            return false;
        }
    }





}
