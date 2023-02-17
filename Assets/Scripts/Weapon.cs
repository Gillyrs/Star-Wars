using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Transform firePosition;
    private IObjectPool projectilePool;
    private GameObject projectilePrefab;
    private Rigidbody2D rb;
    private WeaponDataSctructure weaponData;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        projectilePrefab = projectile.gameObject;
        projectilePool = ObjectPoolSpawner.GetObjectPool(projectilePrefab);
    }

    public void InitStats(WeaponData weaponData)
    {
        this.weaponData = weaponData.GetWeaponData();
    }

    public void Shoot()
    {
        var projectile = projectilePool.Instantiate(firePosition.position, new Quaternion());
        var rigidBody = projectile.GetComponent<Rigidbody2D>();
        rigidBody.rotation = rb.rotation;
        rigidBody.AddForce(transform.right * projectileData.Speed, ForceMode2D.Impulse);
    }

    public void Rotate(Transform target)
    {
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
