using UnityEngine;

public class GetCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SoundManager.PlayDeathSound();
        // воспроизводить анимацию смерти шашки
        other.GetComponentInParent<CheckerController>().SetState(CheckerState.Died);

        //TurnSystem.Instance.CheckEndOfGameConditions(other.gameObject.GetComponent<CheckerController>());
        //TurnSystem.Instance.GetDeathByFall().OnEnter(other.gameObject);
        //FindFirstObjectByType<DeathByFall>().OnEnter(other.gameObject);
    }
}
