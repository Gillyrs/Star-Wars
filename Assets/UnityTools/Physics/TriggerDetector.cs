using System;
using UnityEngine;
using NaughtyAttributes;
/// <summary>
/// Calls events and passes colliders as arguments if some of the messages call
/// </summary>
public class TriggerDetector : Detector
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        InvokeOnColliderEnter(collider);
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        InvokeOnColliderExit(collider);
    }
}

