using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckerState : MonoBehaviour
{
    [NonSerialized] public Transform _standardPosition;
    [NonSerialized] public float _currentSpeed;
    [NonSerialized] public bool _isStopped = true;


    
    void Start()
    {
        FindFirstObjectByType<TurnSystem>()._playersQueue.Add(gameObject);
        _standardPosition = gameObject.transform;
    }

    void Update()
    {
        if (gameObject.activeSelf && !_isStopped) // && gameObject.name == "Checker 1")
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude == 0) // вызов проверки состояния, что все оставшиеся шашки не двигаются
            {
                _isStopped = true; // print("magnituda: " + rigidbody.velocity.magnitude);
                FindFirstObjectByType<TurnSystem>().CheckConditions();

                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (FindFirstObjectByType<TurnSystem>()._playersQueue.Contains(collision.gameObject))
            gameObject.transform.GetChild(1).gameObject.SetActive(false);

        if (collision.gameObject != GameObject.Find("PlayingField"))
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
