using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableCounter
{
    private string name;
    /// <summary> The name of the placeable item </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private int pCount;
    /// <summary> The number of instances of this placeable gameobject currently in the scene </summary>
    public int PCount
    {
        get { return pCount; }
        set { pCount = value; }
    }

    private int pCap;
    /// <summary> The maximum instances of this placeable gameobject allowed in the scene </summary>
    public int PCap
    {
        get { return pCap; }
        set { pCap = value; }
    }

    /// <summary>
    /// Tracks how many instances of a placeable item with the same name as 'name' are in the scene.
    /// </summary>
    /// <param name="name"> Name of the placeable item </param>
    /// <param name="pCap"> The maximum number of instances which can be in the scene </param>
    /// <param name="pCount"> The current number of instances in the scene </param>
    public PlaceableCounter(string name, int pCap, int pCount = 0)
    {
        this.name = name;
        this.pCap = pCap;
        this.pCount = pCount;
    }

    /// <summary>
    /// Returns true if the maximum number of instances in the scene has not been reached.
    /// </summary>
    public bool isCapNotReached()
    {
        if (pCount >= pCap)
            return false;
        return true;
    }
}
