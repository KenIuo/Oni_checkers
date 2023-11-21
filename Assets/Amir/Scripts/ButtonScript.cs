using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEditor.Networking.PlayerConnection;
using Unity.VisualScripting;

public class ButtonScript : MonoBehaviour
{
    public static void OnPlayClick()
    {
        GameManager.Instance.SoundManager.PlayMenuChoose();
        //SceneManager.LoadScene("Arena1Scene");
        //GameManager.Instance.ChangeScreen(GameManager.Instance.MainMenuScreen);

        TurnSystem.Instance.ResetMarkers();
        EventSystem.Instance.StartGame(true);
        TurnSystem.Instance.NewTurn();

        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);
    }

    public static void OnRestartClick()
    {
        GameManager.Instance.SoundManager.PlayMenuChoose();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.Instance.ChangeScreen(GameManager.Instance.MainMenuScreen);
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);
    }

    public static void OnSettingsClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.SettingsScreen);
    }

    public static void OnCreditsClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.CreditsScreen);
    }

    public static void OnResumeClick()
    {
        GameManager.Instance.SoundManager.PlayMenuTransit();
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);
    }

    public static void OnMainMenuClick()
    {
        GameManager.Instance.SoundManager.PlayMenuBack();
        GameManager.Instance.ChangeScreen(GameManager.Instance.MainMenuScreen);
    }

    public static void OnExitClick()
    {
        GameManager.Instance.SoundManager.PlayMenuBack();
        Application.Quit();
    }
}