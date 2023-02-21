using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private AILerp aiLerp;
    
    public void SetDestination(Transform target)
    {
        aiLerp.destination = target.position;
    }
    public void MoveToFightingState()
    {
        aiLerp.isStopped = true;
    }
    public void MoveToRunningState()
    {
        aiLerp.isStopped = false;
    }
}
