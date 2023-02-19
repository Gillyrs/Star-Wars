using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "Datas/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private int lifeTime;
    [SerializeField] private int damage;
    
    public ProjectileDataStructure GetProjectileData()
    {
        return new ProjectileDataStructure(speed, lifeTime, damage);
    }
}

public struct ProjectileDataStructure
{
    public float Speed;
    public int LifeTime;
    public int Damage;
    
    public ProjectileDataStructure(float speed, int lifeTime, int damage)
    {
        Speed = speed;
        LifeTime = lifeTime;
        Damage = damage;
    }
}

