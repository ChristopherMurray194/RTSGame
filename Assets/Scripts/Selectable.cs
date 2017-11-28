using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    bool isSelected = false;
    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    /// <summary> The source image to be displayed on the UI </summary>
    public GameObject sourceImage;
    

	void Start ()
    {
        // Ensure this gameobject to the 'Selectable' Layer
        // - I'd likely forget to do it manually for each new object.
        gameObject.layer = 9;
	}
	
	void Update ()
    {
        /*
         * If this object is currently selected and the corresponding mouse button below, 
         * is pressed, deselect the object.
         */
        if (isSelected && Input.GetMouseButtonDown(0))
            isSelected = false;

	}
}
