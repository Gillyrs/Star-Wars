using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    void Instantiate(Vector2 position, Quaternion quaternion);
}
