using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonScript : MonoBehaviour
{
    [SerializeField] GameObject _currentScreen;
    [SerializeField] GameObject _screenToOpen;

    public void OnPlayClick()
    {
        _currentScreen.SetActive(false);
        _screenToOpen.SetActive(true);

        TurnSystem.Instance._canMove = true;
        TurnSystem.Instance.ShuffleListOfPlayers();
        TurnSystem.Instance.NewTurn();
    }
}
