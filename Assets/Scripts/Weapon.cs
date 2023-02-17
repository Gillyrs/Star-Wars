using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
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
    private bool isShooting = false;
    private bool isReloaded = true;

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

    public async void Shoot()
    {
        if (isShooting || isReloaded == false)
            return;
        isShooting = true;
        for (int i = 0; i < weaponData.machineQueue; i++)
        {
            await UniTask.Delay(weaponData.shootCooldown);
            var projectile = projectilePool.Instantiate(firePosition.position, new Quaternion());
            var rigidBody = projectile.GetComponent<Rigidbody2D>();
            rigidBody.rotation = rb.rotation;
            rigidBody.AddForce(transform.right * projectileData.Speed
                + new Vector3(0, Random.Range(-weaponData.projectileSpreading, weaponData.projectileSpreading)),
                ForceMode2D.Impulse);
            isReloaded = false;
        }
        isShooting = false;
        await UniTask.Delay(weaponData.reloadTime);
        isReloaded = true;


    }

    public void Rotate(Transform target)
    {
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
