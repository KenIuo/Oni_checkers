using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public CheckerController _playerCheckerObject;
    public List<GameObject> players;

    internal List<GameObject> _markers = new();
    internal List<CheckerController> _playersQueue = new ();
    internal byte _currentPlayer = 4;
    internal byte _playerID;
    internal bool _didMoved = false;
    internal bool _gameStarted = false;
    //[NonSerialized] public bool _canMove = false;

    List<CheckerController> _isReadyList = new ();
    //byte _isAllReady = 0;

    List<CheckerController> _eliminationQueue = new(); // ?



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

    public void AddToLists(CheckerController checker, GameObject marker)
    {
        if (!_playersQueue.Contains(checker))
        {
            _playersQueue.Add(checker);
            _markers.Add(marker);
            checker.onCheckerReady.AddListener(CheckConditions);
        }
    }

    public void ResetMarkers()
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
        {
            _markers[i].SetActive(true);
            _markers[i].transform.position = new Vector3(_markers[0].transform.position.x + (i * 140), Screen.height);
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
        if (_isReadyList.Count == _playersQueue.Count) // если условия подходят, то вызывать новый ход
        {
            //_deathByFall.CheckQueue(); // правильное место для проверки ?

            // вызывать смену интерфейса
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
            _markers[_currentPlayer].transform.position = new Vector3(_markers[_currentPlayer].transform.position.x, Screen.height);
            SetMassToOther(1);
        }
        else
        {
            _playersQueue[_currentPlayer].ChangeGameStat(false);

            _markers[_currentPlayer].transform.position = new Vector3(_markers[_currentPlayer].transform.position.x, Screen.height);
            SetMassToOther(1);
            ++_currentPlayer;
        }

        _playersQueue[_currentPlayer].ChangeGameStat(true);
        _didMoved = false;

        ResetStates();

        _markers[_currentPlayer].transform.position = new Vector3(_markers[_currentPlayer].transform.position.x, Screen.height - 80);
        // менять интерфейс (выводить надпись + выдвигать нужную иконку игрока + задвигать других игроков + удалять иконки удалённых игроков)

        if (_playersQueue[_currentPlayer] != _playerCheckerObject) // если ведущий - НЕ игрок
            _playersQueue[_currentPlayer].ChooseAttackVector(); // вызывать функцию выбора вектора и выстрела
        //else // иначе
            //_currentPlayer = _currentPlayer; // разблокировать управление
    }

    public void SetMassToOther(float mass)
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
            if (i != _currentPlayer)
                _playersQueue[i].SetMass(mass);
    }

    public void CheckEndOfGameConditions() // проверяет выполнились ли условия победы
    {
        //_playersQueue.RemoveAt(_playersQueue.IndexOf(checker)); // ?
        //GetDeathByFall().OnEnter(checker.gameObject);

        if (_playersQueue.Count == 1)
        {
            if (_playersQueue[0] == _playerCheckerObject)

                GameManager.Instance.ChangeScreen(GameManager.Instance.WinScreen);
            else
                GameManager.Instance.ChangeScreen(GameManager.Instance.LoseScreen);
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

    void CheckQueue() // удаляет игроков со сцены
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
                GameManager.Instance.ChangeScreen(GameManager.Instance.LoseScreen);

            // _eliminationQueue[0].transform.position; // воспроизведение звука смерти
            // исчезновение из системы ходов
            foreach (CheckerController player in _eliminationQueue)
                _playersQueue.RemoveAt(_playersQueue.IndexOf(player));

            // обновление очереди игроков в интерфейсе
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
