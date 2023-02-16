using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileData projectileData;

    public void ConfigureProjectile(ProjectileData projectileData)
    {
        this.projectileData = projectileData;
    }
}
