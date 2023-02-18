using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Weapon weapon;

    public Unit GetUnit()
    {
        return unit;
    }
    public Weapon GetWeapon()
    {
        return weapon;
    }
}
