using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefab Register", menuName = "Object Pool/Prefab Register")]
public class ObjectPoolRegister : ScriptableObject
{
    [Header("Registred prefabs")]
    public List<GameObject> Prefabs = new List<GameObject>(24);
    //Refactor
    public List<int> Numbers = new List<int>(24);
    
}
