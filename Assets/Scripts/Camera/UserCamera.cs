using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour
{
    /// <summary> Camera movement speed </summary>
    public float movementSpeed = 100.0f;
    /// <summary> Camera rotation speed </summary>
    public float rotationSpeed = 4.0f;

    int floorMask;
    Camera cam;
    /// <summary> Default height of the camera </summary>
    public Vector3 defaultCamHeight;

    void Start()
    {
        floorMask = LayerMask.GetMask("Floor"); // Get the mask from the Floor layer
        cam = GetComponentInChildren<Camera>();
        defaultCamHeight = transform.position;
    }

    void Update()
    {
        // Get inputs
        // The value will be in the range -1...1. For keyboard input this will either be -1, 0, 1
        float h = Input.GetAxisRaw("Horizontal");   // Maps to A and D keys
        float v = 0f;                               // Mapped to X and C keys
        if (Input.GetKey(KeyCode.C))
            v = 1f;
        else if (Input.GetKey(KeyCode.X))
            v = -1f;
        else v = 0f;
        float z = Input.GetAxisRaw("Vertical");     // Maps to W and S keys
        // Apply movement
        Move(h, v, z);

        float r = 0f;                               // Maps to Q and E keys
        if (Input.GetKey(KeyCode.E))
            r = 1f;
        else if (Input.GetKey(KeyCode.Q))
            r = -1f;
        else r = 0f;
        // Apply yaw rotation
        Yaw(r);

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        // Get mouse scroll wheel input to zoom
        if(scrollInput != 0)
            Zoom(scrollInput);

        if(Input.GetMouseButtonDown(2))
            resetZoom();
    }

    /// <summary>
    /// Handles the camera parent transform (not the camera's iteself) movement based on the given inputs; h and v.
    /// </summary>
    /// <param name="h"> Horizontal direction defined by user input; Left if -1, Right if 1 </param>
    /// <param name="v"> Vertical direction defined by user input; Up if 1, Down if -1 </param>
    /// <param name="z"> Forward direction defined by user input; Forward if 1, Backwards if -1 </param>
    void Move(float h, float v, float z)
    {
        transform.Translate((h * movementSpeed * Time.deltaTime),
                            (v * movementSpeed * Time.deltaTime),
                            (z * movementSpeed * Time.deltaTime),
                            Space.Self); // Move in the forward direction of the camera parent (NOT the camera)
    }

    /// <summary>
    /// Handles the camera parent yaw rotation (not the camera's iteself) based on the value of r.
    /// </summary>
    /// <param name="r"> Rotation direction defined by user input; Clockwise if 1, Counter-clockwise if -1 </param>
    void Yaw(float r)
    {
        // Cast a ray from the main camera to the mouse position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Vector3 pos = transform.position;
        if (Physics.Raycast(camRay, out floorHit, 1000f, floorMask))
            pos = floorHit.point;
        // Rotate around the point the ray intersects 
        transform.RotateAround(pos, new Vector3(0f, 1f, 0f), (r * rotationSpeed));
    }

    /// <summary>
    /// Zooms the camera using the mouse scroll wheel.
    /// Mouse ScrollDelta is: .1 if forward on wheel, -.1 if back on wheel, 0 if no scroll.
    /// </summary>
    /// <param name="scrollDelta"> Value of the mouse scroll wheel delta </param>
    void Zoom(float scrollDelta)
    {
        Vector3 scrollVec = cam.transform.localPosition;
        if (scrollDelta > 0)
        {
            scrollVec.y--;
            scrollVec.z++;
        }
        else if (scrollDelta < 0)
        {
            scrollVec.y++;
            scrollVec.z--;
        }
        cam.transform.localPosition = scrollVec;
    }

    /// <summary>
    /// When the middle mouse button is clicked, the camera zoom resets
    /// </summary>
    void resetZoom()
    {
        cam.transform.localPosition = Vector3.zero;
    }
}
