using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Projectile : MonoBehaviour
{
    public ProjectileDataStructure ProjectileData;
    public Action<Projectile> OnLifeTimeEnded;
    public void InitStats(ProjectileData projectileData)
    {
        ProjectileData = projectileData.GetProjectileData();
        Activate();
    }
    private async void Activate()
    {
        await UniTask.Delay(ProjectileData.LifeTime);
        OnLifeTimeEnded(this);
    }
}
