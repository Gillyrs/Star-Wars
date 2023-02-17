using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Chance
{
    private ChanceStructure chances;
    private List<bool> bools = new List<bool>();
    public Chance(ChanceStructure chances)
    {
        this.chances = chances;
        InitNumbers();
    }

    private void InitNumbers()
    {
        for (int i = 0; i < chances.Count; i++)
        {
            for (int j = 0; j < chances.GetChance(i); j++)
            {
                bools.Add(chances.GetBool(i));
            }
        }
    }

    public bool Spin()
    {
        return bools[Random.Range(0, bools.Count)] == false ? false : true;
    }
}
