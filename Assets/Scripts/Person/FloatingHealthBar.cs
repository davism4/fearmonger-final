using UnityEngine;
using System.Collections;

/* modified version of code provided in
 * a tutorial found on youtube by JesseEtzler0. 
 * http://www.youtube.com/watch?v=rNeIj1Ll4_k
 */
[RequireComponent(typeof(GUITexture))]
public class FloatingHealthBar : MonoBehaviour {

	private Transform guest;                //guest health bar is for 
	public Vector3 offset = Vector3.up;     
	public bool fixBarToScreen = false;      // If true, label will be visible even if object is off screen
	public float fixBarBorderSize = .05f;    
	public bool useMainCamera = true;       // If true, use camera tagged as MainCamera
	public Camera altCamera;              // camara used if useMainCamera is false
	
	private Camera displayCamera;
	private Transform displayhealthBarTransform;  
	private Transform displayCameraTransform;  

	void Start()
	{
		guest = transform.parent; // <- this is easier
		displayhealthBarTransform = transform;

		if (useMainCamera)
			displayCamera = Camera.main;
		else
			displayCamera = altCamera;
		
		displayCameraTransform = displayCamera.transform;
	}
	
	void Update()
	{
		SetHealthBarPositionRespectCamera();
	}

	//set health bar to the position of character based on camera position
	void SetHealthBarPositionRespectCamera()
	{
		if (fixBarToScreen)
		{
			Vector3 relativePosition = displayCameraTransform.InverseTransformPoint(guest.position);
			
			float cameraMiddleGroundZ = 1.0f;     //used as middle ground for camera's relative position
			
			relativePosition.z = Mathf.Max(relativePosition.z, cameraMiddleGroundZ);
			
			//set position for element's transform
			Vector3 displayTransformPoint = displayCameraTransform.TransformPoint(relativePosition + offset);
			displayhealthBarTransform.position = displayCamera.WorldToViewportPoint(displayTransformPoint);
		}
		else
		{
			displayhealthBarTransform.position = displayCamera.WorldToViewportPoint(guest.position + offset);
		}
	}
}
