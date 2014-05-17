using UnityEngine;
using System.Collections;

public class CameraObject : MonoBehaviour {
	/*
	private float sizeSmall, sizeLarge;
	private Vector3 mapPosition;
	//private GameManager game;
	private GameObject cursor;
	//private SpriteRenderer renderer;

	public Rigidbody2D moveUpTriangle;
	public Rigidbody2D moveDownTriangle;

	// Use this for initialization
	private void Start () {
		Screen.showCursor = false; 
		sizeSmall=11f;
		sizeLarge=82f;
		mapPosition = GameObject.Find("GameManager").transform.position;
		SetCameraSize(sizeSmall);
		cursor = GameObject.Find ("Cursor");
		//renderer = transform.GetComponent<SpriteRenderer>();
		ZoomOut ();

		moveUpTriangle=transform.GetChild (0).GetComponent<Rigidbody2D>();
		moveDownTriangle=transform.GetChild (1).GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update () {

	}

	private void SetCameraSize(float newsize) {
		Camera.main.orthographicSize = newsize;
	}

	public void ZoomIn(RoomObject room) {
		//Debug.Log("zooming in to "+room.CameraPosition+" at room "+room.RoomName);
//		transform.position = room.CameraPosition;
		SetCameraSize(sizeSmall);
		cursor.transform.localScale *= (sizeSmall/sizeLarge);

		moveUpTriangle.renderer.enabled = true;
		moveDownTriangle.renderer.enabled = true;
	}

	public void ZoomOut() {
		transform.position = mapPosition;
		SetCameraSize(sizeLarge);
		cursor.transform.localScale *= (sizeLarge/sizeSmall);

		moveUpTriangle.renderer.enabled = false;
		moveDownTriangle.renderer.enabled = false;
	}*/
}
