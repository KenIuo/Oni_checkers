using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    [NonSerialized] public List<GameObject> _playersQueue = new ();
    [NonSerialized] public byte _currentPlayer = 4;
    [NonSerialized] public bool _canMove = false;

    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject WinScreen;

    public GameObject _playerCheckerObject;
    public List<Transform> _gameScreen = new();

    List<bool> _isReadyList = new (4);
    DeathByFall _deathByFall;



    #region Singleton
    private static TurnSystem _instance;
    public static TurnSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TurnSystem>();

            return _instance;
        }
    }
    #endregion

    /// <summary>
    ///  должен вызываться при старте игры (т.е. после нажатия на кнопку играть в гл. меню)
    /// </summary>
    public void ShuffleListOfPlayers()
    {
        //_playersQueue.Shuffle();

        foreach (GameObject player in _playersQueue)
        {
            player.SetActive(true);

            if (player != _playerCheckerObject)
                player.AddComponent<AIScript>();
        }
    }

    public void CheckConditions()
    {
        byte _is_all_ready = 0;

        foreach (GameObject player in _playersQueue)
        {
            if (!player.activeSelf || player.GetComponent<CheckerState>()._isStopped)
                _is_all_ready++;
        }

        if (_is_all_ready == _playersQueue.Count) // если условия подходят, то вызывать новый ход
        {
            _deathByFall.CheckQueue(); // правильное место для проверки ?
            //NewTurn();
        }

        if (_playersQueue.Count == 1)
        {
            if (_playersQueue[0] == _playerCheckerObject)
                GameObject.Find("Win").SetActive(true);
            else
                GameObject.Find("Lose").SetActive(true);
        }
    }

    public DeathByFall GetDeathByFall()
    {
        return _deathByFall;
    }



    public void NewTurn()
    {
        if (_currentPlayer == 4)
            _currentPlayer = 0;
        else
            ++_currentPlayer;

        _gameScreen[_currentPlayer].position = new Vector3(_gameScreen[_currentPlayer].position.x, 940);
        // менять интерфейс (выводить надпись + выдвигать нужную иконку игрока + задвигать других игроков + удалять иконки удалённых игроков)

        if (_playersQueue[_currentPlayer] == _playerCheckerObject) // если ведущий - игрок
            _currentPlayer = _currentPlayer; // разблокировать управление
        else // иначе
            _playersQueue[_currentPlayer].GetComponent<AIScript>().ChooseAttackVector(); // разблокировать компонент ai ИЛИ вызывать функцию выбора вектора и выстрела
    }



    /*private void Awake()
    {
        //for (byte i = 0; i < 4; i++)
            //_gameScreen[i] = GameObject.Find("Game").transform.GetChild(i).gameObject.transform;
        //GameObject.Find("Game");.transform.GetChild(1).gameObject.SetActive(false);
    }*/
}
