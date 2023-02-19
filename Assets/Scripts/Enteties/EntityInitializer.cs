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
    public void CreateEntity()
    {
        var obj = Instantiate(entityPrefab, Vector3.zero, Quaternion.identity);
        var entity = obj.GetComponent<Entity>();
        var unit = entity.GetUnit();
        var weapon = entity.GetWeapon();
        unit.Init(unitData, weapon, currentSpawnTeam);
        weapon.InitStats(weaponData, projectileData);
    }
}
