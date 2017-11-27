using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableManager : MonoBehaviour
{
    int selectableMask;
    Selectable selectedObject;


    void Start()
    {
        selectableMask = LayerMask.GetMask("Selectable");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetSelectedObject();
        }
    }

    /// <summary>
    /// Notify an object it has been selected and add it to the list of selected objects.
    /// </summary>
    void GetSelectedObject()
    {
        // Cast a ray from the main camera to the mouse position
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hitObject;

        // If the ray hits an object on the selectable layer
        if (Physics.Raycast(camRay, out hit, 1000f, selectableMask))
        {
            // Get the object that was hit
            hitObject = hit.collider.gameObject;
            // Notify the object it has been selected
            selectedObject = hitObject.GetComponent<Selectable>();
            selectedObject.IsSelected = true;
        }
        // Otherwise we are not clicking on a selectable object
        else if (selectedObject != null)
        {
            // Notify the currently selected object it has been deselected.
            //selectedObject.IsSelected = false;
            selectedObject = null;
        }
    }
}
