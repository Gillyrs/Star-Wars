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
        for (int i = 0; i < weaponData.MachineQueue; i++)
        {
            await UniTask.Delay(weaponData.ShootCooldown);

            var projectile = projectilePool.Instantiate(firePosition.position, new Quaternion());
            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.InitStats(projectileData);
            projectileComponent.OnLifeTimeEnded += (projectile) => projectilePool.Destroy(projectile.gameObject);
            var rigidBody = projectile.GetComponent<Rigidbody2D>();
            rigidBody.rotation = rb.rotation;
            rigidBody.AddForce(transform.right * projectileComponent.ProjectileData.Speed
                + new Vector3(0, Random.Range(-weaponData.ProjectileSpreading, weaponData.ProjectileSpreading)),
                ForceMode2D.Impulse);
            isReloaded = false;
        }
        isShooting = false;
        await UniTask.Delay(weaponData.ReloadTime);
        isReloaded = true;


    }

    public async void Rotate(Transform target, int weaponrotationSpeed)
    {
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        var rt = angle / 100;
        for (int i = 0; i < 100; i++)
        {
            await UniTask.Delay(weaponrotationSpeed);
            Debug.Log("Rotated");
            rb.rotation += rt;
        }
        
    }
}
