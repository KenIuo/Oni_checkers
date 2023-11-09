using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

        gameObject.GetComponentInParent<DeathByFall>().OnEnter(other.gameObject);
    }
}
