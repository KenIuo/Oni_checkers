using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public static void OnPlayClick()
    {
        TurnSystem.Instance.ResetMarkers();
        EventSystem.Instance.StartGame(true);

        //TurnSystem.Instance._canMove = true;
        //TurnSystem.Instance.ShuffleListOfPlayers();
        TurnSystem.Instance.NewTurn();

        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);
    }

    public static void OnRestartClick()
    {

    }

    public static void OnSettingsClick()
    {
        //GameManager.Instance.ChangeScreen(GameManager.Instance.SettingsScreen);
    }

    public static void OnCreditsClick()
    {
        //GameManager.Instance.ChangeScreen(GameManager.Instance.CreditsScreen);
    }

    public static void OnResumeClick()
    {
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);
    }

    public static void OnMainMenuClick()
    {
        GameManager.Instance.ChangeScreen(GameManager.Instance.MainMenuScreen);
    }

    public static void OnExitClick()
    {
        Application.Quit();
    }
}
