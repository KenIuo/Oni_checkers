using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
            transform.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Mouse X") * 2);
    }
}
