using UnityEngine;

public class MarkAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Pull(bool state)
    {
        _animator.SetBool(AnimationTags.PULL, state);
    }

    public void Kill()
    {
        _animator.SetTrigger(AnimationTags.KILL);
    }
}
