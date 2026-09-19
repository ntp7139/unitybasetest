using UnityEngine;

public class PlatformController : MonoBehaviour, ICanInteract, ICanSendEvent
{
    [SerializeField]
    Animator animator;

    public void Interact()
    {
        if (animator != null)
        {
            animator.Play("Platform_Interact");
        }
    }

    public void OnEndInteract()
    {
        this.Publish(new OnExitInteractEvent());
    }
}
