using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    /// <summary> The offset which the object needs to be to be level with the floor </summary>
    public float yOffset = 0.11f;
    /// <summary> Can the object be placed at its current position? </summary>
    bool canPlace = true;

    int floorMask;

	void Awake ()
    {
        floorMask = LayerMask.GetMask("Floor"); // Get the mask from the Floor layer
	}

	void Update ()
    {
        DeterminePosition();
	}

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
}
