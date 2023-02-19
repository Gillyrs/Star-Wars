using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefab Register", menuName = "Object Pool/Prefab Register")]
public class ObjectPoolRegister : ScriptableObject
{
    [Header("Registred prefabs")]
    public List<PrefabStructure> Prefabs = new();
    
}
