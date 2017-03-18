using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour
{
    /// <summary> Camera movement speed </summary>
    public float movementSpeed = 2.0f;
    /// <summary> Camera rotation speed </summary>
    public float rotationSpeed = 4.0f;

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
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
        transform.Rotate(0f, (r * rotationSpeed), 0f, Space.World);
    }
}
