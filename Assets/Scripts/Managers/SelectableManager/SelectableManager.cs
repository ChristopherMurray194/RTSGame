using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SelectableManager : MonoBehaviour
{
    int selectableMask;
    GameObject selectedObject;

    /// <summary> The game object that is displayed to the player to show which object has been selected. </summary>
    public GameObject selectionMarker;
    public float markerOffset = 1f;

    // Essentially just used to control the entering of the ToggleSelectedUI function within the Update function.
    bool uiShown = false;


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

        // If an object has been selected
        if (selectedObject != null)
        {
            // Position the marker above the selected object
            PositionMarker();
            // If the SelectedUI is not being shown
            if(!uiShown)
                // Show the SelectedUI
                ToggleSelectedUI(true);
        }
        // If there is currently no selectedObect
        else
        {
            // Hide the marker
            selectionMarker.SetActive(false);
            // If the SelectedUI is being shown
            if(uiShown)
                // Hide the SelectedUI
                ToggleSelectedUI(false);
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

        // If the ray hits an object on the selectable layer
        if (Physics.Raycast(camRay, out hit, 1000f, selectableMask))
        {
            // Get the object that was hit
            selectedObject = hit.collider.gameObject;
            // Notify the object it has been selected
            Selectable selectableScript = selectedObject.GetComponent<Selectable>();
            selectableScript.IsSelected = true;
        }
        // Otherwise we are not clicking on a selectable object
        else if (selectedObject != null)
        {
            // Notify the currently selected object it has been deselected.
            //selectedObject.IsSelected = false;
            selectedObject = null;
        }
    }

    /// <summary>
    /// Position the selectionMarker above the selected object.
    /// </summary>
    void PositionMarker()
    {
        selectionMarker.SetActive(true);

        Collider col = selectedObject.GetComponent<Collider>();
        if (col != null)
        {
            Vector3 markerPos = selectedObject.transform.position;
            markerPos.y += col.bounds.extents.magnitude + markerOffset;
            selectionMarker.transform.position = markerPos;
        }
    }

    /// <summary>
    /// Toggles the SelectedUI GameObject via the passed parameter.
    /// </summary>
    void ToggleSelectedUI(bool isActive)
    {
        if (isActive)
        {
            GameObject HUD = GameObject.Find("HUDCanvas");
            GameObject sUI = HUD.transform.FindChild("SelectedUI").gameObject;
            sUI.SetActive(isActive);
            uiShown = isActive;
        }
        else if(!isActive)
        {
            GameObject HUD = GameObject.Find("HUDCanvas");
            GameObject sUI = HUD.transform.FindChild("SelectedUI").gameObject;
            sUI.SetActive(isActive);
            uiShown = isActive;
        }
    }
}
