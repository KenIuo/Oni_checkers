using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromCamera : MonoBehaviour
{
    [SerializeField] LayerMask _UIHideableLayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _UIHideableLayer)
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _UIHideableLayer)
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
