using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        TurnSystem.Instance.CheckEndOfGameConditions(other.gameObject.GetComponent<CheckerController>());

        // �������������� �������� ������ �����

        //TurnSystem.Instance.GetDeathByFall().OnEnter(other.gameObject);
        //FindFirstObjectByType<DeathByFall>().OnEnter(other.gameObject);
    }
}
