using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public bool isStopped;
    public event Action OnCanShoot;
    [SerializeField] private Projectile projectile;    
    [SerializeField] private Transform firePosition;
    [SerializeField] private Team team;
    [SerializeField] private List<Detector> myEntityDetectors;
    [SerializeField] public bool isShooting = false;
    [SerializeField] public bool isReloaded = true;
    [SerializeField] private Member currentTarget;

    private ProjectileData projectileData;
    private IObjectPool projectilePool;
    private GameObject projectilePrefab;

    private Rigidbody2D rb;
    private WeaponDataSctructure weaponData;
    
    private Chance chance;   
    private void Awake()
    {       
        rb = GetComponent<Rigidbody2D>();
        projectilePrefab = projectile.gameObject;
        projectilePool = ObjectPoolSpawner.GetObjectPool(projectilePrefab);
    }
    public void Init(WeaponData weaponData, ProjectileData projectileData, Team team, List<Detector> detectors)
    {
        this.weaponData = weaponData.GetWeaponData();
        this.projectileData = projectileData;

        int[] chances = new int[] { this.weaponData.Accuracy, 100 - this.weaponData.Accuracy };
        bool[] bools = new bool[] { true, false };
        chance = new Chance(new ChanceStructure(chances, bools));

        myEntityDetectors = detectors;

        this.team = team;
        if (team == Team.SecondTeam)
        {
            transform.localPosition = -new Vector3(transform.localPosition.x, 0);
            rb.rotation = 180;
        }
    }    
    public async void Shoot(Member member)
    {
        if (isShooting || !isReloaded)
            return;

        Debug.Log("SHOOOOOOOOOOTING");
        isShooting = true;
        currentTarget = member;
        for (int i = 0; i < weaponData.MachineQueue; i++)
        {
            
            if (isStopped)
            {
                isShooting = false;
                return;
            }

            await UniTask.Delay(weaponData.ShootCooldown);
             
            var projectile = projectilePool.Instantiate(firePosition.position, new Quaternion());

            var projectileComponent = projectile.GetComponent<Projectile>();

            var data = projectileData.GetProjectileData();
            float speed = data.Speed;
            float distance = Vector3.Distance(firePosition.position, member.transform.position);
            float time = distance / data.Speed;

            projectileComponent.InitStats(data, member, chance.Spin(), Mathf.RoundToInt(time * 1000));
            projectileComponent.OnLifeTimeEnded += (projectile) => 
            {
                projectile.ClearAllSubsribations();
                projectilePool.Destroy(projectile.gameObject);
            };
            projectileComponent.Rb.AddForce(GetProjectileForce(projectileComponent), ForceMode2D.Impulse);
            isReloaded = false;
        }
        isShooting = false;

        await UniTask.Delay(weaponData.ReloadTime);

        isReloaded = true;
        OnCanShoot?.Invoke();

        Vector3 GetProjectileForce(Projectile projectile)
        {
            var forceDirection = transform.right;
            var spreadingOffset = Random.Range(-weaponData.ProjectileSpreading, weaponData.ProjectileSpreading);
            var ySpreading = new Vector3(0, spreadingOffset);
            return forceDirection * projectile.ProjectileData.Speed + ySpreading;
        }
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
