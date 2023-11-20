using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromCamera : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        other.gameObject.SetActive(true);
    }
}
