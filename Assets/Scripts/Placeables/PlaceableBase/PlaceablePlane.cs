using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablePlane : MonoBehaviour
{
    Placeable placeableScript;
    Renderer renderer;
    Color matInitColour;

    void Awake ()
    {
        placeableScript = GetComponentInParent<Placeable>();
        renderer = GetComponent<Renderer>();
        matInitColour = renderer.material.color;
    }
	
	void Update ()
    {
		
	}

    void OnTriggerStay(Collider other)
    {
        /*
         * NOTE: If the collider is always triggered, ensure parent of the plane object does not have
         * a collider active as it may be colliding with the parent object/building already inside 
         * the collider.
         */
        // If the object we have collided with is another placeable object
        if (other.tag == "Placeable")
        {
            // We cannot place there.
            placeableScript.CanPlace = false;
            // Switch the material colour to red to notify the player object cannot be placed
            renderer.material.color = new Color(1f, 0f, 0f, .5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the object leaves the collider
        if (other.tag == "Placeable")
        {
            // We can place the object again
            placeableScript.CanPlace = true;
            // Revert the plane back to its original colour
            renderer.material.color = matInitColour;
        }
    }
}
