using System;

public class EventSubscription : IDisposable
{
    Action _action;

    public EventSubscription(Action action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void Dispose()
    {
        _action?.Invoke();
        _action = null;
    }

    public static EventSubscription Empty()
    {
        return new EventSubscription(() => { });
    }
}
