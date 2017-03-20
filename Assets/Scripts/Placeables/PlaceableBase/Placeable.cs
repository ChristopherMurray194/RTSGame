using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    /// <summary> The offset which the object needs to be to be level with the floor </summary>
    public float yOffset = 0.11f;
    /// <summary> Can the object be placed at its current position? </summary>
    bool canPlace = true;
    Renderer renderer;
    Color matInitColour;
    /// <summary> Object can be placed colour </summary>
    public Color customGreen = new Color(.46f, .89f, .38f, .5f);
    /// <summary> Object cannot be placed colour </summary>
    public Color customRed = new Color(.84f, .07f, .07f, .85f);
    /// <summary> Determines whether the object has already been palced </summary>
    bool isPlaced = false;
    int floorMask;

	void Awake ()
    {
        floorMask = LayerMask.GetMask("Floor"); // Get the mask from the Floor layer
        
        renderer = GetComponent<Renderer>();
        matInitColour = renderer.material.color;

        // TODO: CHANGE RENDERING MODE TO TRANSPARENT

        // Change the material colour to green to show the object is yet to be placed
        renderer.material.color = customGreen;

        // Check that the object actually has a collider. Otherwise the collision checks are redundant.
        Collider colliderCheck = GetComponent<Collider>();
        if (colliderCheck == null) Debug.LogError("You forgot to add a collider to " + gameObject.name);
    }

	void Update ()
    {
        if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
            PlaceObject();

        // If the object has not already been placed then we can place move it.
        if (!isPlaced)
            DeterminePosition();
	}

    /// <summary>
    /// Handles the logic for moving the object based on the mouse position.
    /// </summary>
    void DeterminePosition()
    {
        // Cast a ray from the main camera to the mouse position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics.Raycast(camRay, out floorHit, 100f, floorMask))
        {
            Vector3 pos = transform.position;
            // The placeable object's position is the mouse's position relative to the floor
            pos = floorHit.point;
            pos.y += yOffset;
            transform.position = pos;
        }
    }

    void OnTriggerStay(Collider other)
    {
        /*
         * NOTE: If the collider is always triggered, ensure parent of the plane object does not have
         * a collider active as it may be colliding with the parent object/building already inside 
         * the collider.
         */

        // We cannot place there.
        canPlace = false;

        // Switch the material colour to red to notify the player object cannot be placed
        renderer.material.color = customRed;
    }

    void OnTriggerExit(Collider other)
    {
        // We can place the object again
        canPlace = true;

        // Revert the plane back to its original colour
        renderer.material.color = customGreen;
    }

    /// <summary>
    /// Finalises the object's position.
    /// </summary>
    void PlaceObject()
    {
        // If the object can be placed and has not already been placed.
        if(canPlace && !isPlaced)
        {
            // Change object back to its original material colour
            renderer.material.color = matInitColour;
            // TODO: CHANGE RENDERING MODE BACK TO OPAQUE
            isPlaced = true;
        }
        // TODO: If object cannot be placed - give the player some form of audio notification that the object
        // cannot be placed there.
    }
}
