using UnityEngine;

public class GetCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CheckerController>().SetState(CheckerState.Died);
        //TurnSystem.Instance.CheckEndOfGameConditions(other.gameObject.GetComponent<CheckerController>());

        // �������������� �������� ������ �����

        //TurnSystem.Instance.GetDeathByFall().OnEnter(other.gameObject);
        //FindFirstObjectByType<DeathByFall>().OnEnter(other.gameObject);
    }
}
