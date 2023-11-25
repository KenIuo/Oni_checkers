using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public static void OnPlayClick()
    {
        GameManager.Instance.SoundManager.PlayMenuChoose();
        SceneManager.LoadScene("Arena1Scene");
    }

    public static void OnSettingsClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();

        if (GameManager.Instance.MainMenuScreen == null)
            GameManager.Instance.ChangeScreen(GameManager.Instance.SettingsScreen);
        else
            GameManager.Instance.ChangeScreen(GameManager.Instance.SettingsScreen, true);
    }

    public static void OnRulesClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.RulesScreen);
    }

    public static void OnCreditsClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.CreditsScreen);
    }

    public static void OnCreditsNameClick(string link)
    {
        Application.OpenURL(link);
    }

    public static void OnResumeClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen, false);
    }

    public static void OnBackClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();

        if (GameManager.Instance.MainMenuScreen == null)
            GameManager.Instance.ChangeScreen(GameManager.Instance.PauseScreen, true);
        else
            GameManager.Instance.ChangeScreen(GameManager.Instance.MainMenuScreen);
    }

    public static void OnMainMenuClick()
    {
        GameManager.Instance.SoundManager.PlayMenuBack();
        SceneManager.LoadScene("MainMenuScene");
    }

    public static void OnExitClick()
    {
        GameManager.Instance.SoundManager.PlayMenuBack();
        Application.Quit();
    }
}
