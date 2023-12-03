using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    bool _changePlayer = true;



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

            if (_playersQueue.Count == 4)
                Invoke(nameof(StartGame), 2.0f);
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

        if (_isReadyList.Count == _playersQueue.Count) // если условия подходят, то вызывать новый ход
        {
            CheckQueue();
            CheckConditions();
            NewTurn();
        }
    }

    public void NewTurn()
    {
        _isReadyList.Clear();

        _markers[_currentPlayer].transform.localPosition = new Vector3(_markers[_currentPlayer].transform.localPosition.x, 540);
        SetMassToOther(1);

        if (_changePlayer)
        {
            if (_currentPlayer >= _playersQueue.Count - 1)
                _currentPlayer = 0;
            else
                ++_currentPlayer;
        }
        else
            _changePlayer = true;

        ResetStates();

        _markers[_currentPlayer].transform.localPosition = new Vector3(_markers[_currentPlayer].transform.localPosition.x, 460);
        // менять интерфейс (выводить надпись + выдвигать нужную иконку игрока + задвигать других игроков + удалять иконки удалённых игроков)

        if (!_playersQueue[_currentPlayer]._isPlayer) // если ведущий - НЕ игрок
            _isReady = true;
    }

    /*[ContextMenu("Test")]
    void Test()
    {
        _markers[0].transform.localPosition = new Vector3(-210, 240);
        _markers[0].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-210, 200);
    }*/

    public void ResetMarkers()
    {
        _markers[0].SetActive(true);
        //_markers[0].transform.localPosition = new Vector3(-210, 540);
        _markers[0].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-210, 200);

        for (byte i = 1; i < _playersQueue.Count; i++)
        {
            _markers[i].SetActive(true);
            //_markers[i].transform.localPosition = new Vector3(_markers[0].transform.localPosition.x + (i * 140), 540);
            _markers[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(_markers[0].gameObject.GetComponent<RectTransform>().localPosition.x + (i * 140), 200);
        }
    }

    public void SetMassToOther(float mass)
    {
        for (byte i = 0; i < _playersQueue.Count; i++)
            if (i != _currentPlayer)
                _playersQueue[i].SetMass(mass);
    }

    public void CheckConditions() // проверяет выполнились ли условия победы
    {
        if (_playersQueue.Count == 1)
        {
            if (_playersQueue[0]._isPlayer)
                GameManager.Instance.ChangeScreen(GameManager.Instance.WinScreen, true);
            else
                GameManager.Instance.ChangeScreen(GameManager.Instance.LoseScreen, true);
        }
    }



    void StartGame()
    {
        //EmptyTitle();
        ResetMarkers();
        NewTurn();
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
        && _eliminationQueue.Contains(_playersQueue[_currentPlayer]))
        {
            foreach (CheckerController player in _eliminationQueue)
                player.ResetPosition();

            _eliminationQueue.Clear();
        }
        else if (_eliminationQueue.Count > 0)
        {
            if (_eliminationQueue[0]._isPlayer)
            {
                GameManager.Instance.ChangeScreen(GameManager.Instance.LoseScreen, true);
                _changePlayer = true;
            }
            else
                _changePlayer = false;

            RemoveFromPlayersQueue();

            _eliminationQueue.Clear();
            ResetMarkers();
        }
    }

    void RemoveFromPlayersQueue()
    {
        CheckerController current_player = _playersQueue[_currentPlayer];

        if (!_eliminationQueue.Contains(_playersQueue[_currentPlayer]))
        {
            _changePlayer = false;

            if (_currentPlayer <= 0)
                _currentPlayer = (byte)(_playersQueue.Count - 1);
            else
                _currentPlayer--;
        }
        else
            _changePlayer = true;

        // исчезновение из системы ходов
        foreach (CheckerController player in _eliminationQueue)
        {
            //_markers[_playersQueue.IndexOf(player)].SetActive(false);

            byte player_index = (byte)_playersQueue.IndexOf(player);

            _playersQueue.RemoveAt(player_index);
            _markers.RemoveAt(player_index);
        }

        if (!_changePlayer)
            for (byte i = 0; i < _playersQueue.Count; i++)
                if (_playersQueue[i] == current_player)
                    _currentPlayer = i;
    }

    void RecalcQueue()
    {
        //for ()
    }

    void EmptyTitle()
    {
        _textMeshPro.text = "";
    }



    void Awake()
    {
        GameManager.Instance.ChangeScreen(GameManager.Instance.GameScreen);
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
