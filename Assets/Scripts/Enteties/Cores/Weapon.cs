using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;
using System.Threading;
using UnityTools;

public class Weapon : MonoBehaviour
{
    public bool isStopped;
    public event Action OnCanShoot;
    [SerializeField] private Projectile projectile;    
    [SerializeField] private Transform firePosition;
    private ProjectileData projectileData;
    private IObjectPool projectilePool;
    private GameObject projectilePrefab;
    private Rigidbody2D rb;
    private WeaponDataSctructure weaponData;
    private bool isShooting = false;
    private bool isReloaded = true;
    private Chance chance;
    [SerializeField]  private Team myTeam;
    [SerializeField] private List<Detector> myEntityDetectors;

    private void Awake()
    {       
        rb = GetComponent<Rigidbody2D>();
        projectilePrefab = projectile.gameObject;
        projectilePool = ObjectPoolSpawner.GetObjectPool(projectilePrefab);
    }

    public void InitStats(WeaponData weaponData, ProjectileData projectileData, List<Detector> detectors, Team team)
    {
        this.weaponData = weaponData.GetWeaponData();
        this.projectileData = projectileData;

        chance = new Chance(new ChanceStructure(new int[] 
        { 
        this.weaponData.Accuracy,
        100 - this.weaponData.Accuracy 
        }, 
        new bool[] { true, false }));

        myEntityDetectors = detectors;
        myTeam = team;
        if (myTeam == Team.SecondTeam)
        {
            transform.localPosition += new Vector3(transform.localPosition.x, 0);
            rb.rotation = 180;
        }
    }

    public void ResetStates()
    {
        isShooting = false;
        isStopped = false;
        isReloaded = true;
    }

    public async void Shoot()
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
            projectileComponent.InitStats(projectileData, chance.Spin(), myEntityDetectors, myTeam);
            projectileComponent.OnLifeTimeEnded += (projectile) => 
            {
                projectile.ClearAllSubsribations();
                projectilePool.Destroy(projectile.gameObject);
            };
            projectileComponent.Rb.AddForce(transform.right * projectileComponent.ProjectileData.Speed
                + new Vector3(0, Random.Range(-weaponData.ProjectileSpreading, weaponData.ProjectileSpreading)),
                ForceMode2D.Impulse);
            isReloaded = false;
        }
        isShooting = false;
        await UniTask.Delay(weaponData.ReloadTime);
        isReloaded = true;
        OnCanShoot?.Invoke();

    }

    public IEnumerator Rotate(Transform target, float weaponrotationSpeed)
    {
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(rb.rotation - angle) > 0.01f)
          {
            float step = Mathf.MoveTowardsAngle(rb.rotation, angle, weaponrotationSpeed * Time.deltaTime);
            rb.rotation = step;
            yield return null;
        }
        OnCanShoot?.Invoke();
    }
}
