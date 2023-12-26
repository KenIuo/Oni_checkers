using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public enum CheckerState { Died, Standing, Moving, Turning }

public class CheckerController : MonoBehaviour
{
    public GameObject _marker;
    public VisualEffect _chargeVFX;

    /// <summary>
    /// соотношение 5 к 1, если расстояние больше заданного, то оно равно ему, если меньше, то 0
    /// </summary>
    internal const float _maxRadius = 5.00f; // max_size = 1.00f
    internal const float _minRadius = 1.25f; // min_size = 0.25f
    internal const float _launchStrength = 8;

    internal CheckerState _state { get; private set; } = CheckerState.Died;
    internal bool _isPlayer { get; private set; } = false;

    [SerializeField] VisualEffect _speedVFX;
    [SerializeField] LayerMask _layer;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] DissolveControl _dissolveControl;
    [SerializeField] CheckerAppearance _checkerAppearance;
    
    MarkAnimationController _markAnimationController;
    Quaternion _standartRotation;
    Vector3 _standartPosition;
    Vector3 _directionMove;
    RigidbodyConstraints _standartConstraints;
    float _currentRadius;
    
    Ray ray;



    void Awake()
    {
        _isPlayer = gameObject.TryGetComponent(out PointerSystemController _);

        _markAnimationController = _marker.GetComponent<MarkAnimationController>();

        _checkerAppearance.SetChargeVFXColor(_chargeVFX);
        _checkerAppearance.SetMarkerImageColor(_marker.GetComponentInChildren<UnityEngine.UI.Image>());

        //_chargeVFX = gameObject.transform.GetChild(1).gameObject;
        //_speedVFX = gameObject.transform.GetChild(2).gameObject;

        _standartPosition = transform.position;
        _standartRotation = transform.rotation;

        _standartConstraints = _rigidbody.constraints;

        if (_isPlayer)
            TurnSystem.Instance.AddToLists(this, _marker);
    }

    void Start()
    {
        if (!_isPlayer)
            TurnSystem.Instance.AddToLists(this, _marker);
    }

    void Update()
    {
        if (!_state.Equals(CheckerState.Died) && !_state.Equals(CheckerState.Turning))
        {
            if (!CheckFloor())
                _rigidbody.constraints = RigidbodyConstraints.None;
            else if (_state.Equals(CheckerState.Standing))
                _rigidbody.constraints = _standartConstraints;

            CheckReadyState();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CheckerController>())
        {
            GameManager.Instance.SoundManager.PlayHitSound();
            _chargeVFX.enabled = false;
            _speedVFX.enabled = false;
        }
        else if (collision.gameObject.name != NamesTags.PLAYING_FIELD)
        {
            GameManager.Instance.SoundManager.PlayCollideSound();
            _speedVFX.enabled = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == NamesTags.PLAYING_FIELD)
        {
            _rigidbody.freezeRotation = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + transform.up, transform.position - transform.up);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + transform.up, 0.1f);
    }



    public float GetCurrentRadius(Vector3 hit_position)
    {
        float current_radius = Vector3.Distance(hit_position, transform.position);

        return GetCurrentRadius(current_radius);
    }

    float GetCurrentRadius(float current_radius)
    {
        if (current_radius > _maxRadius)
            current_radius = _maxRadius;
        else if (current_radius < _minRadius)
            current_radius = 0;

        _chargeVFX.enabled = current_radius >= _maxRadius;

        return current_radius;
    }



    public void LaunchChecker(float current_radius, Vector3 direction_move)
    {
        if (current_radius == _maxRadius)
            TurnSystem.Instance.SetMassToOther(0.1f);

        GameManager.Instance.SoundManager.PlayLaunchSound();
        
        SetState(CheckerState.Moving);
        _speedVFX.enabled = true;

        _rigidbody.velocity = direction_move * (current_radius * _launchStrength);
    }

    public void SetMass(float mass)
    {
        _rigidbody.mass = mass;
    }

    public void ResetPosition()
    {
        gameObject.SetActive(true);
        
        transform.position = _standartPosition;
        transform.rotation = _standartRotation;
        _rigidbody.constraints = _standartConstraints;

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

                // воспроизводить анимацию смерти шашки

                _dissolveControl.SetDissolve(1);
                _markAnimationController.Kill();
                gameObject.SetActive(false);

                TurnSystem.Instance.SetCheckerReady(this, true);
                break;
            case CheckerState.Standing:
                _markAnimationController.Pull(false);
                TurnSystem.Instance.SetCheckerReady(this, true);
                break;
            case CheckerState.Moving:
                _markAnimationController.Pull(false);
                TurnSystem.Instance.SetCheckerReady(this, false);
                break;
            case CheckerState.Turning:
                _markAnimationController.Pull(true);
                TurnSystem.Instance.SetCheckerReady(this, false);
                break;
        }
    }



    void CheckReadyState()
    {
        if (_rigidbody.velocity.magnitude == 0) // проверка состояния, что шашка не двигается
        {
            _speedVFX.enabled = false;
            _chargeVFX.enabled = false;

            SetState(CheckerState.Standing);
        }
        else // иначе (шашка не ходит) и она (активна и двигается)
            SetState(CheckerState.Moving);
    }

    bool CheckFloor()
    {
        ray = new(transform.position + transform.up, -transform.up);

        return Physics.Raycast(ray, out _, float.PositiveInfinity, LayersTags.PF_LAYER);
    }



    #region AI
    public void ChooseAttackVector()
    {
        byte player_to_attack = (byte)Random.Range(0, TurnSystem.Instance._playersQueue.Count);

        while (player_to_attack == TurnSystem.Instance._currentPlayer)
            player_to_attack = (byte)Random.Range(0, TurnSystem.Instance._playersQueue.Count);

        Vector3 random_player_position = TurnSystem.Instance._playersQueue[player_to_attack].transform.position;

        random_player_position = GetDirectionVector(random_player_position);
        _directionMove = GetPositionToKill(random_player_position);

        Invoke(nameof(DelayLaunch), 2.0f);
        //LaunchChecker(random_radius, random_player_position);
    }

    Vector3 GetDirectionVector(Vector3 direction_vector)
    {
        direction_vector.y = transform.position.y;

        Vector3 direction_move = direction_vector - transform.position;
        direction_move.Normalize();

        return direction_move;
    }

    Vector3 GetPositionToKill(Vector3 direction_vector)
    {
        _currentRadius = GetCurrentRadius(Random.Range(_minRadius, 10.0f));

        transform.rotation = Quaternion.LookRotation(direction_vector);
        //initial_position.Normalize();
        return direction_vector;
    }

    void DelayLaunch()
    {
        LaunchChecker(_currentRadius, _directionMove);
        //SetState(CheckerState.Standing);
    }
    #endregion
}
