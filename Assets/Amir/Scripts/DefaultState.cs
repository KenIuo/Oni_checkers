using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : MonoBehaviour
{
    [NonSerialized] public Transform _standardPosition;
    [NonSerialized] public float _currentSpeed;


    
    void Start()
    {
        _standardPosition = gameObject.transform;
    }

    void Update()
    {
        if (gameObject.activeSelf && gameObject.name == "Checker 1") 
            if (gameObject.TryGetComponent(out Rigidbody rigidbody))
                if (rigidbody.velocity.magnitude == 0) // вызов проверки состояния, что все оставшиеся шашки не двигаются
                    print("magnituda: " + rigidbody.velocity.magnitude);
    }
}
