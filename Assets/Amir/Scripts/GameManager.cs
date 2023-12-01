using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject SettingsScreen;
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



    public void ChangeScreen(GameObject screen, bool set_pause)
    {
        ChangeScreen(screen);

        TurnSystem.Instance.LockAllCheckers(set_pause);
    }

    public void ChangeScreen(GameObject screen)
    {
        if (_currentScreen != null)
            _currentScreen.SetActive(false);

        _currentScreen = screen;
        _currentScreen.SetActive(true);
    }



    void Awake()
    {
        _currentScreen = GameScreen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                 if (_currentScreen == SettingsScreen)
                ButtonScript.OnBackClick();
            else if (_currentScreen == LoseScreen)
                ButtonScript.OnMainMenuClick();
            else if (_currentScreen == WinScreen)
                ButtonScript.OnMainMenuClick();
            else if (_currentScreen == PauseScreen)
                ChangeScreen(GameScreen, false);
            else if (_currentScreen == GameScreen)
                ChangeScreen(PauseScreen, true);
        }

        SoundManager.Update();
    }
}
