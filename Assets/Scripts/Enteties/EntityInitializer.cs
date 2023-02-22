using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInitializer : MonoBehaviour
{
    [SerializeField] private UnitData unitData;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private GameObject entityPrefab;
    [SerializeField] private Team currentSpawnTeam;
    [SerializeField] private Transform enemyBase;
    [SerializeField] private Transform spawnPoint;
    private IObjectPool entityPool;

    private void Start()
    {
        entityPool = ObjectPoolSpawner.GetObjectPool(entityPrefab);
    }
    public void CreateEntity()
    {
        var obj = entityPool.Instantiate(spawnPoint.position, Quaternion.identity);
        var entity = obj.GetComponent<Entity>();
        var unit = entity.GetUnit();
        unit.Init(unitData, weaponData, projectileData, currentSpawnTeam, enemyBase);
    }
}
