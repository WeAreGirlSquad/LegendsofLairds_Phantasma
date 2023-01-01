using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum LegendaryRace
{
    PHANTALIAN,
    BAKASHUR,
    ELSAVADI,
    SHEMILYAN,
    NUBALIAK
}

public enum LegendaryCharacterClass
{
    HUNTER_EXPLORER,
    WARRIOR_PROTECTOR,
    SHAMAN_HEALER
}

public class LegendaryCharacterGen : MonoBehaviour
{
    string actorname = "Doe";
    int gender = 0;
    LegendaryRace origin = LegendaryRace.PHANTALIAN;
    LegendaryCharacterClass actorclass = LegendaryCharacterClass.HUNTER_EXPLORER;
    int statIron = 3;
    int statMuscle = 2;
    int statHeart = 2;
    int statGuile = 1 ;
    int statIntuition = 1;
    EventSystem m_EventSystem;
    [SerializeField] GameObject inputField;

    [SerializeField] Transform placement;
    [SerializeField] GameObject CharacterGenDome;
    GameObject currentPlayerObject;

    // skin colors
    [SerializeField] GameObject[] male_phantalian;
    [SerializeField] GameObject[] male_bakashur;
    [SerializeField] GameObject[] male_elsavadi;
    [SerializeField] GameObject[] male_shemilyan;
    [SerializeField] GameObject[] male_nubaliak;
    
    [SerializeField] GameObject[] female_phantalian;
    [SerializeField] GameObject[] female_bakashur;
    [SerializeField] GameObject[] female_elsavadi;
    [SerializeField] GameObject[] female_shemilyan;
    [SerializeField] GameObject[] female_nubaliak;

    // obsolete
    [SerializeField] TextMeshProUGUI item_selected;
    [SerializeField] TextMeshProUGUI item_next;
    [SerializeField] TextMeshProUGUI item_before;
    // stats
    [SerializeField] TextMeshProUGUI iron_stat;
    [SerializeField] TextMeshProUGUI muscle_stat;
    [SerializeField] TextMeshProUGUI heart_stat;
    [SerializeField] TextMeshProUGUI guile_stat;
    [SerializeField] TextMeshProUGUI intuition_stat;

    [SerializeField] GameObject createButton;
    [SerializeField] GameObject progressBar;
    [SerializeField] Image progressBarFill;

    [SerializeField] Transform gameHeli;

    /*
    string warrior = "Warrior Protector";
    string hunter = "Hunter Explorer";
    string shaman = "Shaman Healer";
    */

    LegendaryPlayerInputActions playerLanderInput;

    void Awake()
    {
        iron_stat.text = "3";
        muscle_stat.text = "2";
        heart_stat.text = "2";
        guile_stat.text = "1";
        intuition_stat.text = "1";
        m_EventSystem = EventSystem.current;
        playerLanderInput = new LegendaryPlayerInputActions();
        // OK,
        UpdateCharacter();

    }

    private void OnEnable()
    {
        playerLanderInput.Lander.Enable();
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        playerLanderInput.Lander.Disable();

    #if UNITY_STANDALONE && !UNITY_EDITOR
        Cursor.visible = false;
    #endif
    
    }

    public void SetActorName(string name)
    {
        Debug.Log("name: " + name);
        actorname = name;
    }

    public void Start()
    {
        m_EventSystem.SetSelectedGameObject(inputField);
    }

    public void SetActorGender(int index)
    {
        Debug.Log("index: " + index);
        switch(index)
        {
            case 0:
                gender = 0;
                break;
            case 1:
                gender = 1;
                break;
            
        }
        UpdateCharacter();
    }

    public void SetActorClass(int index)
    {
        switch(index)
        {
            case 0:
                actorclass = LegendaryCharacterClass.HUNTER_EXPLORER;
                break;
            case 1:
                actorclass = LegendaryCharacterClass.WARRIOR_PROTECTOR;
                break;
            case 2:
                actorclass = LegendaryCharacterClass.SHAMAN_HEALER;
                break;
        }
        UpdateCharacter();
    }

    public void SetActorOrigin(int index)
    {
        switch(index)
        {
            case 0:
                origin = LegendaryRace.PHANTALIAN;
                break;
            case 1:
                origin = LegendaryRace.BAKASHUR;
                break;
            case 2:
                origin = LegendaryRace.ELSAVADI;
                break;
            case 3:
                origin = LegendaryRace.SHEMILYAN;
                break;
            case 4:
                origin = LegendaryRace.NUBALIAK;
                break;
        }
        UpdateCharacter();
        Debug.Log("index: " + index);
    }

    public void SetActorStats(int index)
    {
        //Debug.Log("index: " + index);

        // Five Paths

        int high = 3;
        int low = 1;
        int medium = 2;

        switch(index)
        {
            case 0:
                statIron = high;
                statMuscle = medium;
                statHeart = medium;
                statGuile = low;
                statIntuition = low;
                iron_stat.text = high.ToString();
                muscle_stat.text = medium.ToString();
                heart_stat.text = medium.ToString();
                guile_stat.text = low.ToString();
                intuition_stat.text = low.ToString();
                break;
            case 1:
                statIron = medium;
                statMuscle = high;
                statHeart = medium;
                statGuile = low;
                statIntuition = low;
                iron_stat.text = medium.ToString();
                muscle_stat.text = high.ToString();
                heart_stat.text = medium.ToString();
                guile_stat.text = low.ToString();
                intuition_stat.text = low.ToString();
                break;
            case 2:
                statIron = medium;
                statMuscle = low;
                statHeart = high;
                statGuile = low;
                statIntuition = medium;
                iron_stat.text = medium.ToString();
                muscle_stat.text = low.ToString();
                heart_stat.text = high.ToString();
                guile_stat.text = low.ToString();
                intuition_stat.text = medium.ToString();
                break;
            case 3:
                statIron = low;
                statMuscle = low;
                statHeart = medium;
                statGuile = high;
                statIntuition = medium;
                iron_stat.text = low.ToString();
                muscle_stat.text = low.ToString();
                heart_stat.text = medium.ToString();
                guile_stat.text = high.ToString();
                intuition_stat.text = medium.ToString();
                break;
            case 4:
                statIron = low;
                statMuscle = low;
                statHeart = medium;
                statGuile = medium;
                statIntuition = high;
                iron_stat.text = low.ToString();
                muscle_stat.text = low.ToString();
                heart_stat.text = medium.ToString();
                guile_stat.text = medium.ToString();
                intuition_stat.text = high.ToString();
                break;

        }

    }


    public void CreateCharacter()
    {
        Debug.Log("created!");

        currentPlayerObject.GetComponent<LegendaryActor>().currentHP = 50;
        currentPlayerObject.GetComponent<LegendaryActor>().maxHP = 50;
        currentPlayerObject.GetComponent<LegendaryActor>().currentPh = 0;
        currentPlayerObject.GetComponent<LegendaryActor>().maxPh = 5;
        currentPlayerObject.GetComponent<LegendaryActor>().damage = 5;
        currentPlayerObject.GetComponent<LegendaryActor>().actorName = actorname;
        currentPlayerObject.GetComponent<LegendaryActor>().actorLevel = 1;
        currentPlayerObject.GetComponent<LegendaryActor>().gender = gender;

        LegendaryActorStats mystats = new LegendaryActorStats();
        mystats.iron = statIron;
        mystats.muscle = statMuscle;
        mystats.heart = statHeart;
        mystats.guile = statGuile;
        mystats.intuition = statIntuition;
       
        currentPlayerObject.GetComponent<LegendaryPlayer>().UpdateStats(mystats);
        currentPlayerObject.transform.parent = null;
        //LegendaryCore.io.RunTransition();
        LegendaryCore.io.WarpObject(currentPlayerObject, gameHeli);
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.WARP_PHOENIX);
        //LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_SAILING);
        currentPlayerObject.GetComponent<CharacterController>().enabled = true;
        currentPlayerObject.GetComponent<LegendaryPlayer>().enabled = true;
        LegendaryCore.io.DayNightCyle();
        CharacterGenDome.SetActive(false);
        gameObject.SetActive(false);
        // update ActorInfo (LegendaryActor component)
        // update PlayerStats (player_hash / Legendary Actor Stats object in Legendary Player)
        // enable CharacterController
        // enable LegendaryPlayer
        // warp to (random) point

    }

    public void UpdateCharacter()
    {
        if(placement.childCount > 0)
        {
            Destroy(placement.GetChild(0).gameObject);
        }
        
        // female
        if(gender == 0)
        {
            int classindex = 0;
            switch(actorclass)
            {
                case LegendaryCharacterClass.HUNTER_EXPLORER:
                    classindex = 0;
                    break;
                case LegendaryCharacterClass.WARRIOR_PROTECTOR:
                    classindex = 1;
                    break;
                case LegendaryCharacterClass.SHAMAN_HEALER:
                    classindex = 2;
                    break;
            }
            switch(origin)
            {
                case LegendaryRace.PHANTALIAN:
                    currentPlayerObject = Instantiate(female_phantalian[classindex], placement);
                    break;
                case LegendaryRace.BAKASHUR:
                    currentPlayerObject = Instantiate(female_bakashur[classindex], placement);
                    break;
                case LegendaryRace.ELSAVADI:
                    currentPlayerObject = Instantiate(female_elsavadi[classindex], placement);
                    break;
                case LegendaryRace.SHEMILYAN:
                    currentPlayerObject = Instantiate(female_shemilyan[classindex], placement);
                    break;
                case LegendaryRace.NUBALIAK:
                    currentPlayerObject = Instantiate(female_nubaliak[classindex], placement);
                    break;

            }

        }

        // male
        if (gender == 1)
        {
            int classindex = 0;
            switch (actorclass)
            {
                case LegendaryCharacterClass.HUNTER_EXPLORER:
                    classindex = 0;
                    break;
                case LegendaryCharacterClass.WARRIOR_PROTECTOR:
                    classindex = 1;
                    break;
                case LegendaryCharacterClass.SHAMAN_HEALER:
                    classindex = 2;
                    break;
            }
            switch (origin)
            {
                case LegendaryRace.PHANTALIAN:
                    currentPlayerObject = Instantiate(male_phantalian[classindex], placement);
                    break;
                case LegendaryRace.BAKASHUR:
                    currentPlayerObject = Instantiate(male_bakashur[classindex], placement);
                    break;
                case LegendaryRace.ELSAVADI:
                    currentPlayerObject = Instantiate(male_elsavadi[classindex], placement);
                    break;
                case LegendaryRace.SHEMILYAN:
                    currentPlayerObject = Instantiate(male_shemilyan[classindex], placement);
                    break;
                case LegendaryRace.NUBALIAK:
                    currentPlayerObject = Instantiate(male_nubaliak[classindex], placement);
                    break;

            }

        }



        /*
        // skin colors
        [SerializeField] GameObject[] male_phantalian;
        [SerializeField] GameObject[] male_bakashur;
        [SerializeField] GameObject[] male_elsavadi;
        [SerializeField] GameObject[] male_shemilyan;
        [SerializeField] GameObject[] male_nubaliak;

        [SerializeField] GameObject[] female_phantalian;
        [SerializeField] GameObject[] female_bakashur;
        [SerializeField] GameObject[] female_elsavadi;
        [SerializeField] GameObject[] female_shemilyan;
        [SerializeField] GameObject[] female_nubaliak;
        */




    }



}
