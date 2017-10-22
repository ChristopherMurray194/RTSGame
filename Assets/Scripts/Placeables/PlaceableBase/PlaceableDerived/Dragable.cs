using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : Placeable
{
    public float objectWidth = 3.74f;
    protected bool isInitiallyPlaced = false;
    protected bool isClone = false;
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
        Debug.Log(gameObject.transform.forward);
        if (canPlace)
        {
            // If the left mouse button is pressed once
            if (Input.GetMouseButtonDown(0) && !isInitiallyPlaced)
            {
                // Place the object but do not finalise its position by invoking PlaceObject function
                isInitiallyPlaced = true;
            }
            // If the left mouse button is pressed a second time
            else if (Input.GetMouseButtonDown(0) && isInitiallyPlaced)
            {
                FinalisePositions();
            }

            if (isInitiallyPlaced && !isClone)
            {
                InstantiateClones();
                DetermineClonePos();
                DestroyClones();
            }
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
        if (placeableMgrScript.CheckCanClone(gameObject))
        {
            if (Mathf.Floor(positionToMouse().magnitude / objectWidth) > nInstances)
            {
                // Create a new instnace of this game object
                GameObject copy = Instantiate(gameObject);
                // Ensure all instantiated objects interpret a mouse click as their second mouse click as opposed to the first
                Dragable script = copy.GetComponent<Dragable>();
                script.isInitiallyPlaced = true;
                script.isClone = true;
                /* 
                 * Because Instantiate function creates an exact clone, the clone will intialise (in Awake) 
                 * its material to the green material THIS object is intially set to.
                 * THEREFORE pass the clones the correct initial materials.
                 */
                script.matInitColours = matInitColours;
                // Number of instances in the scene has increased
                ++nInstances;
                clones.Add(copy);
            }
        }
    }

    /// <summary>
    /// Determines each clone's position based on its index in the array (the order in which they were instantiated)
    /// and the positionToMouse vector's direction.
    /// </summary>
    void DetermineClonePos()
    {
        // The current y rotation angle
        float yAngle = transform.rotation.eulerAngles.y;
        
        if (transform.forward.z >= 0)
        {
            // Clone along the z axis
            for (int i = 0; i < nInstances; i++)
            {
                clones[i].transform.position = gameObject.transform.position + 
                                            ((gameObject.transform.forward) * (objectWidth * (i + 1))) 
                                            * Mathf.Sign(gameObject.transform.forward.z);
            }
        }
        else if (transform.forward.z < 0)
        {
            // Clone along the x axis
            for (int i = 0; i < nInstances; i++)
            {
                clones[i].transform.position = gameObject.transform.position + 
                                            ((gameObject.transform.forward) * (objectWidth * (i + 1))) 
                                            * Mathf.Sign(gameObject.transform.forward.x);
            }
        }

        Debug.DrawRay(transform.position, (transform.forward * transform.rotation.y) * 100f, Color.red);
    }

    /// <summary>
    /// Remove any clones if the length of mouseToPosition vector is less than the combined width of all clones.
    /// </summary>
    void DestroyClones()
    {
        // Total number of clones that should currently be spawned
        float numberSpawned = Mathf.Floor(positionToMouse().magnitude / objectWidth);
        // If the combined width of the clone instances AND this object's width is greater than
        // the length of the vector from this object's position to the mouse's position...
        // we need to remove some clones.
        if(numberSpawned < nInstances)
        {
            // Remove the last clone from the list n (the difference between numberToSpawn and the number of clones in the scene) times
            for(int i = 0; i < (nInstances - numberSpawned); i++)
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

    /// <summary>
    /// Finalise and place this object and all subsequent clones.
    /// </summary>
    void FinalisePositions()
    {
        PlaceObject();

        foreach(GameObject clone in clones)
        {
            Dragable script = clone.GetComponent<Dragable>();
            script.PlaceObject();
        }
    }

    /// <summary>
    /// Return a vector from this object's position to the mouse's position.
    /// </summary>
    /// <returns> Vector from this game object's position to the mouse's position </returns>
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
