using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour
{
    internal GameObject _previousScreen;



    public static void OnPlayClick()
    {
        UIManager.Instance.PlayMenuChoose();
        SceneManager.LoadScene("Arena1Scene");
    }

    public static void OnMainMenuClick()
    {
        UIManager.Instance.PlayMenuBack();
        SceneManager.LoadScene("MainMenuScene");
    }

    public static void OnExitClick()
    {
        UIManager.Instance.PlayMenuBack();
        Application.Quit();
    }



    public static void OnSettingsClick()
    {
        UIManager.Instance.PlayMenuTransit();

        if (UIManager.Instance.MainMenuScreen != null)
            UIManager.Instance.ChangeScreen(UIManager.Instance.SettingsScreen);
        else
            GameManager.Instance.ChangeScreen(GameManager.Instance.SettingsScreen, true);
    }

    public static void OnBackClick()
    {
        UIManager.Instance.PlayMenuTransit();

        if (UIManager.Instance.MainMenuScreen != null)
            UIManager.Instance.ChangeScreen(UIManager.Instance.MainMenuScreen);
        else
            GameManager.Instance.ChangeScreen(GameManager.Instance.PauseScreen, true);
    }



    public static void OnRulesClick()
    {
        UIManager.Instance.PlayMenuTransit();
        UIManager.Instance.ChangeScreen(UIManager.Instance.RulesScreen);
    }

    public static void OnCreditsClick()
    {
        UIManager.Instance.PlayMenuTransit();
        UIManager.Instance.ChangeScreen(UIManager.Instance.CreditsScreen);
    }



    public static void OnResumeClick()
    {
        UIManager.Instance.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen, false);
    }



    public static void OnCreditsNameClick(string link)
    {
        Application.OpenURL(link);
    }



    void OnMouseEnter()
    {
        CursorManager.Instance.SetHandCursor();
    }

    void OnMouseExit()
    {
        CursorManager.Instance.SetStandartCursor();
    }

    void OnMouseOver()
    {
        CursorManager.Instance.SetHandCursor();
    }
}
