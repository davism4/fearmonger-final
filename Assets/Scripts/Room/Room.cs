using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public MainGame game;

	new public bool enabled=false;
	private float yfloor, yceiling, xleft, xright;
	private BoxCollider2D box;
	public float numberOccupants;
	
	Grid2D grid; // each room has its own grid
	Vector3 gridOffset;

	public float YFloor { get {return yfloor;} }
	public float YCeiling { get {return yceiling;} }
	public float XRight { get {return xright;} }
	public float XLeft { get {return xleft;} }

	// Use this for initialization
	private void Start () {
		box = transform.GetComponent<BoxCollider2D>();
		yfloor = transform.position.y-transform.localScale.y*box.size.y/2;
		yceiling = transform.position.y+transform.localScale.y*box.size.y/2;
		xleft = transform.position.x-transform.localScale.x*box.size.x/2;
		xright = transform.position.x+transform.localScale.x*box.size.x/2;
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

	}

	// Called at the end of the night
	public void CheckOut() {

	}
}
