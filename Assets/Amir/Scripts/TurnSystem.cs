using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TurnSystem : MonoBehaviour
{
    internal List<CheckerController> _playersQueue = new ();
    internal byte _currentPlayer = 3;
    internal bool _gameStarted = false;
    internal bool _isOnPause { get; private set; } = false;

    [SerializeField] GameObject _title;

    List<GameObject> _markers = new();
    List<CheckerController> _eliminationQueue = new();
    List<CheckerController> _isReadyList = new ();
    TextMeshPro _textMeshPro;
    bool _isReady = false;



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



    public void LockAllCheckers(bool lock_all_checkers)
    {
        _isOnPause = lock_all_checkers;
    }

    public void AddToLists(CheckerController checker, GameObject marker)
    {
        if (!_playersQueue.Contains(checker))
        {
            _playersQueue.Add(checker);
            _markers.Add(marker);
            checker.onCheckerReady.AddListener(CheckConditions);

            if (_playersQueue.Count == 4)
            {
                //_textMeshPro = _title.GetComponent<TextMeshPro>();
                //_textMeshPro.text = "start game";
                Invoke(nameof(StartGame), 2.0f);
            }
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

        _markers[_currentPlayer].transform.localPosition = new Vector3(_markers[_currentPlayer].transform.localPosition.x, 540);
        SetMassToOther(1);

        if (_currentPlayer >= _playersQueue.Count - 1)
            _currentPlayer = 0;
        else
            ++_currentPlayer;

        ResetStates();

        _markers[_currentPlayer].transform.localPosition = new Vector3(_markers[_currentPlayer].transform.localPosition.x, 460);
        // менять интерфейс (выводить надпись + выдвигать нужную иконку игрока + задвигать других игроков + удалять иконки удалённых игроков)

        if (!_playersQueue[_currentPlayer]._isPlayer) // если ведущий - НЕ игрок
            _isReady = true;
    }

    public void ResetMarkers()
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
        {
            _markers[i].SetActive(true);

            _markers[0].transform.localPosition = new Vector3(_markers[0].transform.localPosition.x, 540);
            //_markers[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0), new Quaternion()); // _markers[i].transform.localPosition.x + (i * 140)
        }
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
            if (_playersQueue[0]._isPlayer)

                GameManager.Instance.ChangeScreen(GameManager.Instance.WinScreen, true);
            else
                GameManager.Instance.ChangeScreen(GameManager.Instance.LoseScreen, true);
        }
        else
            NewTurn();

        //CheckConditions();
    }



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
                player.ResetPosition();

            _eliminationQueue.Clear();
        }
        else if (_eliminationQueue.Count > 0) /////////////////////////////////////////////////////////////////////// перепроверить условия
        {
            // _eliminationQueue[0].transform.position; // воспроизведение звука смерти
            // исчезновение из системы ходов
            foreach (CheckerController player in _eliminationQueue)
            {
                _markers[_playersQueue.IndexOf(player)].SetActive(false);
                _playersQueue.RemoveAt(_playersQueue.IndexOf(player));
            }

            if (_eliminationQueue[0]._isPlayer)
                GameManager.Instance.ChangeScreen(GameManager.Instance.LoseScreen, true);

            // обновление очереди игроков в интерфейсе
            _eliminationQueue.Clear();
        }
    }

    void StartGame()
    {
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);

        //EmptyTitle();
        ResetMarkers();
        NewTurn();
    }

    void EmptyTitle()
    {
        _textMeshPro.text = "";
    }



    void Update()
    {
        if (!_isOnPause && _isReady)
        {
            _playersQueue[_currentPlayer].ChooseAttackVector();
            _isReady = false;
        }
    }
}
