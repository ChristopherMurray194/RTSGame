﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    /// <summary> The offset which the object needs to be to be level with the floor </summary>
    public float yOffset = 0.11f;
    /// <summary> Can the object be placed at its current position? </summary>
    bool canPlace = true;
    Renderer rend;
    Color matInitColour;
    /// <summary> Object can be placed colour </summary>
    public Color customGreen = new Color(.46f, .89f, .38f, .5f);
    /// <summary> Object cannot be placed colour </summary>
    public Color customRed = new Color(1f, .8f, .64f, .5f);
    /// <summary> Determines whether the object has already been palced </summary>
    bool isPlaced = false;
    int floorMask;

	void Awake ()
    {
        AddRigidBody();
        AddBoxCollider();

        floorMask = LayerMask.GetMask("Floor"); // Get the mask from the Floor layer
        
        rend = GetComponent<Renderer>();
        matInitColour = rend.material.color;

        // Change to the custom transparent shader
        rend.material.shader = Shader.Find("Custom/TransparentShader");

        // Change the material colour to green to show the object is yet to be placed
        rend.material.color = customGreen;
    }

	void Update ()
    {
        if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
            PlaceObject();

        if (Input.GetKeyDown(KeyCode.R))
            Rotate();

        // If the object has not already been placed then we can place move it.
        if (!isPlaced)
            DeterminePosition();
	}

    /// <summary>
    /// Adds a rigid body to the game object so that collision detection functions.
    /// </summary>
    void AddRigidBody()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        // We don't care about applying any physics to placeable objects
        rb.isKinematic = true;
    }

    /// <summary>
    /// Adds a box collider to the game object
    /// </summary>
    void AddBoxCollider()
    {
        gameObject.AddComponent<BoxCollider>();
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
    }

    /// <summary>
    /// Handles the logic for moving the object based on the mouse position.
    /// </summary>
    void DeterminePosition()
    {
        // Cast a ray from the main camera to the mouse position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics.Raycast(camRay, out floorHit, 1000f, floorMask))
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
        rend.material.color = customRed;
    }

    void OnTriggerExit(Collider other)
    {
        // We can place the object again
        canPlace = true;

        // Revert the plane back to its original colour
        rend.material.color = customGreen;
    }

    /// <summary>
    /// Rotate the object 90 degrees.
    /// </summary>
    void Rotate()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f), 90f, Space.Self);
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
            rend.material.color = matInitColour;
            // Change back to Unity's standard shader
            rend.material.shader = Shader.Find("Standard");
            gameObject.isStatic = true;
            isPlaced = true;
        }
        // TODO: If object cannot be placed - give the player some form of audio notification that the object
        // cannot be placed there.
    }
}
