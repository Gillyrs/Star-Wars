using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public event Action<Collider2D> OnColliderEnter;
    public event Action<Collider2D> OnColliderExit;
    public event Action<Collision2D> OnCollisionEnter;
    public event Action<Collision2D> OnCollisionExit;
    public bool HasMember;
    [ShowIf("HasMember")]
    [SerializeField] private Member member;
    
    public void ClearAllSubsribations()
    {
        OnColliderEnter = null;
        OnColliderExit = null;
    }
    public bool TryGetMember(out Member member)
    {
        if (HasMember) member = this.member;
        else member = null;
        return HasMember;
    }
    

    protected void InvokeOnColliderEnter(Collider2D collider) => OnColliderEnter?.Invoke(collider);
    protected void InvokeOnColliderExit(Collider2D collider) => OnColliderExit?.Invoke(collider);
    protected void InvokeOnCollisionEnter(Collision2D collision) => OnCollisionEnter?.Invoke(collision);
    protected void InvokeOnCollisionExit(Collision2D collision) => OnCollisionExit?.Invoke(collision);
}
