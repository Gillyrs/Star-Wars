using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Transform firePosition;
    private IObjectPool projectilePool;
    private GameObject projectilePrefab;

    private void Awake()
    {
        projectilePrefab = projectile.gameObject;
        projectilePool = ObjectPoolSpawner.GetObjectPool(projectilePrefab);
    }

    public void Shoot()
    {
        var projectile = projectilePool.Instantiate(firePosition.position, new Quaternion());
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.forward);
    }
}
