using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Datas/Unit Data")]
public class UnitData : ScriptableObject
{
    [SerializeField] private int health;
    [Tooltip("Amount which will be regenerate at 1 tick")]
    [SerializeField] private int regenerationAmount;
    [Tooltip("Tick duration")]
    [SerializeField] private int regenerationSpeed;
    [Tooltip("Amount of time which unit has to stay without any damage for starting regeneration")]
    [SerializeField] private int regenerationCooldown;
    [SerializeField] private int armor;
    [Tooltip("Movement speed")]
    [SerializeField] private float speed;
    [Tooltip("Speed of weapon rotation to target")]
    [SerializeField] private int weaponRotationSpeed;

    public UnitDataStructure GetUnitData()
    {
        return new UnitDataStructure(health, regenerationAmount, regenerationSpeed, 
                                     regenerationCooldown, armor, speed, weaponRotationSpeed);
    }
}

public struct UnitDataStructure
{
    public int Health;
    public int RegenerationAmount;
    public int RegenerationSpeed;
    public int RegenerationCooldown;
    public int Armor;
    public float Speed;
    public int WeaponRotationSpeed;

    public UnitDataStructure(int health, int regenerationAmount, int regenerationSpeed, 
                             int regenerationCooldown, int armor, float speed, int weaponRotationSpeed)
    {
        Health = health;
        RegenerationAmount = regenerationAmount;
        RegenerationSpeed = regenerationSpeed;
        RegenerationCooldown = regenerationCooldown;
        Armor = armor;
        Speed = speed;
        WeaponRotationSpeed = weaponRotationSpeed;
    }
}
