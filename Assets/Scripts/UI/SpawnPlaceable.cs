using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaceable : MonoBehaviour
{
    /// <summary> The placeable object to spawn </summary>
    public GameObject placeable;

	void Start ()
    {
	    	
	}
	
	void Update ()
    {
		
	}

    /// <summary>
    /// Create an instance of the placeable object.
    /// </summary>
    public void SpawnObject()
    {
        Instantiate(placeable);
    }

}
