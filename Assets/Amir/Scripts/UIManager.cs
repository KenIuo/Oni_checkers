using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject SettingsScreen;
    public GameObject CreditsScreen;
    public GameObject RulesScreen;
    public GameObject ArenaSelectionScreen;

    [SerializeField] AudioSource audioMenuChoose;
    [SerializeField] AudioSource audioMenuTransit;
    [SerializeField] AudioSource audioMenuBack;

    GameObject _currentScreen;



    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            return _instance;
        }
    }
    #endregion



    public void ChangeScreen(GameObject screen)
    {
        CursorManager.Instance.SetStandartCursor();

        if (_currentScreen != null)
            _currentScreen.SetActive(false);

        _currentScreen = screen;
        _currentScreen.SetActive(true);
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



    void Awake()
    {
        _currentScreen = MainMenuScreen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
        &&  MainMenuScreen != null)
        {
            if (_currentScreen == ArenaSelectionScreen)
                ButtonScript.OnBackClick();
            else if (_currentScreen == SettingsScreen)
                ButtonScript.OnBackClick();
            else if (_currentScreen == CreditsScreen)
                ButtonScript.OnBackClick();
            else if (_currentScreen == RulesScreen)
                ButtonScript.OnBackClick();
        }
    }
}
