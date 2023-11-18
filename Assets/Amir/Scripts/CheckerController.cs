using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CheckerState { Died, Standing, Moving, Turning }

public class CheckerController : MonoBehaviour
{
    public UnityEvent OnCheckerReady;
    public GameObject _marker;

    internal Transform _standardPosition;
    internal CheckerState _state { get; private set; } = CheckerState.Died;
    internal bool _gameStarted { get; private set; } = false;

    Rigidbody _rigidbody;
    bool _isStopped = false;
    bool _isAlive = true;



    public void SetMass(float mass)
    {
        _rigidbody.mass = mass;
    }

    //public void SetAlive(bool state)
    //{
        //_isAlive = state;
        //SetState(CheckerState.Died);
        //gameObject.SetActive(state);

        //OnCheckerReady.Invoke(this); // ?
    //}

    public void SetState(CheckerState state)
    {
        _state = state;

        switch (_state)
        {
            case CheckerState.Died:
                gameObject.SetActive(false);
                TurnSystem.Instance.SetCheckerReady(this, true);
                break;
            case CheckerState.Standing:
                TurnSystem.Instance.SetCheckerReady(this, true);
                break;
            case CheckerState.Moving:
                TurnSystem.Instance.SetCheckerReady(this, false);
                break;
            case CheckerState.Turning:
                TurnSystem.Instance.SetCheckerReady(this, false);
                break;
        }
    }

    public void ChooseAttackVector()
    {
        Invoke(nameof(DelaySkip), 0.5f);

        //_checkerState._isStopped = false;
    }

    //public void SetMovedState()
    //{
        //Invoke(nameof(DelaySkip), 0.1f);
    //}



    void DelaySkip()
    {
        TurnSystem.Instance._didMoved = true;
        SetState(CheckerState.Standing);
    }

    public void ChangeGameStat(bool stat_to)
    {
        _gameStarted = stat_to;
    }

    void CheckReadyState()
    {
        if (_rigidbody.velocity.magnitude == 0) // проверка состояния, что шашка не двигается
        {
            _isStopped = true;
            _gameStarted = false;
            SetState(CheckerState.Standing);

            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);

            OnCheckerReady.Invoke();
        }
        else // иначе (шашка не ходит) и она (активна и двигается)
        {
            SetState(CheckerState.Moving);
        }
    }



    void Awake()
    {
        if (gameObject == TurnSystem.Instance._playerCheckerObject)
            TurnSystem.Instance.AddToLists(this, _marker.transform);

        EventSystem.Instance.OnStartGame.AddListener(ChangeGameStat);
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _standardPosition = gameObject.transform;

        gameObject.SetActive(true);
    }

    void Start()
    {
        if (gameObject != TurnSystem.Instance._playerCheckerObject)
            TurnSystem.Instance.AddToLists(this, _marker.transform);
    }

    void Update()
    {
        //if (!_isStopped)
            //CheckReadyState();

        if (!_state.Equals(CheckerState.Died) || !_state.Equals(CheckerState.Turning))
            CheckReadyState();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CheckerController>())
            gameObject.transform.GetChild(1).gameObject.SetActive(false);

        if (collision.gameObject.name != "PlayingField")
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
