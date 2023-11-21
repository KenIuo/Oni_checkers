using System;
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
    internal bool _isPlayer { get; private set; } = false;

    GameObject _chargeVFX;
    GameObject _speedVFX;
    Rigidbody _rigidbody;
    Vector3 _directionMove;
    float _currentRadius;



    public float GetCurrentRadius(Vector3 hit_position)
    {
        float current_radius = Vector3.Distance(hit_position, gameObject.transform.position);

        if (current_radius > _max_radius)
        {
            current_radius = _max_radius;
            _chargeVFX.SetActive(true);
        }
        else if (current_radius < _min_radius)
        {
            current_radius = 0;
            _chargeVFX.SetActive(false);
        }
        else
            _chargeVFX.SetActive(false);

        return current_radius;
    }

    float GetCurrentRadius(float current_radius)
    {
        if (current_radius > _max_radius)
        {
            current_radius = _max_radius;
            _chargeVFX.SetActive(true);
        }
        else if (current_radius < _min_radius)
        {
            current_radius = 0;
            _chargeVFX.SetActive(false);
        }
        else
            _chargeVFX.SetActive(false);

        return current_radius;
    }

    public void SetMass(float mass)
    {
        _rigidbody.mass = mass;
    }

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
        byte player_to_attack = (byte)UnityEngine.Random.Range(0, 3);

        while (player_to_attack == TurnSystem.Instance._currentPlayer)
            player_to_attack = (byte)UnityEngine.Random.Range(0, 3);

        Vector3 random_player_position = TurnSystem.Instance._playersQueue[player_to_attack].gameObject.transform.position;

        random_player_position = GetDirectionVector(random_player_position);
        _directionMove = GetPositionToKill(random_player_position);

        Invoke(nameof(DelayLaunch), 2.0f);
        //LaunchChecker(random_radius, random_player_position);
    }

    public void LaunchChecker(float current_radius, Vector3 direction_move)
    {
        if (current_radius == _max_radius)
            TurnSystem.Instance.SetMassToOther(0.1f);

        GameManager.Instance.SoundManager.PlayLaunchSound();
        //_playerCheckerController.SetMovedState();
        SetState(CheckerState.Moving);
        _speedVFX.SetActive(true);

        _rigidbody.velocity = direction_move * (current_radius * _launchStrength);
    }



    Vector3 GetDirectionVector(Vector3 direction_vector)
    {
        direction_vector.y = gameObject.transform.position.y;

        Vector3 direction_move = direction_vector - gameObject.transform.position;
        direction_move.Normalize();

        return direction_move;
    }

    Vector3 GetPositionToKill(Vector3 direction_vector)
    {
        _currentRadius = GetCurrentRadius(UnityEngine.Random.Range(_min_radius, 10.0f));

        gameObject.transform.rotation = Quaternion.LookRotation(direction_vector);
        //initial_position.Normalize();
        return direction_vector;
    }

    void DelayLaunch()
    {
        LaunchChecker(_currentRadius, _directionMove);
        //SetState(CheckerState.Standing);
    }

    void CheckReadyState()
    {
        if (_rigidbody.velocity.magnitude == 0) // проверка состояния, что шашка не двигается
        {
            SetState(CheckerState.Standing);

            _chargeVFX.SetActive(false);
            _speedVFX.SetActive(false);

            onCheckerReady.Invoke();
        }
        else // иначе (шашка не ходит) и она (активна и двигается)
        {
            SetState(CheckerState.Moving);
        }
    }



    void Awake()
    {
        _isPlayer = gameObject.TryGetComponent(out PointerSystemController _);

        if (_isPlayer)
            TurnSystem.Instance.AddToLists(this, marker);

        //EventSystem.Instance.OnStartGame.AddListener(ChangeGameStat);
        _chargeVFX = gameObject.transform.GetChild(1).gameObject;
        _speedVFX = gameObject.transform.GetChild(2).gameObject;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _standardPosition = gameObject.transform;

        gameObject.SetActive(true);
    }

    void Start()
    {
        if (!_isPlayer)
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
            _chargeVFX.SetActive(false);
        }
        else if (collision.gameObject.name != "PlayingField")
        {
            GameManager.Instance.SoundManager.PlayCollideSound();
            _speedVFX.SetActive(false);
        }
    }
}
