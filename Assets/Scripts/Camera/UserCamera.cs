using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour
{
    /// <summary> Camera movement speed </summary>
    public float movementSpeed = 2.0f;
    /// <summary> Camera rotation speed </summary>
    public float rotationSpeed = 1.0f;

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
        float v = 0;                                // Mapped to X and C keys
        if (Input.GetKey(KeyCode.C))
            v = 1;
        else if (Input.GetKey(KeyCode.X))
            v = -1;
        else v = 0;
        float z = Input.GetAxisRaw("Vertical");     // Maps to W and S keys

        Move(h, v, z);
        Rotate(h, v, z);
    }

    /// <summary>
    /// Handles camera movement based on the given inputs; h and v.
    /// </summary>
    /// <param name="h"> The horizontal axis input </param>
    /// <param name="v"> The vertical axis input </param>
    void Move(float h, float v, float z)
    {
        Transform transform = GetComponent<Transform>();
        // Obtain the new camera position
        Vector3 position = transform.position;
        position.x += h * movementSpeed * Time.deltaTime;
        position.y += v * movementSpeed * Time.deltaTime;
        position.z += z * movementSpeed * Time.deltaTime;
        // Update the camera's position
        transform.position = position;
    }

    void Rotate(float h, float v, float z)
    {

    }
}
