using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;


public class Weapon : MonoBehaviour
{
    public bool isStopped;
    [SerializeField] private Projectile projectile;    
    [SerializeField] private Transform firePosition;
    private ProjectileData projectileData;
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

    public void InitStats(WeaponData weaponData, ProjectileData projectileData)
    {
        this.weaponData = weaponData.GetWeaponData();
        this.projectileData = projectileData;
    }

    public async UniTask Shoot()
    {
        if (isShooting || isReloaded == false)
            return;
        isShooting = true;
        for (int i = 0; i < weaponData.MachineQueue; i++)
        {
            if(isStopped)
                return;
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

    public IEnumerator Rotate(Transform target, float weaponrotationSpeed, Action doneAction)
    {
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(rb.rotation - angle) > 0.01f)
        {
            float step = Mathf.MoveTowardsAngle(rb.rotation, angle, weaponrotationSpeed * Time.deltaTime);
            rb.rotation = step;
            yield return null;
        }
        doneAction();
    }
}
