using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private ProjectileData projectile;
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform target;

    private void Start()
    {
        weapon.InitStats(weaponData);
    }

    private void FixedUpdate()
    {
        weapon.Rotate(target);
    }
}
