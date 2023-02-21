using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Datas/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Tooltip("Percantage that projectile will reach target")]
    [Range(0, 100)]
    [SerializeField] private int accuracy;
    [Range(0, 3)]
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
    public int Accuracy;
    public float ProjectileSpreading;
    public int ReloadTime;
    public int ShootCooldown;
    public int MachineQueue;

    public WeaponDataSctructure(int accuracy, float projectileSpreading,
                                int reloadTime, int shootCooldown, int machineQueue)
    {
        Accuracy = accuracy;
        ProjectileSpreading = projectileSpreading;
        ReloadTime = reloadTime;
        ShootCooldown = shootCooldown;
        MachineQueue = machineQueue;
    }
}
