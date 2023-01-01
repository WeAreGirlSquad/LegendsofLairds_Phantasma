using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using QFSW.QC;
using QFSW.QC.Actions;
using UnityEngine.InputSystem;

public class LegendaryBoot : MonoBehaviour
{
    [SerializeField] GameObject bootLoader;
    [SerializeField] GameObject bootLoader_action;
    [SerializeField] CanvasGroup bootLoaderCanvasGroup;
    [SerializeField] Image bootLoaderBackground;
    [SerializeField] Image bootLoaderLogo;
    [SerializeField] GameObject logoHeader;
    [SerializeField] GameObject bootCamera;
    [SerializeField] GameObject timeLine;
    [SerializeField] GameObject press_start;

    public static LegendaryBoot io;
    LegendaryPlayerInputActions playerLanderInput;
    bool isActionOnePressed;
    float cooldown_actionone = 0.0f;
    float cooldown_boot;
    bool startingGame = false;

    [SerializeField] GameObject characterSelection;
    [SerializeField] GameObject characterGenerator;

    void OnEnable()
    {
        playerLanderInput.Lander.Enable();
    }

    void OnDisable()
    {
        playerLanderInput.Lander.Disable();
    }

    private void Awake()
    {
        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }
        LegendaryAudio.io.BattleCrossfade(false);
        playerLanderInput = new LegendaryPlayerInputActions();
        playerLanderInput.Lander.ActionOne.started += onActionOne;
        playerLanderInput.Lander.ActionOne.performed += onActionOne;
        playerLanderInput.Lander.ActionOne.canceled += onActionOne;
        cooldown_boot = Time.time + 8.0f;
    }

    void onActionOne(InputAction.CallbackContext context)
    {
        isActionOnePressed = context.ReadValueAsButton();
    }

    private void Update()
    {
        if(cooldown_boot > Time.time)
        {
            return;
        }

        if(isActionOnePressed)
        {
            if(cooldown_actionone > Time.time)
            {
                return;
            }
            cooldown_actionone = Time.time + 0.12f;

            if(startingGame)
            {
                return;
            } else
            {
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.START_GAME);
                StartGame();
            }
        }
    }

    private void Start()
    {
        StartCoroutine(BootToGame());
       
    }

    [Command]
    public void StartGame()
    {
        startingGame = true;
        bootLoaderCanvasGroup.DOKill();
        bootLoaderCanvasGroup.DOFade(1f, 1f);
        StartCoroutine(ClearBoot());   
    }

    IEnumerator ClearBoot()
    {
        press_start.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        characterSelection.SetActive(true);
        //characterGenerator.SetActive(true);
        timeLine.SetActive(false);
        bootCamera.SetActive(false);
        bootLoader_action.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        bootLoaderCanvasGroup.DOFade(0f, 1f);
        LegendaryAudio.io.PlayMusic(LegendaryAudioType.CHARACTER_CREATE_THEME);
        yield return new WaitForSeconds(1.2f);
        characterGenerator.SetActive(true);
        bootLoader.SetActive(false);
        yield return null;
    }

    IEnumerator BootToGame()
    {
        bootLoaderBackground.DOColor(Color.white, 4.0f);
        yield return new WaitForSeconds(2.0f);
        LegendaryAudio.io.PlayMusic(LegendaryAudioType.WORLD_THEME_INTRO);
        bootLoaderLogo.DOColor(Color.white, 4.0f);
        yield return new WaitForSeconds(4.0f);
        bootLoaderCanvasGroup.DOFade(0f, 2.5f);
        timeLine.SetActive(true);
        yield return new WaitForSeconds(8.0f);
        press_start.SetActive(true);
        yield return null;
    }


}
