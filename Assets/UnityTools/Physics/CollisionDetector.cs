using System;
using UnityEngine;

/// <summary>
/// Calls events and passes collisions as arguments if some of the messages call
/// </summary>
public class CollisionDetector : Detector
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        InvokeOnCollisionEnter(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        InvokeOnCollisionExit(collision);
    }
}
