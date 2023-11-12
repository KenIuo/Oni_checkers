using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollision : MonoBehaviour
{
    void Awake()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

        // �������������� �������� ������ �����

        TurnSystem.Instance.GetDeathByFall().OnEnter(other.gameObject);
        //FindFirstObjectByType<DeathByFall>().OnEnter(other.gameObject);
    }
}
