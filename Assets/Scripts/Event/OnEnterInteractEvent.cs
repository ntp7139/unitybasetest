public struct OnEnterInteractEvent : IEvent
{
    public readonly ICanInteract InteractObject;

    public OnEnterInteractEvent(ICanInteract interactObject)
    {
        InteractObject = interactObject;
    }
}
