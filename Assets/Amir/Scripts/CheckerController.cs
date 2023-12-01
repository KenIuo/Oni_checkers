using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CheckerState { Died, Standing, Moving, Turning }

public class CheckerController : MonoBehaviour
{
    public GameObject marker;

    /// <summary>
    /// соотношение 5 к 1, если расстояние больше заданного, то оно равно ему, если меньше, то 0
    /// </summary>
    public readonly float _maxRadius = 5.00f; // max_size = 1.00f
    public readonly float _minRadius = 1.25f; // min_size = 0.25f
    public readonly float _launchStrength = 8;
    
    internal CheckerState _state { get; private set; } = CheckerState.Died;
    internal bool _isPlayer { get; private set; } = false;

    [SerializeField] GameObject _chargeVFX;
    [SerializeField] GameObject _speedVFX;
    [SerializeField] MeshRenderer _bodyMaterial;
    //[SerializeField] private LayerMask _layer;

    Rigidbody _rigidbody;
    DissolveControl _dissolveControl;
    Vector3 _standartPosition;
    Quaternion _standartRotation;
    Vector3 _directionMove;
    float _currentRadius;
    
    Ray ray;



    public float GetCurrentRadius(Vector3 hit_position)
    {
        float current_radius = Vector3.Distance(hit_position, gameObject.transform.position);

        if (current_radius > _maxRadius)
        {
            current_radius = _maxRadius;
            _chargeVFX.SetActive(true);
        }
        else if (current_radius < _minRadius)
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
        if (current_radius > _maxRadius)
        {
            current_radius = _maxRadius;
            _chargeVFX.SetActive(true);
        }
        else if (current_radius < _minRadius)
        {
            current_radius = 0;
            _chargeVFX.SetActive(false);
        }
        else
            _chargeVFX.SetActive(false);

        return current_radius;
    }



    public void LaunchChecker(float current_radius, Vector3 direction_move)
    {
        if (current_radius == _maxRadius)
            TurnSystem.Instance.SetMassToOther(0.1f);

        GameManager.Instance.SoundManager.PlayLaunchSound();
        //_playerCheckerController.SetMovedState();
        SetState(CheckerState.Moving);
        _speedVFX.SetActive(true);

        _rigidbody.velocity = direction_move * (current_radius * _launchStrength);
    }

    public void SetMass(float mass)
    {
        _rigidbody.mass = mass;
    }

    public void ResetPosition()
    {
        gameObject.SetActive(true);
        
        gameObject.transform.position = _standartPosition;
        gameObject.transform.rotation = _standartRotation;

        SetState(CheckerState.Moving);
        StartCoroutine(_dissolveControl.SpawnCoroutine());
    }

    public void SetState(CheckerState state)
    {
        _state = state;

        switch (_state)
        {
            case CheckerState.Died:
                _rigidbody.angularVelocity -= _rigidbody.angularVelocity;
                _rigidbody.velocity -= _rigidbody.velocity;

                _dissolveControl.SetDissolve(1);
                gameObject.SetActive(false);

                TurnSystem.Instance.SetCheckerReady(this, true);
                break;
            case CheckerState.Standing:
                if(!CheckFloor())
                {
                    _rigidbody.freezeRotation = false;
                    SetState(CheckerState.Moving);
                }
                else
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



    void CheckReadyState()
    {
        if (_rigidbody.velocity.magnitude == 0) // проверка состояния, что шашка не двигается
        {
            _speedVFX.SetActive(false);
            _chargeVFX.SetActive(false);

            SetState(CheckerState.Standing);
        }
        else // иначе (шашка не ходит) и она (активна и двигается)
            SetState(CheckerState.Moving);
    }

    bool CheckFloor()
    {
        Vector3 point_to = gameObject.transform.position;
        point_to.y = 1;
        //point_to.Normalize();

        ray = new(gameObject.transform.position, Vector3.down);

        //серж начал тут говнокодить

        //RaycastHit hit;
        //Ray ray1 = new Ray(transform.position, Vector3.down);

        //if (Physics.Raycast(ray1, out hit, 2, _layer))
        //{
        //    Debug.Log(hit.transform.name);
        //    if (hit.transform.name == "CubeBottom")
        //    {
        //        return true;
        //    }
        //}

        //return false;

        //тут закончил говнокодить

        if (Physics.Raycast(ray, out _, float.PositiveInfinity, LayerMask.GetMask("Playing_Field")))
            return true;
        else
            return false;
    }



    #region AI
    public void ChooseAttackVector()
    {
        byte player_to_attack = (byte)UnityEngine.Random.Range(0, TurnSystem.Instance._playersQueue.Count);

        while (player_to_attack == TurnSystem.Instance._currentPlayer)
            player_to_attack = (byte)UnityEngine.Random.Range(0, TurnSystem.Instance._playersQueue.Count);

        Vector3 random_player_position = TurnSystem.Instance._playersQueue[player_to_attack].gameObject.transform.position;

        random_player_position = GetDirectionVector(random_player_position);
        _directionMove = GetPositionToKill(random_player_position);

        Invoke(nameof(DelayLaunch), 2.0f);
        //LaunchChecker(random_radius, random_player_position);
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
        _currentRadius = GetCurrentRadius(UnityEngine.Random.Range(_minRadius, 10.0f));

        gameObject.transform.rotation = Quaternion.LookRotation(direction_vector);
        //initial_position.Normalize();
        return direction_vector;
    }

    void DelayLaunch()
    {
        LaunchChecker(_currentRadius, _directionMove);
        //SetState(CheckerState.Standing);
    }
    #endregion



    void Awake()
    {
        _isPlayer = gameObject.TryGetComponent(out PointerSystemController _);

        //_chargeVFX = gameObject.transform.GetChild(1).gameObject;
        //_speedVFX = gameObject.transform.GetChild(2).gameObject;

        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _dissolveControl = gameObject.GetComponent<DissolveControl>();

        _standartPosition = gameObject.transform.position;
        _standartRotation = gameObject.transform.rotation;

        //_bodyMaterial.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        if (_isPlayer)
            TurnSystem.Instance.AddToLists(this, marker);
    }

    void Start()
    {
        if (!_isPlayer)
            TurnSystem.Instance.AddToLists(this, marker);
    }

    void Update()
    {
        //if (CheckFloor())
        //{
        //    _rigidbody.constraints = RigidbodyConstraints.None;
        //}
        //else
        //{
        //    _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        //    _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
        //}

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

            //collision.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.name == "PlayingField")
        {
            _rigidbody.freezeRotation = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(ray.origin, 0.1f); // Vector3.down);
    }
}
