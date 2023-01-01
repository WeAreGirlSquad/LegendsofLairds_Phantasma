using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Erdcsharp.Provider.Dtos;
using System.Linq;

public class LegendsofLairds_BIOS : MonoBehaviour
{
    public static LegendsofLairds_BIOS io;

    private bool ready = false;
    AccountDto playerAccount;
    public GameObject connectButton;
    [SerializeField] bool mobileActive = false;

    private void Awake()
    {
        if(io == null)
        {
            io = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(io);
        }
    }

    public void ShowConnectButton()
    {
        connectButton.SetActive(true);
    }

    public void SetPlayerElrondAccount(AccountDto dto)
    {
        playerAccount = dto;
    }

    public bool isMobile()
    {
        return mobileActive;
    }

    public AccountDto GetPlayerElrondAccount()
    {
        return playerAccount;
    }

    private void Start()
    {
        StartCoroutine(LoadLegendsofLairds());
    }

    public void RuntimeBIOS()
    {
        Debug.Log("BIOS OK.");
    }

    public void Boot()
    {

        ready = true;
        
    }

    public bool isReady()
    {
        return ready;
    }

    IEnumerator LoadLegendsofLairds()
    {
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            
            // Check if the load has finished
            if (asyncOperation.progress >= 0.90f)
            {
                if (ready)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }





}
