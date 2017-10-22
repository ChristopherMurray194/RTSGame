using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    /// <summary> Slight Y axis offset to avoid collisions with the floor </summary>
    public float yOffset = 0.11f;
    /// <summary> Time it takes to 'build' the object - essentially how long it takes to rise up from the ground </summary>
    public float buildTime = 2.0f;
    public float snapValue = 5f;
    /// <summary> The angle to rotate by when the R key is pressed </summary>
    public float rotationIncrement = 90.0f;
    /// <summary> Can the object be placed at its current position? </summary>
    protected bool canPlace = true;
    /// <summary> List of renderers found in children </summary>
    List<Renderer> renderers = new List<Renderer>();
    /// <summary> List of intial material colours found in children </summary>
    protected List<Color> matInitColours = new List<Color>();
    /// <summary> Object can be placed colour </summary>
    Color customGreen = new Color(.46f, .89f, .38f, .3f);
    /// <summary> Object cannot be placed colour </summary>
    Color customRed = new Color(1f, .8f, .64f, .3f);
    /// <summary> Determines whether the object has already been placed </summary>
    bool isPlaced = false;
    protected int floorMask;
    protected PlaceableManager placeableMgrScript;

    /// <summary> The Y value of the floor hit </summary>
    float floorY = 0f;

    protected virtual void Awake()
    {
        if(GetComponent<Rigidbody>() == null)
            AddRigidBody();
        if(GetComponent<BoxCollider>() == null || GetComponentInChildren<BoxCollider>() == null)
            AddBoxCollider();

        floorMask = LayerMask.GetMask("Floor"); // Get the mask from the Floor layer

        // Remove '(Clone)' from the GameObject name
        gameObject.name = name.Remove(name.IndexOf('('));

        placeableMgrScript = GameObject.Find("PlaceableManager").GetComponent<PlaceableManager>();
        // Number of this placeable type in the scene has increased
        placeableMgrScript.IncrementObjCount(gameObject);
        // Notify the manager that this object has yet to be placed, so no new placeable objects can be spawned
        placeableMgrScript.CanSpawn = false;

        // If there is a renderer component in the main game object, I assume there is no children
        if (GetComponent<Renderer>() != null)
        {
                renderers.Add(GetComponent<Renderer>());
        }
        else
        {
            // Obtain the renders from the children
            Renderer[] tempRenderer = GetComponentsInChildren<Renderer>();
            if (tempRenderer != null)
                foreach (Renderer r in tempRenderer)
                    renderers.Add(r);
        }

        // Add the initial model materials to the list
        foreach (Renderer r in renderers)
            matInitColours.Add(r.material.color);

        ApplyCanPlaceShader();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
            PlaceObject();

        if (Input.GetKeyDown(KeyCode.R))
            Rotate();

        // If the object has not already been placed then we can move it.
        if (!isPlaced)
        {
            if (Input.GetMouseButtonDown(1)) // 1 for right mouse button
                Deselect();

            // If the game object and therefore this script have not been destroyed
            if (this != null)
                DeterminePosition();
        }
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
    /// Adds a box collider to the game object or its children (those with meshes) 
    /// </summary>
    void AddBoxCollider()
    {
        // If this game object has a mesh - then I am assuming it has NO children with meshes
        if (GetComponent<MeshFilter>() != null)
        {
            gameObject.AddComponent<BoxCollider>();
            BoxCollider bc = GetComponent<BoxCollider>();
            bc.isTrigger = true;
            // Reduce the scale of the collider slightly so placeables can be placed flush with the same type
            Vector3 scaleVec = bc.size;
            scaleVec.x = Mathf.Floor(scaleVec.x);
            scaleVec.y = Mathf.Floor(scaleVec.y);
            scaleVec.z = Mathf.Floor(scaleVec.z);
            bc.size = scaleVec;
        }
        else
        {
            // Find all children with a mesh
            Component[] children = GetComponentsInChildren<MeshFilter>();
            foreach(Component child in children)
            {
                // Add a box collider to each game object
                child.gameObject.AddComponent<BoxCollider>();
                BoxCollider bc = child.gameObject.GetComponent<BoxCollider>();
                bc.isTrigger = true;
                Vector3 scaleVec = bc.size;
                scaleVec.x = Mathf.Floor(scaleVec.x);
                scaleVec.y = Mathf.Floor(scaleVec.y);
                scaleVec.z = Mathf.Floor(scaleVec.z);
                bc.size = scaleVec;
            }
        }
    }

    /// <summary>
    /// Applies the custom green shader to the mesh and (if any) all child meshes.
    /// Which notifies the user the object can be placed.
    /// </summary>
    void ApplyCanPlaceShader()
    {
        if (renderers != null)
        {
            // Change to the custom transparent shader
            foreach (Renderer r in renderers)
                r.material.shader = Shader.Find("Custom/TransparentShader");
        }

        ApplyGreenMaterial();
    }

    void ApplyGreenMaterial()
    {
        if (renderers != null)
            foreach (Renderer r in renderers)
                // Change the material colour to green to show the object is yet to be placed
                r.material.color = customGreen;
    }

    protected void ApplyRedMaterial()
    {
        if (renderers != null)
            foreach(Renderer r in renderers)
                // Switch the material to red to notify the player object cannot be placed
                r.material.color = customRed;
    }

    /// <summary>
    /// Handles the logic for moving the object based on the mouse position.
    /// </summary>
    protected void DeterminePosition()
    {
        // Cast a ray from the main camera to the mouse position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Vector3 pos = transform.position;

        if (Physics.Raycast(camRay, out floorHit, 1000f, floorMask))
        {
            // The placeable object's position is the mouse's position relative to the floor
            pos = floorHit.point;
            pos.y += yOffset;
            // Save the Y value of the point where the ray hit
            floorY = floorHit.point.y;
        }

        // Only snap position if the camera is not being moved. I don't like the juddering effect caused.
        if ((Input.GetAxisRaw("Horizontal") == 0)
            && (Input.GetAxisRaw("Vertical") == 0))
        {
            // snap the object
            pos.x = snapRound(pos.x);
            pos.z = snapRound(pos.z);
        }

        transform.position = pos;
    }

    /// <summary>
    /// Rounds the input to the nearest multiple of snapValue.
    /// </summary>
    /// <param name="inVal"></param>
    /// <returns></returns>
    float snapRound(float inVal)
    {
        return snapValue * (Mathf.Round(inVal / snapValue));
    }

    void OnTriggerStay(Collider other)
    {
        if (this.enabled)
        {
            // We cannot place there.
            canPlace = false;

            ApplyRedMaterial();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (this.enabled)
        {
            // We can place the object again
            canPlace = true;

            // Revert the material back to green to show the player the object is able to be placed
           ApplyGreenMaterial();
        }
    }

    /// <summary>
    /// Rotate the placeable object 90 degrees.
    /// </summary>
    protected void Rotate()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f), rotationIncrement, Space.Self);
    }

    /// <summary>
    /// Finalises the placeable object's position.
    /// </summary>
    protected void PlaceObject()
    {
        // If the object can be placed and has not already been placed.
        if (canPlace)
        {
            // The size of the renderers list AND the matInitColours list SHOULD (I assume) always be the same...
            for(int i = 0; i < renderers.Count; i++)
            {
                // Change object back to its original material colour
                renderers[i].material.color = matInitColours[i];
                // Change back to Unity's standard shader
                renderers[i].material.shader = Shader.Find("Standard");
            }
            isPlaced = true;
            // Object has been placed, a new placeable can be spawned
            placeableMgrScript.CanSpawn = true;

            // Make the object rise from the ground
            StartCoroutine(Rise());
            gameObject.isStatic = true;
            // Object has been placed disable this script!
            this.enabled = false;
        }
        // TODO: If object cannot be placed - give the player some form of audio notification that the object
        // cannot be placed there.
    }

    /// <summary>
    /// A coroutine which rises the object from the ground.
    /// </summary>
    IEnumerator Rise()
    {
        // Move the object below the floor
        transform.Translate(new Vector3(0f, (transform.position.y - (yOffset * 4) - floorY), 0f));

        float timer = 0f;

        // If the buildTime in seconds has not passed
        while (timer < buildTime)
        {
            timer += Time.deltaTime;
            // Move the object up at increments
            transform.Translate(new Vector3(0, (yOffset * 3/buildTime) * Time.deltaTime, 0));
            yield return null;
        }
    }

    /// <summary>
    /// This object is no longer going to be placed, destroy it.
    /// </summary>
    protected void Deselect()
    {
        // Notifiy the manager that a new placeable object can be spawned in place of this one, as we are about to destroy it
        placeableMgrScript.CanSpawn = true;
        /*  Decrement the count of placeable gameObject types in the scene. i.e. If we have a placeable gameObject named 'House',
            this would decrement the count of Houses in the scene. */
        placeableMgrScript.DecrementObjCount(gameObject);
        GameObject.Destroy(gameObject);
    }
}
