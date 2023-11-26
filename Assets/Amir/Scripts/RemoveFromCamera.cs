using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromCamera : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
