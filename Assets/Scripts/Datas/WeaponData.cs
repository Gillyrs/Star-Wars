using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Datas/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float accuracy;
    [SerializeField] private float projectileSpreading;
    [Tooltip("Time between machine queues")]
    [SerializeField] private float reloadTime;
    [Tooltip("Time between shots in machine queue")]
    [SerializeField] private float shootCooldown;
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
    public float reloadTime;
    public float shootCooldown;
    public int machineQueue;

    public WeaponDataSctructure(float accuracy, float projectileSpreading, 
                                float reloadTime, float shootCooldown, int machineQueue)
    {
        this.accuracy = accuracy;
        this.projectileSpreading = projectileSpreading;
        this.reloadTime = reloadTime;
        this.shootCooldown = shootCooldown;
        this.machineQueue = machineQueue;
    }
}
