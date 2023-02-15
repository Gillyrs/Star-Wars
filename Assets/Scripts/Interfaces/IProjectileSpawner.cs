using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileSpawner
{
    void Spawn(IWeapon weapon);
}
