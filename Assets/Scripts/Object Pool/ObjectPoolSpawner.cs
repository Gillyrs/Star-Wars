using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolRegister prefabRegister;

    private void Awake()
    {
        InitPools();
    }
    private void InitPools()
    {
        for (int i = 0; i < prefabRegister.Prefabs.Count; i++)
        {
            var prefab = prefabRegister.Prefabs[i];
            var pool = new GameObject(prefab.name + " ObjectPool");
            var component = pool.AddComponent<ObjectPool>();
            component.InitObjects(prefab, prefabRegister.Numbers[i]);
        }
    }
}
