using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    /// <summary> The offset which the object needs to be to be level with the floor </summary>
    public float yOffset = 0.11f;
    /// <summary> Can the object be placed at its current position? </summary>
    bool canPlace = true;
    /// <summary> Determines whether the object has already been palced </summary>
    bool isPlaced = false;

    int floorMask;

	void Awake ()
    {
        floorMask = LayerMask.GetMask("Floor"); // Get the mask from the Floor layer
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

    /// <summary>
    /// Finalises the object's position.
    /// </summary>
    void PlaceObject()
    {
        // If the object can be placed and has not already been placed.
        if(canPlace && !isPlaced)
        {
            isPlaced = true;
        }
        // TODO: If object cannot be placed - give the player some form of notification that the object
        // cannot be placed there.
    }
}
