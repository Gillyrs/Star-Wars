using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Projectile : MonoBehaviour
{
    public ProjectileDataStructure ProjectileData;
    public Action<Projectile> OnLifeTimeEnded;
    [SerializeField] private Rigidbody2D rb;

    public void InitStats(ProjectileData projectileData)
    {
        ProjectileData = projectileData.GetProjectileData();
        Activate();
    }

    public void ClearAllSubsribations()
    {
        OnLifeTimeEnded = null;
    }
    private async void Activate()
    {
        await UniTask.Delay(ProjectileData.LifeTime);
        OnLifeTimeEnded(this);
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }
}
