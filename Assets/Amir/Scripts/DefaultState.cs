using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : MonoBehaviour
{
    [NonSerialized] public Transform _standardPosition;


    
    void Start()
    {
        _standardPosition = gameObject.transform;
    }
}
