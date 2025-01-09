using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEventInt : ScriptableObject
{
    private readonly List<GameEventListenerInt> eventListeners =
        new List<GameEventListenerInt>();

    public void Raise(int a)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(a);
    }

    public void RegisterListener(GameEventListenerInt listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerInt listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
