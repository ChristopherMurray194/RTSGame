using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaceable : MonoBehaviour
{
    /// <summary> The placeable object to spawn </summary>
    public GameObject placeable;

    PlaceableManager objMgrScript;

	void Start ()
    {
        objMgrScript = GameObject.Find("PlaceableManager").GetComponent<PlaceableManager>();
	}
	
	void Update ()
    {
		
	}

    /// <summary>
    /// Create an instance of the placeable object.
    /// </summary>
    public void SpawnObject()
    {
        if (objMgrScript.CheckCanSpawn(placeable))
        {
            Instantiate(placeable);
        }
        // TODO: If it cannot be spawned notify the user why not.
    }

}
