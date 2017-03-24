using System.Collections;
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

    // Stores number of pizza shops spawned
    int pizzaShopCount = 0;
    public int PizzaShopCount
    {
        get { return pizzaShopCount; }
        set { pizzaShopCount = value; }
    }
    int pizzaShopCap = 10;

	void Start ()
    {
        
	}
	
	void Update ()
    {
		
	}

    /// <summary>
    /// Check whether we can spawn the obj.
    /// </summary>
    /// <param name="obj"> Object to spawn </param>
    public bool CheckCanSpawn(GameObject obj)
    {
        // TODO: Player will need to be notified the spawn cap has been reached
        return (canSpawn && pizzaShopCount < pizzaShopCap) ? true : false;
    }
}
