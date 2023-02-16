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
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        weapon.GetComponent<Rigidbody2D>().rotation = angle;
    }
}
