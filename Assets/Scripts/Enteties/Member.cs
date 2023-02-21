using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Member : MonoBehaviour
{
    public abstract event Action<Member> OnMemberDestroyed;
    public abstract void TakeDamage(int damage);
    public abstract bool TeamEquals(Team team);
    
}
