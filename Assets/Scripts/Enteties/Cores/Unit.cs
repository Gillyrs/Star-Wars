using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Unit : MonoBehaviour
{
    private Weapon weapon;
    private UnitDataStructure unitData;

    public void Init(UnitData unitData, Weapon weapon)
    {
        this.unitData = unitData.GetUnitData();
        this.weapon = weapon;
    }


}
