using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour, ICanListenEvent, ICanSendEvent
{
    public ICanInteract curInteractObject;
    public GameObject imageHideButton;
    public Button interactButton;
    EventSubscription _onEnterInteractEvent;
    EventSubscription _onExitInteractEvent;
    EventSubscription _onDeathEvent;

    void OnEnable()
    {
        _onEnterInteractEvent = this.Listen<OnEnterInteractEvent>(OnEnterInteract);
        _onExitInteractEvent = this.Listen<OnExitInteractEvent>(OnExitInteract);
        _onDeathEvent = this.Listen<OnDeathEvent>(OnPlayerDie);
    }

    void OnDisable()
    {
        _onEnterInteractEvent?.Dispose();
        _onExitInteractEvent?.Dispose();
        _onDeathEvent?.Dispose();
    }

    void Start()
    {
        HideButton();
    }

    void HideButton()
    {
        if (imageHideButton != null)
        {
            imageHideButton.SetActive(true);
        }
        if (interactButton != null)
        {
            interactButton.enabled = false;
        }
    }

    void ShowButton()
    {
        if (imageHideButton != null)
        {
            imageHideButton.SetActive(false);
        }
        if (interactButton != null)
        {
            interactButton.enabled = true;
        }
    }

    public void OnEnterInteract(OnEnterInteractEvent evt)
    {
        if (evt.InteractObject != null)
        {
            curInteractObject = evt.InteractObject;
        }
        ShowButton();
    }

    public void OnExitInteract(OnExitInteractEvent evt)
    {
        HideButton();
    }

    public void OnPlayerDie(OnDeathEvent evt)
    {
        HideButton();
    }

    public void ExecuteInteract()
    {
        if (curInteractObject != null)
        {
            curInteractObject.Interact();
            this.Publish(new OnExecuteInteractEvent());
        }
    }
}
