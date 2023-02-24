using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.RVO;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private RVOController controller;
    private bool locked;
    private void Update()
    {
        if (!locked)
        {
            var delta = controller.CalculateMovementDelta(transform.position, Time.deltaTime);
            transform.position = transform.position + delta;
        }

    }
    public void SetDestination(Transform target, float speed)
    {
        controller.SetTarget(target.position, speed, speed);
        
    }
    public void MoveToFightingState()
    {
        locked = true;
        return;
    }
    public void MoveToRunningState()
    {
        locked = false;
        return;
    }
}
