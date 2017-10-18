using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : Placeable
{
    public bool isInitiallyPlaced = false;
    public bool isClone = false;
    // Number of instantiated objects
    int nInstances = 0;
    // List of instantiated objects
    List<GameObject> clones = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        // If the left mouse button is pressed once
        if (Input.GetMouseButtonDown(0) && !isInitiallyPlaced)
        {
            // Place the object but do not finalise its position by invoking PlaceObject function
            isInitiallyPlaced = true;
        }
        // If the left mouse button is pressed a second time
        else if(Input.GetMouseButtonDown(0) && isInitiallyPlaced)
        {
            // Finalise the object's position and all subsequently spawned objects
        }

        if(isInitiallyPlaced && !isClone)
        {
            InstantiateClones();
            DetermineClonePos();
            DestroyClones();
        }

        if (Input.GetMouseButtonDown(1)) // 1 for right mouse button
        {
            nInstances = 0;
            Deselect();
        }

        // If the object has not already been placed then we can move it.
        if (!isInitiallyPlaced)
        {
            if (Input.GetKeyDown(KeyCode.R))
            Rotate();

            // If the game object and therefore this script have not been destroyed
            if (this != null)
                DeterminePosition();
        }
    }

    /// <summary>
    /// Create new instances of this game object
    /// </summary>
    void InstantiateClones()
    {
        if (Mathf.Floor(positionToMouse().magnitude / 3.74f) > nInstances)
        {
            // Create a new instnace of this game object
            GameObject copy = Instantiate(gameObject);
            // Ensure all instantiated objects interpret a mouse click as their second mouse click as opposed to the first
            Dragable script = copy.GetComponent<Dragable>();
            script.isInitiallyPlaced = true;
            script.isClone = true;
            // Add to the list
            clones.Add(copy);
            
            // Number of instances in the scene has increased
            ++nInstances;
        }
    }

    /// <summary>
    /// Determines each clone's position based on its index in the array (the order in which they were instantiated)
    /// and the positionToMouse vector's direction.
    /// </summary>
    void DetermineClonePos()
    {
        for(int i = 0; i < nInstances; i++)
        {
            Vector3 temp = clones[i].transform.position;
            temp.x = gameObject.transform.position.x + -(3.74f * i);
            clones[i].transform.position = temp;
        }
    }

    /// <summary>
    /// Remove any clones if the length of mouseToPosition vector is less than the combined width of all clones.
    /// </summary>
    void DestroyClones()
    {
        // Total number of clones that should currently be spawned
        float numberToSpawn = Mathf.Floor(positionToMouse().magnitude / 3.74f);
        // If the combined width of the clone instances AND this object's width is greater than
        // the length of the vector from this object's position to the mouse's position...
        // we need to remove some clones.
        if(numberToSpawn < nInstances)
        {
            // Remove the last clone from the list n (the difference between numberToSpawn and the number of clones in the scene) times
            for(int i = 0; i < (nInstances - numberToSpawn); i++)
            {
                // Create a copy of the last clone in the list
                GameObject lastClone = clones[clones.Count - 1];
                Dragable script = lastClone.GetComponent<Dragable>();
                // Remove it from the list
                clones.RemoveAt(clones.Count - 1);
                // Remove it from the scene
                script.Deselect();
                --nInstances;
            }
        }
    }

    Vector3 positionToMouse()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(camRay, out floorHit, 1000f, floorMask))
        {
            mousePosition = floorHit.point;
        }
        Vector3 posToMouse = mousePosition - transform.position;
        return posToMouse;
    }
}
