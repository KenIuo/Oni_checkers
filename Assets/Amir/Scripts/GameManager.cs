using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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



    public void ChangeScreen(GameObject screen, bool set_pause = false, bool disable = true)
    {
        if (_currentScreen != null)
            _currentScreen.SetActive(!disable);

        _currentScreen = screen;
        _currentScreen.SetActive(true);

        TurnSystem.Instance.LockAllCheckers(set_pause);
    }



    void Awake()
    {
        _currentScreen = MainMenuScreen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentScreen == SettingsScreen)
                ButtonScript.OnBackClick();
            else if (_currentScreen == CreditsScreen)
                ButtonScript.OnBackClick();
            else if (_currentScreen == PauseScreen)
                ChangeScreen(GameScreen, false);
            else if (_currentScreen == GameScreen)
                ChangeScreen(PauseScreen, true, false);
            else if (_currentScreen == LoseScreen)
                ButtonScript.OnMainMenuClick();
            else if (_currentScreen == WinScreen)
                ButtonScript.OnMainMenuClick();
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
