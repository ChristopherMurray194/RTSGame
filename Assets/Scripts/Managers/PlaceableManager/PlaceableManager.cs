﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableManager : MonoBehaviour
{
    /// <summary> If true an object can be spawned </summary>
    bool canSpawn = true;
    public bool CanSpawn
    {
        get { return canSpawn; }
        set { canSpawn = value; }
    }

    List<PlaceableCounter> spawnableItems = new List<PlaceableCounter>();

	void Start ()
    {
        spawnableItems.Add(new PlaceableCounter("PizzaShop", 10));
        spawnableItems.Add(new PlaceableCounter("Apartment", 5));
	}
	
	void Update ()
    {
        
	}

    /// <summary>
    /// Increments the pCount count for the PlaceableCounter with the same name as obj.
    /// </summary>
    /// <param name="obj"> Placeable object to spawn </param>
    public void IncrementObjCount(GameObject obj)
    {
        for (int i = 0; i < spawnableItems.Count; i++)
            if (obj.name.Equals(spawnableItems[i].Name))
                spawnableItems[i].PCount++;
    }

    /// <summary>
    /// Decrements the pCount for the PlaceableCounter with the same name as obj.
    /// </summary>
    /// <param name="obj"> Placeable object which has been removed </param>
    public void DecrementObjCount(GameObject obj)
    {
        // Remove '(Clone)' from the GameObject name
        string objName = obj.name.Remove(obj.name.IndexOf('(', 7));

        for (int i = 0; i < spawnableItems.Count; i++)
            if (objName.Equals(spawnableItems[i].Name))
                spawnableItems[i].PCount--;
    }

    /// <summary>
    /// Check whether we can spawn the obj.
    /// </summary>
    /// <param name="obj"> Placeable object to spawn </param>
    public bool CheckCanSpawn(GameObject obj)
    {
        bool capReached = false;
        // TODO: Player will need to be notified the spawn cap has been reached
        for (int i = 0; i < spawnableItems.Count; i++)
        if (obj.name.Equals(spawnableItems[i].Name))
            capReached = spawnableItems[i].isCapReached();

        return (canSpawn && !capReached) ? true : false;
    }
}
