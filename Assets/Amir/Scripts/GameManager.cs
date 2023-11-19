using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject SettingsScreen;
    public GameObject CreditsScreen;
    public GameObject GameScreen;
    public GameObject PauseScreen;
    public GameObject LoseScreen;
    public GameObject WinScreen;

    [SerializeField] SoundManager _soundManager;

    GameObject _currentScreen;



    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }
    #endregion



    public SoundManager SoundManager => _soundManager;



    public void ChangeScreen(GameObject screen, bool disable = true)
    {
        _currentScreen.SetActive(!disable);
        _currentScreen = screen;
        _currentScreen.SetActive(true);
    }



    void Awake()
    {
        _currentScreen = MainMenuScreen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentScreen == MainMenuScreen)
            {
                ButtonScript.OnExitClick();
            }
            if (_currentScreen == SettingsScreen)
            {
                ChangeScreen(MainMenuScreen);
            }
            if (_currentScreen == CreditsScreen)
            {
                ChangeScreen(MainMenuScreen);
            }
            if (_currentScreen == GameScreen)
            {
                ChangeScreen(PauseScreen, false);
            }
            if (_currentScreen == PauseScreen)
            {
                ChangeScreen(GameScreen);
            }
            if (_currentScreen == LoseScreen)
            {
                ChangeScreen(MainMenuScreen);
            }
            if (_currentScreen == WinScreen)
            {
                ChangeScreen(MainMenuScreen);
            }
        }

        SoundManager.Update();
    }
}

[Serializable]
public class SoundManager
{
    [SerializeField] AudioSource audioHit1;
    [SerializeField] AudioSource audioHit2;
    [SerializeField] AudioSource audioDeath;
    [SerializeField] AudioSource audioLaunch;
    [SerializeField] AudioSource audioCollide1;
    [SerializeField] AudioSource audioCollide2;
    [SerializeField] AudioSource audioMenuChoose;
    [SerializeField] AudioSource audioMenuTransit;
    [SerializeField] AudioSource audioMenuBack;
    [SerializeField] float soundRate;

    float _soundRate1 = 1, _soundRate2 = 1;

    public void PlayHitSound()
    {
        if (_soundRate1 >= soundRate)
        {
            if (audioHit1.isPlaying)
                audioHit2.Play();
            else
                audioHit1.Play();

            _soundRate1 = 0;
        }
    }

    public void PlayCollideSound()
    {
        if (_soundRate2 >= soundRate)
        {
            if (audioCollide1.isPlaying)
                audioCollide2.Play();
            else
                audioCollide1.Play();

            _soundRate2 = 0;
        }
    }

    public void PlayDeathSound()
    {
        audioDeath.Play();
    }

    public void PlayLaunchSound()
    {
        audioLaunch.Play();
    }

    public void PlayMenuChoose()
    {
        audioMenuChoose.Play();
    }

    public void PlayMenuTransit()
    {
        audioMenuTransit.Play();
    }

    public void PlayMenuBack()
    {
        audioMenuBack.Play();
    }

    public void Update()
    {
        if (_soundRate1 < soundRate)
            _soundRate1 += Time.deltaTime;

        if (_soundRate2 < soundRate)
            _soundRate2 += Time.deltaTime;
    }
}
