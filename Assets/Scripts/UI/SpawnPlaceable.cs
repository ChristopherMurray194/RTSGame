using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnPlaceable : MonoBehaviour
{
    /// <summary> The placeable object to spawn </summary>
    public GameObject placeable;

    PlaceableManager objMgrScript;

	void Start ()
    {
        try
        {
            objMgrScript = GameObject.Find("PlaceableManager").GetComponent<PlaceableManager>();
        }
        catch (NullReferenceException e)
        {
            // If there is no placeable manager game object, create one...
            // Create a game object with the PlaceableManager script attached
            GameObject pMgrObj = new GameObject();
            pMgrObj.AddComponent<PlaceableManager>();
            pMgrObj.name = "PlaceableManager";
            //Instantiate(pMgrObj);
            objMgrScript = pMgrObj.GetComponent<PlaceableManager>();

        }
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
