using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    Selectable selectScript;
    int floorMask;
    NavMeshAgent navAgent;
    float stoppingDistance = 1.0f;

    Animator anim;
    /// <summary> Store the destination the character is moving towards </summary>
    Vector3 newPosition = new Vector3();
    bool isWalking = false;
    public bool IsWalking
    {
        get { return isWalking; }
    }


	void Start ()
    {
        anim = GetComponent<Animator>();
        selectScript = GetComponent<Selectable>();
        floorMask = LayerMask.GetMask("Floor");
        navAgent = GetComponent<NavMeshAgent>();
        stoppingDistance = navAgent.stoppingDistance;
    }
	
	void Update ()
    {
        /*
         * If the distance from the character's position and the destination position is less than or equal to the stopping distance,
         * we can say the character has reached its destination
         */
        if (Vector3.Distance(transform.position, newPosition) <= stoppingDistance)
        {
            isWalking = false;
            anim.SetBool("isWalking", isWalking);
        }

        // If this character is currently selected AND the right mouse button has been pressed
        if (selectScript.IsSelected && Input.GetMouseButtonDown(1))
        {
            Walk();
        }
	}
    
    /// <summary>
    /// Move the character to a new position.
    /// </summary>
    void Walk()
    {
        // Cast a ray from the main camera to the mouse position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
            
        if (Physics.Raycast(camRay, out hit, 1000f, floorMask))
        {
            // The new position to move to is the position the ray hit
            newPosition = hit.point;
        }

        // Make the character navigate the navmesh to the new position
        navAgent.destination = newPosition;
        isWalking = true;
        anim.SetBool("isWalking", isWalking);
    }
}
