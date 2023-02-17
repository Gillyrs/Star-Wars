using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private ProjectileData projectile;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform target;
    private void FixedUpdate()
    {
        weapon.Rotate(target);
    }
}
