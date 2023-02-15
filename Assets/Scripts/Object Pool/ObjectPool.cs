using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour, IObjectPool
{
    [SerializeField] private GameObject poolPrefab;
    [SerializeField] private List<GameObject> poolObjects = new List<GameObject>();
    private bool isInitialized = false;
    public void InitObjects(GameObject poolPrefab, int count)
    {
        if (isInitialized == true)
            throw new NotImplementedException();
        isInitialized = true;
        this.poolPrefab = poolPrefab;
        for (int i = 0; i < count; i++)
        {
            var poolObject = Instantiate(poolPrefab, transform);
            poolObject.SetActive(false);
            poolObjects.Add(poolObject);
        }       
    }

    public void Instantiate(Vector2 position, Quaternion quaternion)
    {
        
    }
}
