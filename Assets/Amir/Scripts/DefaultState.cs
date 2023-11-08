using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : MonoBehaviour
{
    [NonSerialized] public Transform _standardPosition;
    [NonSerialized] public float _currentSpeed;
    [NonSerialized] public bool _isCheckable;


    
    void Start()
    {
        _standardPosition = gameObject.transform;
    }

    void Update()
    {
       if (_isCheckable)
            if (gameObject.TryGetComponent(out Rigidbody rigidbody))
                //if (rigidbody.velocity.magnitude != 0)
                    print(rigidbody.name + ": " + rigidbody.velocity.magnitude);
    }
}
