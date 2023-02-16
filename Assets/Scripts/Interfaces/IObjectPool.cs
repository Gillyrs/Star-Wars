using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    public GameObject Prefab { get; set; }
    GameObject Instantiate(Vector2 position, Quaternion quaternion);
}
