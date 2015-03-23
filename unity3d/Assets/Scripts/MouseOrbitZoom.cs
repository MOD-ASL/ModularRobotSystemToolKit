using UnityEngine;
using System.Collections;

public class MouseOrbitZoom : MonoBehaviour 
{
	//
	// VARIABLES
	//
	
	public float turnSpeed;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed;		// Speed of the camera when being panned
	public float zoomSpeed;		// Speed of the camera going back and forth
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?
	
	//
	// UPDATE
	//
	
	void Update () 
	{
		// Get the left mouse button
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}
		
		// Get the right mouse button
		if(Input.GetMouseButtonDown(2))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}
		
		// Disable movements on button release
		if (!Input.GetMouseButton(1)) isRotating=false;
		if (!Input.GetMouseButton(2)) isPanning=false;
		
		// Rotate camera along X and Y axis
		if (isRotating)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			
			transform.RotateAround(new Vector3 (0,0,0), transform.right, -pos.y * turnSpeed);
			transform.RotateAround(new Vector3 (0,0,0), Vector3.up, pos.x * turnSpeed);
			mouseOrigin = Input.mousePosition;
		}
		
		// Move the camera on it's XY plane
		if (isPanning)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			Vector3 move = new Vector3(-pos.x * panSpeed, -pos.y * panSpeed, 0);
			transform.Translate(move, Space.Self);
			mouseOrigin = Input.mousePosition;
		}

		// Zoom
		transform.Translate(Vector3.forward * zoomSpeed *Input.GetAxis("Mouse ScrollWheel"));
	}
}