using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public GameObject _playerCheckerObject;
    public List<Transform> _gameScreen = new();

    internal List<CheckerController> _playersQueue = new ();
    internal byte _currentPlayer = 4;
    internal byte _playerID;
    internal bool _didMoved = false;
    //[NonSerialized] public bool _canMove = false;

    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject WinScreen;

    List<bool> _isReadyList = new (4);
    DeathByFall _deathByFall;
    byte _isAllReady = 0;

    List<GameObject> _eliminationQueue = new(); // ? � ������ DeathByFall ������ ?



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



    /*public void ShuffleListOfPlayers()
    {
        //_playersQueue.Shuffle();

        for (byte i = 0; i < _playersQueue.Count; i++)
        {
            _playersQueue[i].SetActive(true);

            //if (_playersQueue[i] == _playerCheckerObject)
                //_playerID = i;
        }
    }*/

    public void AddToList(CheckerController checker)
    {
        if (!_playersQueue.Contains(checker))
        {
            _playersQueue.Add(checker);
            checker.OnCheckerReady.AddListener(CheckConditions);
        }
    }

    public void CheckConditions(CheckerController player)
    {
        _isAllReady++;

        _playersQueue.RemoveAt(_playersQueue.IndexOf(player)); // ?

        if (_didMoved && _isAllReady == _playersQueue.Count) // ���� ������� ��������, �� �������� ����� ���
        {
            _deathByFall.CheckQueue(); // ���������� ����� ��� �������� ?
            
            // �������� ����� ����������

            NewTurn();
        }
    }

    public void NewTurn()
    {
        CheckerController checker_controller;

        if (_currentPlayer >= 4)
        {
            _currentPlayer = 0;
            _gameScreen[_currentPlayer].position = new Vector3(_gameScreen[_currentPlayer].position.x, 1040);
            SetMassToOther(1);
        }
        else
        {
            checker_controller = _playersQueue[_currentPlayer];
            checker_controller._gameStarted = false;

            _gameScreen[_currentPlayer].position = new Vector3(_gameScreen[_currentPlayer].position.x, 1040);
            SetMassToOther(1);
            ++_currentPlayer;
        }

        checker_controller = _playersQueue[_currentPlayer];
        //checker_controller._isStopped = false;
        checker_controller._gameStarted = true;
        _didMoved = false;

        _gameScreen[_currentPlayer].position = new Vector3(_gameScreen[_currentPlayer].position.x, 940);
        // ������ ��������� (�������� ������� + ��������� ������ ������ ������ + ��������� ������ ������� + ������� ������ �������� �������)

        if (_playersQueue[_currentPlayer] != _playerCheckerObject) // ���� ������� - �� �����
            checker_controller.ChooseAttackVector(); // �������� ������� ������ ������� � ��������
        //else // �����
            //_currentPlayer = _currentPlayer; // �������������� ����������
    }

    public void SetMassToOther(float mass)
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
            if (i != _currentPlayer)
                _playersQueue[i].GetComponent<Rigidbody>().mass = mass;
    }

    public void CheckEndOfGameConditions(CheckerController checker)
    {
        checker.SetAlive(false);
        //GetDeathByFall().OnEnter(checker.gameObject);

        if (_playersQueue.Count == 1)
        {
            if (_playersQueue[0] == _playerCheckerObject)
                GameObject.Find("Win").SetActive(true);
            else
                GameObject.Find("Lose").SetActive(true);
        }
        //else
            //CheckConditions(checker);

    }

    public DeathByFall GetDeathByFall()
    {
        return _deathByFall;
    }



    void Awake()
    {
        //Instance.GetDeathByFall();
        //for (byte i = 0; i < 4; i++)
        //_gameScreen[i] = GameObject.Find("Game").transform.GetChild(i).gameObject.transform;
        //GameObject.Find("Game");.transform.GetChild(1).gameObject.SetActive(false);
    }
}
