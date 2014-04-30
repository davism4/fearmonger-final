using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public Game2 game;
	public List<Person2> occupants=null;
	public List<Furniture> furnitureList=null;
	private Vector3 spawnPosition;

	new public bool enabled=false;
	private float yfloor, yceiling, xleft, xright;
	private BoxCollider2D box;
	public float numberOccupants;
	public int quality; // how nice the room is -> influences how wealthy the people are
	
	Grid2D grid; // each room has its own grid
	Vector3 gridOffset;

	public float YFloor { get {return yfloor;} }
	public float YCeiling { get {return yceiling;} }
	public float XRight { get {return xright;} }
	public float XLeft { get {return xleft;} }

	// Use this for initialization
	private void Start () {
		furnitureList = new List<Furniture>();
		occupants = new List<Person2>();
		box = transform.GetComponent<BoxCollider2D>();
		yfloor = transform.position.y-transform.localScale.y*box.size.y/2;
		yceiling = transform.position.y+transform.localScale.y*box.size.y/2;
		xleft = transform.position.x-transform.localScale.x*box.size.x/2;
		xright = transform.position.x+transform.localScale.x*box.size.x/2;
		spawnPosition = transform.FindChild("SpawnPosition").position;
		//grid = new Grid2D();
		//gridOffset = new Vector3(xLeft,yFloor,0);
		//grid.offset = gridOffset;
		//grid.Width = 5;
		//grid.Height = 2;
		//grid.CellWidth = Mathf.Abs (xright-xleft)/grid.Width;
		//grid.CellWidth = Mathf.Abs (yceiling-yfloor)/grid.Height;
	}
	
	// Update is called once per frame
	private void Update () {
	
	}

	// Called at the beginning of the night
	public void CheckIn() {
		if (enabled) {
			// calculate quality
			int totalcost=0;
			foreach (Furniture f in furnitureList){
				totalcost += f.buyCost;
			}
			quality = (totalcost/100)+1;



			//bool left = (0.5f>UnityEngine.Random.value);
			for (int i=0; i<UnityEngine.Random.Range (1,4); i++){
				GameObject personObject = null;
			/*	if (left) {		
				} else {
				}
				left = !left;*/ // Always check in to left side of the room
				Instantiate(personObject,spawnPosition,Quaternion.identity);
				personObject.GetComponent<Person2>().SetRoom(this);
			}
		}
	}

	// Called at the end of the night
	public void CheckOut() {

	}
}
