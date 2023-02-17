using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Datas/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Tooltip("Percantage that projectile will reach target")]
    [Range(0, 100)]
    [SerializeField] private float accuracy;
    [Range(0, 1)]
    [SerializeField] private float projectileSpreading;
    [Tooltip("Time between machine queues in miliseconds")]
    [SerializeField] private int reloadTime;
    [Tooltip("Time between shots in machine queue in miliseconds")]
    [SerializeField] private int shootCooldown;
    [Tooltip("Amount of shot in 1 machine queue")]
    [SerializeField] private int machineQueue;
    
    public WeaponDataSctructure GetWeaponData()
    {
        return new WeaponDataSctructure(accuracy, projectileSpreading, reloadTime, shootCooldown, machineQueue);
    }
}

public struct WeaponDataSctructure
{
    public float accuracy;
    public float projectileSpreading;
    public int reloadTime;
    public int shootCooldown;
    public int machineQueue;

    public WeaponDataSctructure(float accuracy, float projectileSpreading,
                                int reloadTime, int shootCooldown, int machineQueue)
    {
        this.accuracy = accuracy;
        this.projectileSpreading = projectileSpreading;
        this.reloadTime = reloadTime;
        this.shootCooldown = shootCooldown;
        this.machineQueue = machineQueue;
    }
}
