using UnityEngine;

public class GetCollision : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.SoundManager.PlayDeathSound();
        other.GetComponentInParent<CheckerController>().SetState(CheckerState.Died);
    }
}
