using UnityEngine;

public class VentilationController : MonoBehaviour, ICanInteract, ICanSendEvent
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    GameObject originVentilation;

    public void Interact()
    {
        if (originVentilation != null)
        {
            originVentilation.SetActive(false);
        }
        if (animator != null)
        {
            animator.Play("Ventilation_Interact");
        }
    }

    public void OnEndInteract()
    {
        this.Publish(new OnExitInteractEvent());
    }
}
