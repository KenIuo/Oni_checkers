using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public CheckerController _playerCheckerObject;

    internal List<Transform> _markers = new();
    internal List<CheckerController> _playersQueue = new ();
    internal byte _currentPlayer = 4;
    internal byte _playerID;
    internal bool _didMoved = false;
    internal bool _gameStarted = false;
    //[NonSerialized] public bool _canMove = false;

    [SerializeField] GameObject GameScreen;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject WinScreen;

    List<CheckerController> _isReadyList = new ();
    //DeathByFall _deathByFall;
    //byte _isAllReady = 0;

    List<CheckerController> _eliminationQueue = new(); // ? � ������ DeathByFall ������ ?



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

    public void AddToLists(CheckerController checker, Transform marker)
    {
        if (!_playersQueue.Contains(checker))
        {
            _playersQueue.Add(checker);
            _markers.Add(marker);
            checker.OnCheckerReady.AddListener(CheckConditions);
        }
    }

    public void ResetMarkers()
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
        {
            _playersQueue[i]._marker.SetActive(true);
            _markers[i].position = new Vector3(_markers[0].position.x + (i * 140), Screen.height);
        }
    }

    public void SetCheckerReady(CheckerController checker, bool state)
    {
        if (state)
        {
            if (!_isReadyList.Contains(checker))
                _isReadyList.Add(checker);

            if (checker._state.Equals(CheckerState.Died)
            && !_eliminationQueue.Contains(checker))
                _eliminationQueue.Add(checker);
        }
        else
        {
            if (_isReadyList.Contains(checker))
                _isReadyList.RemoveAt(_isReadyList.IndexOf(checker));
        }

        CheckConditions();
    }

    public void CheckConditions()
    {
        if (_isReadyList.Count == _playersQueue.Count) // ���� ������� ��������, �� �������� ����� ���
        {
            //_deathByFall.CheckQueue(); // ���������� ����� ��� �������� ?

            // �������� ����� ����������
            CheckQueue();
            CheckEndOfGameConditions();
        }
    }

    public void NewTurn()
    {
        _isReadyList.Clear();

        if (_currentPlayer >= _playersQueue.Count)
        {
            _currentPlayer = 0;
            _markers[_currentPlayer].position = new Vector3(_markers[_currentPlayer].position.x, Screen.height);
            SetMassToOther(1);
        }
        else
        {
            _playersQueue[_currentPlayer].ChangeGameStat(false);

            _markers[_currentPlayer].position = new Vector3(_markers[_currentPlayer].position.x, Screen.height);
            SetMassToOther(1);
            ++_currentPlayer;
        }

        _playersQueue[_currentPlayer].ChangeGameStat(true);
        _didMoved = false;

        ResetStates();

        _markers[_currentPlayer].position = new Vector3(_markers[_currentPlayer].position.x, Screen.height - 80);
        // ������ ��������� (�������� ������� + ��������� ������ ������ ������ + ��������� ������ ������� + ������� ������ �������� �������)

        if (_playersQueue[_currentPlayer] != _playerCheckerObject) // ���� ������� - �� �����
            _playersQueue[_currentPlayer].ChooseAttackVector(); // �������� ������� ������ ������� � ��������
        //else // �����
            //_currentPlayer = _currentPlayer; // �������������� ����������
    }

    public void SetMassToOther(float mass)
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
            if (i != _currentPlayer)
                _playersQueue[i].SetMass(mass);
    }

    public void CheckEndOfGameConditions() // ��������� ����������� �� ������� ������
    {
        //_playersQueue.RemoveAt(_playersQueue.IndexOf(checker)); // ?
        //GetDeathByFall().OnEnter(checker.gameObject);

        if (_playersQueue.Count == 1)
        {
            GameScreen.SetActive(false);

            if (_playersQueue[0] == _playerCheckerObject)
                WinScreen.SetActive(true);
            else
                LoseScreen.SetActive(true);
        }
        else
            NewTurn();

        //CheckConditions();
    }

    //public DeathByFall GetDeathByFall()
    //{
        //return _deathByFall;
    //}

    void ResetStates()
    {
        foreach (CheckerController checker in _playersQueue)
        {
            if (checker == _playersQueue[_currentPlayer])
                checker.SetState(CheckerState.Turning);
            else
                checker.SetState(CheckerState.Standing);
        }
    }

    void CheckQueue() // ������� ������� �� �����
    {
        if (_eliminationQueue.Count > 1
        &&  _eliminationQueue.Contains(_playersQueue[_currentPlayer]))
        {
            foreach (CheckerController player in _eliminationQueue)
                player.transform.position = player._standardPosition.position;
        }
        else if (_eliminationQueue.Count > 0)
        {
            if (_eliminationQueue[0] == _playerCheckerObject)
            {
                GameScreen.SetActive(false);
                LoseScreen.SetActive(true);
            }

            // _eliminationQueue[0].transform.position; // ��������������� ����� ������
            // ������������ �� ������� �����
            foreach (CheckerController player in _eliminationQueue)
                _playersQueue.RemoveAt(_playersQueue.IndexOf(player));

            // ���������� ������� ������� � ����������
        }

        _eliminationQueue.Clear();
    }



    //void Awake()
    //{
        //Instance.GetDeathByFall();
        //for (byte i = 0; i < 4; i++)
        //_gameScreen[i] = GameObject.Find("Game").transform.GetChild(i).gameObject.transform;
        //GameObject.Find("Game");.transform.GetChild(1).gameObject.SetActive(false);
    //}
}