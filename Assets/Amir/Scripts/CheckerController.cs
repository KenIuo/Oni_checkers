using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CheckerState { Died, Standing, Moving, Turning }

public class CheckerController : MonoBehaviour
{
    public UnityEvent onCheckerReady;
    public GameObject marker;

    /// <summary>
    /// соотношение 5 к 1, если расстояние больше заданного, то оно равно ему, если меньше, то 0
    /// </summary>
    public readonly float _max_radius = 5.00f; // max_size = 1.00f
    public readonly float _min_radius = 1.25f; // min_size = 0.25f
    public readonly float _launchStrength = 8;
    
    internal Transform _standardPosition;
    internal CheckerState _state { get; private set; } = CheckerState.Died;
    internal bool _gameStarted { get; private set; } = false;
    internal bool _isPlayer { get; private set; } = false;

    [SerializeField] bool IsPlayer;

    Rigidbody _rigidbody;
    float _current_radius;
    //bool _isStopped = false;
    //bool _isAlive = true;



    public void ChangeGameStat(bool stat_to)
    {
        _gameStarted = stat_to;
    }

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

        
    }

    //public void SetMovedState()
    //{
    //Invoke(nameof(DelaySkip), 0.1f);
    //}



    public float GetCurrentRadius(Vector3 hit_position)
    {
        _current_radius = Vector3.Distance(hit_position, gameObject.transform.position);

        if (_current_radius > _max_radius)
        {
            _current_radius = _max_radius;
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (_current_radius < _min_radius)
        {
            _current_radius = 0;
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
            gameObject.transform.GetChild(1).gameObject.SetActive(false);

        return _current_radius;
    }



    void DelaySkip()
    {
        TurnSystem.Instance._didMoved = true;
        SetState(CheckerState.Standing);
    }

    void CheckReadyState()
    {
        if (_rigidbody.velocity.magnitude == 0) // проверка состояния, что шашка не двигается
        {
            //_isStopped = true;
            _gameStarted = false;
            SetState(CheckerState.Standing);

            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);

            onCheckerReady.Invoke();
        }
        else // иначе (шашка не ходит) и она (активна и двигается)
        {
            SetState(CheckerState.Moving);
        }
    }



    void Awake()
    {
        _isPlayer = IsPlayer;

        if (IsPlayer)
            TurnSystem.Instance.AddToLists(this, marker);

        EventSystem.Instance.OnStartGame.AddListener(ChangeGameStat);
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _standardPosition = gameObject.transform;

        gameObject.SetActive(true);
    }

    void Start()
    {
        if (!IsPlayer)
            TurnSystem.Instance.AddToLists(this, marker);
    }

    void Update()
    {
        if (!_state.Equals(CheckerState.Died) && !_state.Equals(CheckerState.Turning))
            CheckReadyState();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CheckerController>())
        {
            GameManager.Instance.SoundManager.PlayHitSound();
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (collision.gameObject.name != "PlayingField")
        {
            GameManager.Instance.SoundManager.PlayCollideSound();
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
