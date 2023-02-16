using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour, IObjectPool
{
    public GameObject Prefab { get; set; }
    [SerializeField] private List<GameObject> poolObjects = new List<GameObject>();
    private bool isInitialized = false;
    public void InitObjects(GameObject poolPrefab, int count)
    {
        if (isInitialized == true)
            throw new NotImplementedException();
        isInitialized = true;
        Prefab = poolPrefab;
        for (int i = 0; i < count; i++)
        {
            var poolObject = Instantiate(poolPrefab, transform);
            poolObject.SetActive(false);
            poolObjects.Add(poolObject);
        }       
    }

    public GameObject Instantiate(Vector2 position, Quaternion quaternion)
    {
        var poolObject = poolObjects[0];
        poolObjects.RemoveAt(0);
        poolObject.transform.position = position;
        poolObject.transform.rotation = quaternion;
        poolObject.SetActive(true);
        return poolObject;
    }

    public void Destroy(GameObject poolObject)
    {
        poolObject.SetActive(false);
        poolObjects.Add(poolObject);
    }
}
