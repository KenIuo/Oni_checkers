using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckerController : MonoBehaviour
{
    public GameObject Marker;
    public UnityEvent<CheckerController> OnCheckerReady;
    public bool _gameStarted = false;

    internal Transform _standardPosition;

    Rigidbody _rigidbody;
    bool _isStopped = true;
    bool _isAlive = true;



    public void SetMass(float mass)
    {
        _rigidbody.mass = mass;
    }

    public void SetAlive(bool state)
    {
        _isAlive = state;
        gameObject.SetActive(state);

        //OnCheckerReady.Invoke(this); // ?
    }

    public void ChooseAttackVector()
    {
        Invoke(nameof(DelaySkip), 0.5f);

        //_checkerState._isStopped = false;
    }



    void DelaySkip()
    {
        TurnSystem.Instance._didMoved = true;
    }

    void ChangeGameStat(bool stat_to)
    {
        _gameStarted = stat_to;
    }

    void SayReady()
    {
        if (_gameStarted)
            if (!gameObject.activeSelf || _rigidbody.velocity.magnitude == 0) // проверка состояния, что шашка не двигается
            {
                _isStopped = true;

                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);

                OnCheckerReady.Invoke(this);
            }
    }



    void Awake()
    {
        if (gameObject == TurnSystem.Instance._playerCheckerObject)
            TurnSystem.Instance.AddToList(this);

        EventSystem.Instance.OnStartGame.AddListener(ChangeGameStat);
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _standardPosition = gameObject.transform;

        gameObject.SetActive(true);
    }

    void Start()
    {
        if (gameObject != TurnSystem.Instance._playerCheckerObject)
            TurnSystem.Instance.AddToList(this);
    }

    void Update()
    {
        if (!_isStopped)
            SayReady();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CheckerController>())
            gameObject.transform.GetChild(1).gameObject.SetActive(false);

        if (collision.gameObject.name != "PlayingField")
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
