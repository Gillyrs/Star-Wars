using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct ChanceStructure
{
    public int Count;
    private int[] chances;
    private bool[] miss;

    public ChanceStructure(int[] chances, bool[] actions)
    {
        if (chances.Length != actions.Length)
            throw new ArgumentException();
        this.chances = chances;
        this.miss = actions;
        Count = chances.Length;
    }
    public int GetChance(int index)
    {
        return chances[index];
    }

    public bool GetBool(int index)
    {
        return miss[index];
    }
}
