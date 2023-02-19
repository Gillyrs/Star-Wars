using System;
using UnityEngine;

/// <summary>
/// Calls events and passes colliders as arguments if some of the messages call
/// </summary>
public class TriggerDetector : MonoBehaviour
{
    public event Action<Collider2D> OnTrigerEnter;
    public event Action<Collider2D> OnTrigerStay;
    public event Action<Collider2D> OnTrigerExit;
    [SerializeField] private Unit myUnit;

    public Unit GetMyUnit()
    {
        return myUnit;
    }

    public void ClearAllSubsribations()
    {
        OnTrigerEnter = null;
        OnTrigerStay = null;
        OnTrigerExit = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigerEnter?.Invoke(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTrigerStay?.Invoke(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTrigerExit?.Invoke(collision);
    }
}

