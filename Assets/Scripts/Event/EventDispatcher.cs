using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class EventDispatcher : MonoBehaviour
{
    public static EventDispatcher Instance;
    Dictionary<Type, List<Delegate>> _handlers = new(64);
    Stack<List<Delegate>> _bufferPools = new(32);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }

    public void Publish<TEvent>(TEvent evt)
        where TEvent : struct, IEvent
    {
        var type = typeof(TEvent);
        if (!_handlers.TryGetValue(type, out var actions))
        {
            return;
        }

        List<Delegate> buffer =
            _bufferPools.Count > 0 ? _bufferPools.Pop() : new List<Delegate>(32);
        try
        {
            for (int i = 0; i < actions.Count; i++)
            {
                buffer.Add(actions[i]);
            }
            for (int i = 0; i < buffer.Count; i++)
            {
                ((Action<TEvent>)(buffer[i])).Invoke(evt);
            }
        }
        finally
        {
            buffer.Clear();
            _bufferPools.Push(buffer);
        }
    }

    public EventSubscription Subscribe<TEvent>(Action<TEvent> handler)
        where TEvent : struct, IEvent
    {
        var type = typeof(TEvent);

        if (!_handlers.TryGetValue(type, out var actions))
        {
            _handlers[type] = actions = new List<Delegate>(32);
        }
        _handlers[type].Add(handler);

        return new EventSubscription(() =>
        {
            UnSubscribe(handler);
        });
    }

    public void UnSubscribe<TEvent>(Action<TEvent> handler)
        where TEvent : struct, IEvent
    {
        var type = typeof(TEvent);
        if (!_handlers.TryGetValue(type, out var actions))
        {
            return;
        }
        actions.Remove(handler);
    }
}

public static class EventDispatcherExtensions
{
    public static void Publish<TEvent>(this ICanSendEvent _, TEvent evt)
        where TEvent : struct, IEvent
    {
        if (EventDispatcher.Instance != null)
        {
            EventDispatcher.Instance.Publish(evt);
        }
    }

    public static EventSubscription Listen<TEvent>(this ICanListenEvent _, Action<TEvent> handler)
        where TEvent : struct, IEvent
    {
        if (EventDispatcher.Instance != null)
        {
            return EventDispatcher.Instance.Subscribe(handler);
        }
        return EventSubscription.Empty();
    }
}
