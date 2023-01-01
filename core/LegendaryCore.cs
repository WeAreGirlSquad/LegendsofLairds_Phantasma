using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using QFSW.QC;
using QFSW.QC.Actions;
using Cinemachine;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;
using Erdcsharp.Provider.Dtos;
using System.Linq;

public enum LegendaryView
{
    NORMAL,
    CLOSE,
    SAIL,
    BATTLE,
    MAP,
    CLIMB,
    HOME
}

public enum LegendaryCurrencySymbol
{
    SOUL,
    KCAL,
    WAGS
}

public enum LegendaryWeather
{
    NORMAL,
    DRY,
    RAIN,
    SNOW,
    FOGGY,
    AUTUMN
}

public enum LegendaryLayer
{
    EXTERIOR,
    INTERIOR
}

[CommandPrefix("core.")]
public class LegendaryCore : MonoBehaviour
{
    [SerializeField] LegendarySysconfig configSys;
    public static LegendaryCore io;
    [SerializeField] AudioSource sfxChannel2;
    [SerializeField] AudioSource sfxChannel3;
    [SerializeField] AudioSource sfxChannel4;
    [SerializeField] AudioSource battleMusic;
    [SerializeField] AudioSource worldMusic;
    [SerializeField] Transform battleLoader;
    [SerializeField] Transform localPlayer;
    [SerializeField] GameObject MiniMapCamera;
    [SerializeField] GameObject MiniMapCanvas;
    [SerializeField] GameObject WeaponRing; // 120 Y normal, 280 Y hunt view
    [SerializeField] GameObject WarpingEffectProfile;
    [SerializeField] GameObject WarpingTornado;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI chatText;
    [SerializeField] Image chatAvatar;
    [SerializeField] TextMeshProUGUI chatActorname;
    [SerializeField] CanvasGroup playerWorldHud;
    [SerializeField] GameObject itemBox;
    [SerializeField] TextMeshProUGUI itemTitle;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Image itemThumbnail;
    [SerializeField] GameObject craftingPP;
    [SerializeField] CanvasGroup craftingHud;
    [SerializeField] CanvasGroup cookingHud;
    [SerializeField] CanvasGroup tanningHud;
    [SerializeField] GameObject nightPP;
    [SerializeField] Light directionalSun;
    [SerializeField] TextMeshProUGUI statIron;
    [SerializeField] TextMeshProUGUI statMuscle;
    [SerializeField] TextMeshProUGUI statHeart;
    [SerializeField] TextMeshProUGUI statGuile;
    [SerializeField] TextMeshProUGUI statIntuition;

    [SerializeField] Image statIron_progress;
    [SerializeField] Image statMuscle_progress;
    [SerializeField] Image statHeart_progress;
    [SerializeField] Image statGuile_progress;
    [SerializeField] Image statIntuition_progress;

    [SerializeField] Image statMount;

    [SerializeField] GameObject transition_swipe;

    [SerializeField] Image hpPlayerHud;
    [SerializeField] Image phPlayerHud;
    [SerializeField] TextMeshProUGUI hpAmountPlayerHud;
    [SerializeField] TextMeshProUGUI phAmountPlayerHud;
    [SerializeField] Image avatarPlayerHud;
    [SerializeField] TextMeshProUGUI lvlAmountPlayerHud;


    [SerializeField] GameObject hitEffectHarvestable;
    [SerializeField] GameObject godRayEffect;

    [SerializeField] GameObject defeatCamera;
    [SerializeField] GameObject battlePP;
    [SerializeField] Transform battleDefeat;

    // weaponring objects
    [SerializeField] Image currentActiveWeaponHudImage;
    [SerializeField] Transform[] weapons;
    // weaponring objects glow
    [SerializeField] Transform[] weapon_glow;

    [SerializeField] GameObject sailingCamera;
    [SerializeField] GameObject CloseCamera;
    [SerializeField] GameObject buildingCamera;

    int weaponRingIndex = 3; // element 4

    [SerializeField] GameObject mountEffect;
    [SerializeField] GameObject levelupEffect;

    bool craftingToggle = false;
    bool minimapToggle = false;
    bool weaponringToggle = false;
    float cooldown_minimap = 0.0f;
    float cooldown_weaponring = 0.0f;
    float cooldown_weaponring_navigation = 0.0f;
    float cooldown_craftingmode = 0.0f;
    float cooldown_statsmenu = 0.0f;
    float cooldown_camswitch = 0.0f;
    bool killed = false;
    LegendaryWeaponType currentSelectedWeapon = LegendaryWeaponType.UNARMED;

    [SerializeField] GameObject debrisWood;
    [SerializeField] GameObject debrisIron;
    [SerializeField] GameObject debrisRock;

    [SerializeField] GameObject StatsCamera;
    [SerializeField] CanvasGroup menuStatsHud;

    [SerializeField] GameObject LegendaryConsole;
    [SerializeField] GameObject questBox;
    [SerializeField] TextMeshProUGUI QuestTrackerTitle;
    [SerializeField] TextMeshProUGUI QuestTrackerDescription;

    [SerializeField] GameObject interactionButton;
    [SerializeField] TextMeshProUGUI interactionButtonText;

    [SerializeField] GameObject treasureBox;
    [SerializeField] CanvasGroup treasureBoxCanvasGroup;
    [SerializeField] Image treasureThumb;
    [SerializeField] TextMeshProUGUI treasureTitle;
    [SerializeField] TextMeshProUGUI treasureDescription;

    public bool isActionOnePressed = false;

    [SerializeField] GameObject worldCam;
    [SerializeField] GameObject battleCam;
    [SerializeField] GameObject battle_victory_cam;
    [SerializeField] GameObject battle_defeat_cam;
    [SerializeField] GameObject homecam;
    [SerializeField] GameObject worldCamPerspective;
    [SerializeField] GameObject climbCam;

    [SerializeField] TextMeshProUGUI amountSoul;
    [SerializeField] TextMeshProUGUI amountKcal;
    [SerializeField] TextMeshProUGUI amountWags;

    float playerAmountSoul = 0.0f;
    float playerAmountKcal = 0.0f;
    float playerAmountWags = 0.0f;
    int playerExperience = 0;
    int playerRequiredExperience = 0;

    int base_xp = 100;
    int exponent_xp = 5;

    [SerializeField] TextMeshProUGUI amountXp;
    [SerializeField] TextMeshProUGUI amountNxtLvl;

    [SerializeField] CanvasGroup worldMessageCG;
    [SerializeField] TextMeshProUGUI worldMessageTitle;
    [SerializeField] TextMeshProUGUI worldMessageDescription;

    [SerializeField] RectTransform hitDataBox;
    [SerializeField] GameObject hitDataObject;
    [SerializeField] RectTransform canvasRect;

    [SerializeField] int phantalia_daytime = 8; // port to remote config

    [SerializeField] LineRenderer questCompass;
    [SerializeField] Transform questOrigin;
    public Transform questTarget;

    [SerializeField] GameObject weatherRainObject;
    [SerializeField] GameObject weatherSnowObject;
    [SerializeField] GameObject weatherFoggyObject;
    [SerializeField] GameObject weatherAutumnObject;

    GameObject currentWeather;
    [SerializeField] Transform weatherAnchor;
    [SerializeField] GameObject birdsController;
    bool isDayTime = false;
    bool inMusicZone = false;

    [SerializeField] LayerMask battleLayer;
    [SerializeField] LayerMask exteriorLayer;
    [SerializeField] LayerMask interiorLayer;
    [SerializeField] GameObject interiorCam;
    [SerializeField] GameObject interiorPP;

    AccountDto playerDto;
    string playerAddress;
    CinemachineComposer composer;
    void Awake()
    {



        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }

        LegendsofLairds_BIOS.io.RuntimeBIOS();
        Application.targetFrameRate = 30;
        questCompass.GetComponent<LineRenderer>();
        playerDto = LegendsofLairds_BIOS.io.GetPlayerElrondAccount();
        playerAddress = playerDto.Address;
        composer = CloseCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();

    }

    [Command]
    public string GetPlayerAddress()
    {
        return playerAddress;
    }

    public void SwitchLayer(LegendaryLayer index)
    {
        switch(index)
        {
            case LegendaryLayer.EXTERIOR:
                Camera.main.cullingMask = exteriorLayer;
                interiorCam.SetActive(false);
                DOTween.To(() => interiorPP.GetComponent<PostProcessVolume>().weight, x => interiorPP.GetComponent<PostProcessVolume>().weight = x, 0f, 0.5f);
                break;
            case LegendaryLayer.INTERIOR:
                Camera.main.cullingMask = interiorLayer;
                interiorCam.SetActive(true);
                DOTween.To(() => interiorPP.GetComponent<PostProcessVolume>().weight, x => interiorPP.GetComponent<PostProcessVolume>().weight = x, 1f, 0.5f);
                break;
        }
    }

    [Command]
    public void DayNightCyle()
    {
        //Debug.Log("invoked! Day and Nightcycle");
        InvokeRepeating("IncreaseDaytime", 0.1f, 60f);
    }

    public void SetMusicZone(bool toggle)
    {
        inMusicZone = toggle;
    }

    [Command] 
    public void ChangeWeather(LegendaryWeather weatherType)
    {
        switch(weatherType)
        {
            case LegendaryWeather.NORMAL:
                Destroy(currentWeather);
                break;
            case LegendaryWeather.RAIN:
                Destroy(currentWeather);
                currentWeather = Instantiate(weatherRainObject, weatherAnchor);
                break;
            case LegendaryWeather.SNOW:
                Destroy(currentWeather);
                currentWeather = Instantiate(weatherSnowObject, weatherAnchor);
                break;
            case LegendaryWeather.FOGGY:
                Destroy(currentWeather);
                currentWeather = Instantiate(weatherFoggyObject, weatherAnchor);
                break;
            case LegendaryWeather.AUTUMN:
                Destroy(currentWeather);
                currentWeather = Instantiate(weatherAutumnObject, weatherAnchor);
                break;
        }
    }

    [Command]
    public void LegendsClassicMode(bool toggle)
    {
        Camera.main.gameObject.GetComponent<Helix.ImageEffects.Retro>().enabled = toggle;
    }

    [Command]
    public void EnableCompass(bool toggle)
    {
        if(toggle)
        {
            questCompass.enabled = true;
        } else
        {
            questCompass.enabled = false;
        }

        


    }

    public bool IsDaytimeNow()
    {
        return isDayTime;
    }

    public void CloseCamAdjust(float adjust)
    {
        composer.m_TrackedObjectOffset.y = adjust;
        // .GetComponent<CinemachineComposer>().m_TrackedObjectOffset.y = adjust;


    }

    [Command]
    public void IncreaseDaytime()
    {
        
        phantalia_daytime += 1;
        Debug.Log("Ding! Its " + phantalia_daytime + " in Phantalia");
        if (phantalia_daytime == 24)
        {
            phantalia_daytime = 0;
        }

        if(phantalia_daytime < 6)
        {
            // night to dawn
        }

        if(phantalia_daytime == 8)
        {
            DOTween.To(() => nightPP.GetComponent<PostProcessVolume>().weight, x => nightPP.GetComponent<PostProcessVolume>().weight = x, 0f, 180f);
            if(!inMusicZone)
            {
                LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_SAILING);
            }
            
            birdsController.SetActive(true);
            isDayTime = true;

        }

        if(phantalia_daytime == 12)
        {
            //LegendaryAudio.io.PlayMusic(LegendaryAudioType.);
        }

        if (phantalia_daytime == 20)
        {
            DOTween.To(() => nightPP.GetComponent<PostProcessVolume>().weight, x => nightPP.GetComponent<PostProcessVolume>().weight = x, 1f, 180f);
            if(!inMusicZone)
            {
                LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_NIGHT);
            }
            
            birdsController.SetActive(false);
            isDayTime = false;
        }

        if (phantalia_daytime > 6 && phantalia_daytime < 12 )
        {
            // dawn to noon
            if (directionalSun.intensity < 1.0f)
            {
                directionalSun.DOIntensity(directionalSun.intensity + 0.1f, 0.9f);
               
            }

        }

        if(phantalia_daytime > 12 && phantalia_daytime < 18)
        {
            // noon
        }

        if (phantalia_daytime > 18 && phantalia_daytime < 24 )
        {
            if(directionalSun.intensity > 0.4f)
            {
                directionalSun.DOIntensity(directionalSun.intensity - 0.1f, 0.9f);
              
            } 

            // evening to night
        }

    }


    public void UpdateQuestTracker(string title, string description)
    {
        questBox.SetActive(true);
        QuestTrackerTitle.text = title;
        QuestTrackerDescription.text = description;
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.QUEST_UPDATE);
    }

    [Command]
    public void ShowWorldMessage(string title, string description)
    {
        worldMessageCG.DOKill();
        worldMessageTitle.text = title;
        worldMessageDescription.text = description;
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.GAMEMENU_ACTIVATE);
        Sequence worldmessageSequence = DOTween.Sequence()
        .Append(worldMessageCG.DOFade(1.0f, 0.5f))
        .AppendInterval(4.5f)
        .Append(worldMessageCG.DOFade(0.0f, 0.6f))
        ;
    }

    [Command]
    public string ShowPlayerLocation()
    {
        return "X: " + GetLocalPlayer().position.x + " Z: " + GetLocalPlayer().position.z + "Y: " + GetLocalPlayer().position.y;
    }

    public bool CheckLegendaryConsoleEnabled()
    {
        if(LegendaryConsole.activeSelf)
        {
            return true;
        } else
        {
            return false;
        }

    }

    [Command]
    public void FreeFromLock()
    {
        localPlayer.GetComponent<LegendaryPlayer>().AllowControls(true);
    }
    private void OnEnable()
    {
    #if UNITY_STANDALONE && !UNITY_EDITOR
        Cursor.visible = false;
    #endif

    }

    public void KillPlayerByTrap()
    {
        if(killed)
        {
            return;
        }
        localPlayer.GetComponent<LegendaryPlayer>().Mount(false);
        localPlayer.GetComponent<CharacterController>().enabled = false;
        localPlayer.GetComponent<LegendaryActor>().currentHP = 0;
        localPlayer.GetComponent<LegendaryPlayer>().HudUpdates();
        localPlayer.GetComponent<LegendaryPlayer>().AllowControls(false);
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(true);
        StartCoroutine(GameOverByDeath());
    }

    IEnumerator GameOverByDeath()
    {
        localPlayer.GetComponent<Animator>().SetTrigger("Trap_DEATH");
        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.BLOOD_DEATH, localPlayer);
        //DOTween.To(() => battlePP.GetComponent<PostProcessVolume>().weight, x => battlePP.GetComponent<PostProcessVolume>().weight = x, 1f, 2f);
        defeatCamera.SetActive(true);
        LegendaryAudio.io.PlayMusic(LegendaryAudioType.BATTLE_LOST);
        yield return new WaitForSeconds(1.5f);
        battleDefeat.gameObject.SetActive(true);
        yield return new WaitForSeconds(10.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void ShowHitData(int amount, GameObject target)
    {




        GameObject go = Instantiate(hitDataObject, hitDataBox);
        TextMeshProUGUI hitdata = go.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        hitdata.text = amount + "";
        if (amount == 0)
        {
            hitdata.text = "Miss";
        }
        
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(target.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
        go.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        go.SetActive(true);
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.HIT_BOX_1);
        Sequence mySequence = DOTween.Sequence()
        .Append(go.GetComponent<RectTransform>().DOAnchorPosY(20f, 0.2f))
        .AppendInterval(0.2f)
        .Append(go.GetComponent<RectTransform>().DOSizeDelta(new Vector2(142, 100), 0.2f))
        .AppendInterval(0.5f)
        .Append(go.GetComponent<RectTransform>().DOAnchorPosY(500f, 0.5f))
        .AppendInterval(0.3f)
        .Append(go.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1, 1), 0.2f))
        ;
        Destroy(go, 3.5f);
    }

    /*
    [Command("legendsoflairds")]
    public static IEnumerator<ICommandAction> Countdown()
    {
        for (int i = 3; i > 0; i--)
        {
            yield return new Value(i);
            yield return new WaitRealtime(1);
        }

        yield return new Value("Countdown over!");
    }*/

    [Command]
    private static void setQualityLevel(int index) {
        QualitySettings.SetQualityLevel(index);
    }

    public void PausePlayerActions()
    {

    }

    public void SetQuestMapObject(GameObject mapObject)
    {
        questTarget = mapObject.transform;
    }

    private void FixedUpdate()
    {
        if(localPlayer != null)
        {
            if(questCompass.enabled)
            {
                if(questTarget == null)
                {
                    EnableCompass(false);
                    return;
                }
                questOrigin = localPlayer;
                questCompass.SetPosition(0, questOrigin.position);
                questCompass.SetPosition(1, questTarget.position);
            }
        }
    }

    void CharacterLevelUp()
    {
        int remainder_xp = playerExperience - GetRequiredXpForNextLevel(localPlayer.GetComponent<LegendaryActor>().actorLevel);
        playerExperience = remainder_xp;
        localPlayer.GetComponent<LegendaryActor>().actorLevel = localPlayer.GetComponent<LegendaryActor>().actorLevel + 1;
        localPlayer.GetComponent<LegendaryActor>().maxHP += 10;
        localPlayer.GetComponent<LegendaryActor>().maxPh += 2;
        localPlayer.GetComponent<LegendaryActor>().currentHP = localPlayer.GetComponent<LegendaryActor>().maxHP;
        PlayLevelUpEffect();
        UpdatePlayerStatsHud();
        //UpdateAvatarStats(avatarPlayerHud.sprite, localPlayer.GetComponent<LegendaryActor>().actorLevel);
        localPlayer.GetComponent<LegendaryPlayer>().HudUpdates();
    }

    [Command]
    public bool hasExperienceForNextLevel()
    {
        int req = GetRequiredXpForNextLevel(localPlayer.GetComponent<LegendaryActor>().actorLevel);

        if(playerExperience < req)
        {
            return false;
        } 

        return true;
    }

    [Command]
    public int GetRequiredXpForNextLevel( int currentLvl)
    {
        //Debug.Log("req." + (currentLvl + 1) * exponent_xp * base_xp);
        return (currentLvl+1) * exponent_xp * base_xp;
    }

 
    public void AddExperience(int xp)
    {
        playerExperience += xp;
        if(hasExperienceForNextLevel())
        {
            CharacterLevelUp();
        }
    }

    public void AddSpirit(int ph)
    {
        if (localPlayer.GetComponent<LegendaryActor>().currentPh < localPlayer.GetComponent<LegendaryActor>().maxPh)
        {
            localPlayer.GetComponent<LegendaryActor>().currentPh += ph;
            localPlayer.GetComponent<LegendaryPlayer>().HudUpdates();
        }

    }



    public void AddHealth(int hp)
    {
        if(localPlayer.GetComponent<LegendaryActor>().currentHP < localPlayer.GetComponent<LegendaryActor>().maxHP)
        {
            localPlayer.GetComponent<LegendaryActor>().currentHP += hp;
            localPlayer.GetComponent<LegendaryPlayer>().HudUpdates();
        }
        
    }

    [Command]
    public int GetExperience()
    {
        return playerExperience;
    }

    public void AddEarnings(float amount, LegendaryCurrencySymbol symbol)
    {
        switch(symbol)
        {
            case LegendaryCurrencySymbol.SOUL:
                playerAmountSoul += amount;
                break;
            case LegendaryCurrencySymbol.KCAL:
                playerAmountKcal += amount;
                break;
            case LegendaryCurrencySymbol.WAGS:
                playerAmountWags += amount;
                break;
        }
        UpdatePlayerStatsHud();
    }

    public void SetTreasureItem(Sprite thumb, string title, string description)
    {
        treasureThumb.sprite = thumb;
        treasureTitle.text = title;
        treasureDescription.text = description;
    }

    public void willReceiveTreasureReward()
    {
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(true);
        localPlayer.GetComponent<LegendaryPlayer>().GetTreasure();
        treasureBox.SetActive(true);
        treasureBoxCanvasGroup.DOFade(1.0f, 0.5f);
    }

    public void hasReceiveTreasureReward()
    {
        StartCoroutine(showReceivedTreasureReward());
        
    }

    IEnumerator showReceivedTreasureReward()
    {
        yield return new WaitForSeconds(1.0f);
        treasureBoxCanvasGroup.DOFade(0f, 0.8f);
        yield return new WaitForSeconds(0.5f);

        localPlayer.GetComponent<LegendaryPlayer>().GotTreasure();
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(false);
        
        
        yield return new WaitForSeconds(1.0f);
        treasureBox.SetActive(false);
        yield return null;
    }

    [Command]
    public void SwitchCamera(LegendaryView view)
    {
        if(cooldown_camswitch > Time.time)
        {
            return;
        }

        cooldown_camswitch = Time.time + 0.5f;

        switch (view)
        {
            case LegendaryView.CLOSE:
                CloseCamera.SetActive(true);
                WeaponRing.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -280f);
                ToggleWorldHud(false);
                break;
            case LegendaryView.NORMAL:
                CloseCamera.SetActive(false);
                WeaponRing.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 120f);
                ToggleWorldHud(true);
                break;
            case LegendaryView.CLIMB:
                //climbCam.SetActive(true);

                break;

        }
    } 

    public void UpdateAvatarStats(Sprite avatar, int lvl)
    {
        avatarPlayerHud.sprite = avatar;
        lvlAmountPlayerHud.text = "LEVEL " + lvl;
    }

    public void UpdateWorldHud(float hpfill, float phfill, int hp, int ph)
    {
        hpPlayerHud.DOFillAmount(hpfill, 0.5f);
        phPlayerHud.DOFillAmount(phfill, 0.5f);
        hpAmountPlayerHud.text = hp + "";
        phAmountPlayerHud.text = ph + "";
    }

    public void UpdatePlayerStatsHud()
    {
        LegendaryPlayer player = GetLocalPlayer().GetComponent<LegendaryPlayer>();
        float maxstat = 10.0f;

        float iron = (float) player.GetStatistic(LegendaryStatistic.IRON);
        float muscle = (float)player.GetStatistic(LegendaryStatistic.MUSCLE);
        float heart = (float)player.GetStatistic(LegendaryStatistic.HEART);
        float guile = (float)player.GetStatistic(LegendaryStatistic.GUILE);
        float intuition = (float)player.GetStatistic(LegendaryStatistic.INTUITION);

        statIron_progress.fillAmount = iron / maxstat ;
        statMuscle_progress.fillAmount = muscle / maxstat;
        statHeart_progress.fillAmount = heart / maxstat;
        statGuile_progress.fillAmount = guile / maxstat;
        statIntuition_progress.fillAmount = intuition / maxstat;

        statIron.text = player.GetStatistic(LegendaryStatistic.IRON).ToString();
        statMuscle.text = player.GetStatistic(LegendaryStatistic.MUSCLE).ToString();
        statHeart.text = player.GetStatistic(LegendaryStatistic.HEART).ToString();
        statGuile.text = player.GetStatistic(LegendaryStatistic.GUILE).ToString();
        statIntuition.text = player.GetStatistic(LegendaryStatistic.INTUITION).ToString();

        statMount.sprite = player.GetCurrentMount().avatar;

        amountSoul.text = playerAmountSoul.ToString("F2");
        amountKcal.text = playerAmountKcal.ToString("F2");
        amountWags.text = playerAmountWags.ToString("F2");

        amountXp.text = playerExperience.ToString();
        amountNxtLvl.text = GetRequiredXpForNextLevel(localPlayer.GetComponent<LegendaryActor>().actorLevel) - playerExperience + "";

    }

    [Command]
    public void EscapeAll()
    {

        transition_swipe.SetActive(false);
        menuStatsHud.DOFade(0, 1f);
        StatsCamera.SetActive(false);

        WeaponRing.SetActive(false);

        dialogueBox.SetActive(false);
        ToggleInteractionButtons(false, false, "", false);
        LegendaryInventory.io.ToggleInventory(false);
        MiniMapCanvas.SetActive(minimapToggle);
        minimapToggle = false;
        ToggleCookingMode(false);
        ToggleTanningMode(false);
        ToggleCraftingMode(false);
        ToggleQuestBox(false);
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(false);
        localPlayer.GetComponent<LegendaryPlayer>().EscapeCrafting();
        localPlayer.GetComponent<LegendaryPlayer>().AllowControls(true);

        localPlayer.GetComponent<Animator>().SetBool("isPicking", false);

    }

    public bool inHuntMode()
    {
        return CloseCamera.activeSelf;
    }

    public void OnApplicationQuit()
    {
        ElrondUnityTools.Manager.Disconnect();
    }



    public void ShowStatisticsMenu(bool toggle)
    {
        if (cooldown_statsmenu > Time.time)
        {
            return;
        }
        cooldown_statsmenu = Time.time + 0.12f;
        WeaponRing.SetActive(false);
        weaponringToggle = false;

        if (toggle)
        {
            localPlayer.DORotate(new Vector3(0, 180, 0), 0.2f);
            ToggleWorldHud(false);
            localPlayer.GetComponent<LegendaryPlayer>().AllowControls(false);
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.GAMEMENU_ACTIVATE);
            UpdatePlayerStatsHud();
            StatsCamera.SetActive(true);
            menuStatsHud.DOFade(1, 1f);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 1f, 1f);
        }
        else
        {
            ToggleWorldHud(true);
            localPlayer.GetComponent<LegendaryPlayer>().AllowControls(true);
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.GAMEMENU_DEACTIVATE);
            StatsCamera.SetActive(false);
            menuStatsHud.DOFade(0, 1f);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 0f, 1f);
        }
    }

    public void PlayMountEffect()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_EFFECT);
        GameObject mountfx = Instantiate(mountEffect, localPlayer.position + Vector3.up, localPlayer.rotation);
        Destroy(mountfx, 1.5f);
    }

    public void PlayDestructEffect()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_EFFECT);
        GameObject smokefx = Instantiate(mountEffect, localPlayer.position + Vector3.up, localPlayer.rotation);
        Destroy(smokefx, 1.5f);
    }

    public void PlayDisappearEffect(Transform disappearingObject)
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.MOUNT_EFFECT);
        GameObject smokefx = Instantiate(mountEffect, disappearingObject.position + Vector3.up, disappearingObject.rotation);
        Destroy(smokefx, 1.5f);
    }

    public void DestructWithEffect(GameObject go)
    {
        go.transform.DOMove(new Vector3(0f,0f,0f), 2f);
        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GROUND_WIND, localPlayer);
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.WARP_PHOENIX);
        Destroy(go, 3f);

    }

    public void DestructToGaia(GameObject go)
    {
        go.transform.DOMoveY(-2.0f, 2.0f);
        Destroy(go, 3f);
    }


    public void PlayLevelUpEffect()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.WARP_PHOENIX);
        GameObject smokefx = Instantiate(levelupEffect, localPlayer.position + Vector3.up, localPlayer.rotation);
        Destroy(smokefx, 1.5f);
    }

    public void PlaceDebris(PhantaliaWorldItem debris, Vector3 placement)
    {
        switch(debris)
        {
            case PhantaliaWorldItem.WOOD:
                Instantiate(debrisWood, placement, Quaternion.identity);
                break;
            case PhantaliaWorldItem.ROCK:
                Instantiate(debrisRock, placement, Quaternion.identity);
                break;
            case PhantaliaWorldItem.IRON:
                Instantiate(debrisIron, placement, Quaternion.identity);
                break;
        }
    }

    [Command]
    public void ActivateSailCamera(bool status)
    {
        sailingCamera.SetActive(status);
    }

    [Command]
    public void ActivateBuildingCamera(bool status)
    {
        buildingCamera.SetActive(status);
    }

    [Command]
    public void ActivateCloseCamera(bool status)
    {
        CloseCamera.SetActive(status);
        ToggleWorldHud(!status);
    }

    private void Start()
    {
        Dishook.Send("A new Lander has appeared in Phantalia from " + playerDto.Address );
        //LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_SAILING);
        weapon_glow[weaponRingIndex].gameObject.SetActive(true);
        currentActiveWeaponHudImage.transform.parent.gameObject.SetActive(false);
        /*foreach(Transform i in weapons)
        {
            i.GetChild(0).gameObject.SetActive(false);
        }*/
        //StartCoroutine(SpawnPlayers());
    }

    public GameObject GetGodRay()
    {
        return godRayEffect;
    }

    IEnumerator SpawnPlayers()
    {
        int counter = 0;
        int target = 2127;
        while(counter < target)
        {
            Dishook.Send("Generating new enemy:" + Animator.StringToHash("dungeon" + counter));
            yield return new WaitForSeconds(0.12f);
            Dishook.Send("Placing loot NFT");
            yield return new WaitForSeconds(0.1f);
            Dishook.Send("Success. Registering on Legendary Network.");
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            counter++;
        }
        yield return null;
    }

    public Transform GetLocalPlayer()
    {
        return localPlayer;
    }

    public LegendaryMountData GetLocalMount()
    {
        return localPlayer.GetComponent<LegendaryPlayer>().GetCurrentMount();
    }

    public void HarvestableHit(Transform spawnPoint)
    {
        GameObject hiteffect = Instantiate(hitEffectHarvestable, spawnPoint);
        //Destroy(hiteffect, 2f);
    }

    [Command]
    public void UpdateWeaponRing()
    {
        weapons[0].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.AXE_1H).thumb;
        weapons[1].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.SWORD_1H).thumb;
        weapons[2].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.DAGGER).thumb;
        weapons[3].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.STAFF).thumb;
        weapons[4].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.HAMMER).thumb;
        weapons[5].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.BOW).thumb;
        weapons[6].GetChild(0).GetComponent<Image>().sprite = LegendaryWeaponRing.io.GetWeaponItemFromRing(LegendaryWeaponType.SPEAR).thumb;
    }

    [Command]
    public void SetWeaponAvailability(LegendaryWeaponType weapon, bool status)
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
        switch(weapon)
        {
            case LegendaryWeaponType.SWORD_1H:
                weapons[1].GetChild(0).gameObject.SetActive(status);
                break;
            case LegendaryWeaponType.AXE_1H:
                weapons[0].GetChild(0).gameObject.SetActive(status);
                break;
            case LegendaryWeaponType.HAMMER:
                weapons[4].GetChild(0).gameObject.SetActive(status);
                break;
            case LegendaryWeaponType.STAFF:
                weapons[3].GetChild(0).gameObject.SetActive(status);
                break;
            case LegendaryWeaponType.SPEAR:
                weapons[6].GetChild(0).gameObject.SetActive(status);
                break;
            case LegendaryWeaponType.DAGGER:
                weapons[2].GetChild(0).gameObject.SetActive(status);
                break;
            case LegendaryWeaponType.BOW:
                weapons[5].GetChild(0).gameObject.SetActive(status);
                break;
        }
    }

    [Command]
    public void ToggleWeaponRing()
    {
        if (cooldown_weaponring < Time.time)
        {

            if(!weaponringToggle)
            {
                //UpdateWeaponRing();
                WeaponRing.SetActive(true);
                weaponringToggle = true;
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.WEAPON_RING_OPEN);
                cooldown_weaponring = Time.time + 0.22f;
            } else
            {
                localPlayer.GetComponent<LegendaryPlayer>().CancelAim();
                localPlayer.GetComponent<LegendaryPlayer>().DeactivateWeapon();
                localPlayer.GetComponent<LegendaryPlayer>().ActivateWeapon(LegendaryWeaponRing.io.SpawnWeapon(weaponRingIndex));
                currentActiveWeaponHudImage.sprite = LegendaryWeaponRing.io.GetWeaponSprite(weaponRingIndex);
                currentActiveWeaponHudImage.transform.parent.gameObject.SetActive(true);

                // set selected weapon as active
                weaponringToggle = false;
                cooldown_weaponring = Time.time + 0.22f;
                //StartCoroutine(DelayObject(WeaponRing, 1.0f));
                WeaponRing.GetComponent<Animator>().SetTrigger("OK");
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.WEAPON_RING_CLOSE);
                UpdateWeaponRing();

            }
        }
    }

    public bool WeaponRingStatus()
    {
        return weaponringToggle;
    }

    [Command]
    public void WeaponRingDisable()
    {
        WeaponRing.SetActive(false);
    }

    public int GetWeaponIndex()
    {
        return weaponRingIndex;
    }

    public void WeaponRingLeft()
    {
        if(cooldown_weaponring_navigation > Time.time)
        {
            return;
        }

        if(weaponRingIndex == 0)
        {
            cooldown_weaponring_navigation = Time.time + 0.12f;
            weaponRingIndex = 6;
            weapon_glow[weaponRingIndex].gameObject.SetActive(true);
            weapon_glow[0].gameObject.SetActive(false);
            //return;
        } else
        {
            cooldown_weaponring_navigation = Time.time + 0.12f;
            weaponRingIndex--;
            weapon_glow[weaponRingIndex].gameObject.SetActive(true);
            weapon_glow[weaponRingIndex + 1].gameObject.SetActive(false);
        }
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_SELECT);
        

    }

    public void WeaponRingRight()
    {
        if (cooldown_weaponring_navigation > Time.time)
        {
            return;
        }

        if (weaponRingIndex == 6)
        {
            cooldown_weaponring_navigation = Time.time + 0.12f;
            weaponRingIndex = 0;
            weapon_glow[weaponRingIndex].gameObject.SetActive(true);
            weapon_glow[6].gameObject.SetActive(false);
            //return;
        }
        else
        {
            cooldown_weaponring_navigation = Time.time + 0.12f;
            weaponRingIndex++;
            weapon_glow[weaponRingIndex].gameObject.SetActive(true);
            weapon_glow[weaponRingIndex - 1].gameObject.SetActive(false);
        }
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_SELECT);
       
    }

    public void WeaponRingReset()
    {

    }


    [Command]
    public void ToggleMiniMap()
    {
        if(cooldown_minimap < Time.time)
        {
            if(!minimapToggle)
            {
                MiniMapCamera.SetActive(true);
                EnableCompass(true);
                minimapToggle = true;
                MiniMapCanvas.SetActive(minimapToggle);
                cooldown_minimap = Time.time + 0.16f;
            } else
            {
                minimapToggle = false;
                EnableCompass(false);
                cooldown_minimap = Time.time + 0.16f;
                StartCoroutine(DelayObject(MiniMapCanvas, 0.8f));
                MiniMapCanvas.GetComponent<Animator>().SetTrigger("OK");
            }
             
        }
       
    }

    public void ToggleMiniMap(bool toggle)
    {
        MiniMapCamera.SetActive(toggle);
        MiniMapCanvas.SetActive(toggle);
        minimapToggle = toggle;

    }


    public void KillEnemy(GameObject creature)
    {
        LegendaryItem loot = creature.GetComponent<LegendaryEnemy>().GetReward();
        if(loot != null)
        {
            LegendaryInventory.io.AddLegendaryItem(loot);
            SetItem(loot.itemTitle, loot.itemDescription, loot.thumb);
            ShowItem(1);
        }
        
        StartCoroutine(Death(creature));
    }

    IEnumerator Death(GameObject creature)
    {
        yield return new WaitForSeconds(2.0f);
        PlayDisappearEffect(creature.transform);
        Destroy(creature);
    }


    // Update is called once per frame
    public void PlaySfx2(AudioClip ac)
    {
        sfxChannel2.clip = ac;
        sfxChannel2.Play();
    }

    public void PlaySfx3(AudioClip ac)
    {
        sfxChannel3.clip = ac;
        sfxChannel3.Play();
    }

    public void PlaySfx4(AudioClip ac)
    {
        sfxChannel4.clip = ac;
        sfxChannel4.Play();
    }

    public void PlayBattleMusic(AudioClip ac)
    {
        battleMusic.clip = ac;
        battleMusic.Play();
    }

    public void PlayWorldMusic(AudioClip ac)
    {
        worldMusic.clip = ac;
        worldMusic.Play();
    }

    public void BattleSequenceBegin()
    {
        battleLoader.gameObject.SetActive(true);
    }

    public void BattleSequenceFinished()
    {
        battleLoader.GetComponent<BattleLoader>().BattleSequenceFinished();
    }

    public void RegisterPlayer(GameObject player)
    {
        if(player.CompareTag("Player"))
        {
            localPlayer = player.transform;

            // minimap
            MiniMapCamera.GetComponent<MMFollowTarget>().Target = player.transform;
            // sailingCamera
            sailingCamera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            sailingCamera.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            // StatsCamera
            StatsCamera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            StatsCamera.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            // CloseCamera
            CloseCamera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            CloseCamera.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            // buildingCamera
            buildingCamera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            buildingCamera.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            //
            //.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            //buildingCamera.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;

            worldCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            battleCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            battle_victory_cam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            battle_defeat_cam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            homecam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            worldCamPerspective.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            climbCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            interiorCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;

            worldCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            battleCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            battle_victory_cam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            battle_defeat_cam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            homecam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            worldCamPerspective.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            climbCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
            interiorCam.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;

            ToggleWorldHud(true);
            
        }

    }

    public void SetItem(string title, string description, Sprite sprite)
    {
        
        itemTitle.text = title;
        itemDescription.text = description;
        itemThumbnail.sprite = sprite;
    }

    public void ShowItem(int id)
    {
        itemBox.SetActive(true);
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_RECEIVED);
        Sequence mySequence = DOTween.Sequence()
        .Append(itemBox.GetComponent<RectTransform>().DOAnchorPosY(-20f, 0.2f))
        .AppendInterval(0.2f)
        .Append(itemThumbnail.GetComponent<RectTransform>().DOSizeDelta(new Vector2(96, 96), 0.2f))
        .AppendInterval(0.3f)
        .Append(itemThumbnail.GetComponent<RectTransform>().DOSizeDelta(new Vector2(64, 64), 0.2f))
        .AppendInterval(3f)
        .Append(itemBox.GetComponent<RectTransform>().DOAnchorPosY(500f, 0.5f))
        ;
    }

    public void ToggleInteractionButtons(bool ActionOne, bool ActionTwo, string description, bool toggle)
    {
        

        if (toggle)
        {
            interactionButton.SetActive(true);
            interactionButtonText.text = description;
            localPlayer.GetComponent<Animator>().SetBool("inPeace", true);
        } else
        {
            interactionButton.SetActive(false);
            interactionButtonText.text = "";
            //localPlayer.GetComponent<Animator>().SetBool("inPeace", false);
            StartCoroutine(DelayedPeace());
        }
    }

    public void ToggleClimbableCamera(bool toggle)
    {
        climbCam.SetActive(toggle);
    }
     

    public void ToggleQuestBox(bool toggle)
    {
        questBox.SetActive(toggle);
    }

    public void SetChatData(Sprite avatar, string avatarname)
    {
        chatAvatar.sprite = avatar;
        chatActorname.text = avatarname;
    }

    public void SetChatData(bool avatar, bool avatarname)
    {
        chatAvatar.gameObject.SetActive(avatar);
        chatActorname.gameObject.SetActive(avatarname);
    }

    public void Chat(string text)
    {

        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(true);
        ToggleWorldHud(false);
        chatText.text = "";
        dialogueBox.SetActive(true);
        chatText.DOText(text, 1.0f);
        StartCoroutine(QuitChat());
        //ShowItem(1);
    }

    public void Chat(bool lastline)
    {
        StartCoroutine(CloseChat());
        //ShowItem(1);
    }

    public void Chat(string text, bool multiline)
    {
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(true);
        ToggleWorldHud(false);
        chatText.text = "";
        dialogueBox.SetActive(true);
        chatText.DOText(text, 1.0f);
        //StartCoroutine(ContinueChat());
        //ShowItem(1);
    }

    IEnumerator CloseChat()
    {
        dialogueBox.transform.GetChild(0).GetComponent<Animator>().SetTrigger("OK");
        yield return new WaitForSeconds(1.0f);
        dialogueBox.SetActive(false);
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(false);
        chatText.text = "";
        ToggleWorldHud(true);
        yield return null;
    }

    IEnumerator QuitChat()
    {
        yield return new WaitForSeconds(3.0f);
        dialogueBox.transform.GetChild(0).GetComponent<Animator>().SetTrigger("OK");
        yield return new WaitForSeconds(1.0f);
        dialogueBox.SetActive(false);
        localPlayer.GetComponent<LegendaryPlayer>().InGameMenu(false);
        chatText.text = "";
        ToggleWorldHud(true);
        yield return null;
    }

    public void WarpObject(GameObject player, Transform destination)
    {
        StartCoroutine(WarpHops(player, destination));
    }

    IEnumerator WarpHops(GameObject player, Transform destination)
    {
        GameObject wt = Instantiate(WarpingTornado, player.transform);
        //WarpingEffectProfile.SetActive(true);
        DOTween.To(() => WarpingEffectProfile.GetComponent<PostProcessVolume>().weight, x => WarpingEffectProfile.GetComponent<PostProcessVolume>().weight = x, 1f, 1f);
        Time.timeScale = 0.5f;
        player.transform.position = destination.position;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(4.7f);
        DOTween.To(() => WarpingEffectProfile.GetComponent<PostProcessVolume>().weight, x => WarpingEffectProfile.GetComponent<PostProcessVolume>().weight = x, 0f, 1f);
        //WarpingEffectProfile.SetActive(false);
        Destroy(wt);
    }

    public void ToggleCraftingMode(bool toggle)
    {
        if(toggle)
        {
            ToggleMiniMap(false);
            LegendaryCraft.io.UpdateInventory();
            ToggleCraftingdHud(true);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 1f, 1f);
        } else
        {
            ToggleCraftingdHud(false);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 0f, 1f);
        }
    }

    public void ToggleCookingMode(bool toggle)
    {
        if (toggle)
        {
            ToggleMiniMap(false);
            LegendaryCraft.io.UpdateSatchel();
            ToggleCookingdHud(true);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 1f, 1f);
        }
        else
        {
            ToggleCookingdHud(false);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 0f, 1f);
            
        }
    }

    public void ToggleTanningMode(bool toggle)
    {
        if (toggle)
        {
            ToggleMiniMap(false);
            LegendaryCraft.io.UpdateBasket();
            ToggleTanningdHud(true);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 1f, 1f);
        }
        else
        {
            ToggleTanningdHud(false);
            DOTween.To(() => craftingPP.GetComponent<PostProcessVolume>().weight, x => craftingPP.GetComponent<PostProcessVolume>().weight = x, 0f, 1f);
        }
    }

    public void ToggleFishingMode(bool toggle)
    {

        
    }


    // add enums
    [Command]
    public void RunTransition()
    {
        transition_swipe.SetActive(true);
        StartCoroutine(DelayObject(transition_swipe, 1f));
    }


    IEnumerator DelayObject(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

    IEnumerator DelayedPeace()
    {
        localPlayer.GetComponent<Animator>().ResetTrigger("Action1");
        yield return new WaitForSeconds(1.5f);
        localPlayer.GetComponent<Animator>().SetBool("inPeace", false);
    }

    [Command]
    public void ToggleWorldHud(bool toggle)
    {
        if(CloseCamera.activeSelf)
        {
            playerWorldHud.DOFade(0, 1f);
            return;
        }


        if(toggle)
        {
            playerWorldHud.DOFade(1, 0.4f);
        } else
        {
            playerWorldHud.DOFade(0, 1f);
        }

    }

    public void ToggleCraftingdHud(bool toggle)
    {
        if (toggle)
        {
            LegendaryCraft.io.UpdateInventory();
            Sequence mySequence = DOTween.Sequence()
            .AppendInterval(2.0f)
            .Append(craftingHud.DOFade(1, 1f))
            ;
        }
        else
        {
            craftingHud.DOFade(0, 1f);
        }

    }

    public void ToggleCookingdHud(bool toggle)
    {
        if (toggle)
        {
            LegendaryCraft.io.UpdateSatchel();
            Sequence mySequence = DOTween.Sequence()
            .AppendInterval(2.0f)
            .Append(cookingHud.DOFade(1, 1f))
            ;
        }
        else
        {
            cookingHud.DOFade(0, 1f);
        }

    }

    public void ToggleTanningdHud(bool toggle)
    {
        if (toggle)
        {
            LegendaryCraft.io.UpdateBasket();
            Sequence mySequence = DOTween.Sequence()
            .AppendInterval(2.0f)
            .Append(tanningHud.DOFade(1, 1f))
            ;
        }
        else
        {
            tanningHud.DOFade(0, 1f);
        }

    }


}
