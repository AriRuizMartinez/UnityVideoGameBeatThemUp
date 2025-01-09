using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerInt : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEventInt Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<int> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int a)
    {

        Response.Invoke(a);
    }
}
