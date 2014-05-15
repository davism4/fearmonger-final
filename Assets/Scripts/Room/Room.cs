using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {
	public bool HASLOADED=false;
	public Game2 game;
	public List<Person2> occupants=new List<Person2>();
	private List<GameObject> items=new List<GameObject>();

	private Vector3 spawnPosition;
	public AudioClip doorSound;
	public bool open=false;
	private float yfloor, yceiling, xleft, xright;
	private BoxCollider2D box;
	public int numberOccupants {
		get {return occupants.Count; }
	}
	public int quality; // how nice the room is -> influences how wealthy the people are
	public float doorLeft {
		get { return spawnPosition.x; }
	}
	
//	Grid2D grid; // each room has its own grid?
	//Vector3 gridOffset; // ?

	public float YFloor { get {return yfloor;} }
	public float YCeiling { get {return yceiling;} }
	public float XRight { get {return xright;} }
	public float XLeft { get {return xleft;} }
	private GameObject[,] people;
	public Node[] nodes;

	// GRID LOGIC HERE

	// Use this for initialization
	private void Start () {
		game = GameObject.Find ("Main Game").GetComponent<Game2>();
		box = transform.GetComponent<BoxCollider2D>();
		yfloor = transform.position.y-transform.localScale.y*box.size.y/2;
		yceiling = transform.position.y+transform.localScale.y*box.size.y/2;
		xleft = transform.position.x-transform.localScale.x*box.size.x/2;
		xright = transform.position.x+transform.localScale.x*box.size.x/2;
		spawnPosition = transform.FindChild("SpawnPosition").position;

	//	Debug.Log ("Ceiling: " + yceiling);
	//	Debug.Log ("floor: " + yfloor);
	//	Debug.Log ("left: " + xleft);
	//	Debug.Log ("right: " + xright);

		//grid = this.GetComponent<Grid2D>();
		//gridOffset = new Vector3(xLeft,yFloor,0);
		//grid.offset = gridOffset;

		//grid = GetComponent<Grid2D>();
		
		//grid.Width = 5;
		//grid.Height = 1;
		//grid.CellWidth = (Mathf.Abs (xright-xleft)/grid.Width)/transform.localScale.x;
//		Debug.Log ("CellWidth: " + grid.CellWidth);
		//grid.CellHeight = Mathf.Abs (yceiling-yfloor)/grid.Height;
//		Debug.Log ("CellHeight: " + grid.CellHeight);
		nodes = transform.GetComponentsInChildren<Node>();
		//nodes = transform.
		/*
		for (int y = 0; y < grid.Height; ++y)
		{
			for (int x = 0; x < grid.Width; ++x)
			{
				// Instantiate new object
				GameObject instance = Instantiate(PrefabToPlace) as GameObject;
				
				// Move to correct position
				instance.transform.position = grid.GetBottomCenter(x, y);
				
				// Add the object to the grid
				grid.Add(instance.transform, x, y);
			}
		}
		*/

		people = new GameObject[3,2];
		people[0,0] = Resources.Load<GameObject>("Prefabs/Person/Boy");
		people[0,1] = Resources.Load<GameObject>("Prefabs/Person/Girl");
		people[1,0] = Resources.Load<GameObject>("Prefabs/Person/Man");
		people[1,1] = Resources.Load<GameObject>("Prefabs/Person/Lady");
		people[2,0] = Resources.Load<GameObject>("Prefabs/Person/RichMan");
		people[2,1] = Resources.Load<GameObject>("Prefabs/Person/RichLady");
		//personThug = Resources.Load<GameObject>("Prefabs/Person/Thug");
		//personPriest = Resources.Load<GameObject>("Prefabs/Person/Priest");
		HASLOADED=true;
	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown("m") && open)
			CheckIn ();
		if (Input.GetKeyDown("n"))
			CheckOut ();
	}

	public void PlayDoorSound(){
		if (doorSound!=null)
			AudioSource.PlayClipAtPoint (doorSound, transform.position);
	}

	public void AddItem(GameObject g){
		items.Add (g);
//		Debug.Log("Added "+g.name+" to furniture list. List has "+items.Count);
	}

	public void RemoveItem(GameObject g){
		items.Remove (g);
		g.GetComponent<Furniture>().Sell ();
	}

	public int TrapCount(){
		int i=0;
		if (items.Count>0)
		foreach (GameObject g in items){
			if (g.GetComponent<Furniture>() is Trap)
				i++;
		}
		return i;
	}

	public int NonTrapFurnitureCount(){
		int i=0;
		if (items.Count>0)
		foreach (GameObject g in items){
			if (!(g.GetComponent<Furniture>() is Trap))
				i++;
		}
		return i;
	}

	// Generates a random person based on the quality of the room
	// Chance of a rich person appearing is proportional to the quality
	private GameObject RandomPerson(bool adult){
		int gender = UnityEngine.Random.Range (0,2);
		if (adult){
			if (quality>4 && UnityEngine.Random.value<0.1f*quality)
				return people[2,gender];
			else // below a quality level, no rich people check in
				return people[1,gender];
		} else if (quality <=4) {// no rich people
			if (UnityEngine.Random.value<0.5f)
				return people[2,gender];
			else
				return people[0,gender];
		} else {
			if (UnityEngine.Random.value<0.1f*quality)
				return people[2,gender];
			else if (UnityEngine.Random.value<0.5f)
				return people[1,gender];
			else
				return people[0,gender];
		}
	}

	// the priest or thug enters/exits a room
	public void AddEnemy(GameObject enemy){
		//Instantiate(enemy,spawnPosition,Quaternion.identity);
		enemy.transform.position = spawnPosition;
		Person2 p = enemy.GetComponent<Person2>();
		p.SetRoom (this);
		occupants.Add (p);
	}
	public void RemoveEnemy(GameObject enemy, bool stay){
		Person2 p = enemy.GetComponent<Person2>();
		enemy.transform.position -= new Vector3(-100,0,0);// hide off-screen for a bit
		occupants.Remove (p);
		game.CheckOutEnemy (p, stay);
	}

	// Called at the beginning of the night
	public void CheckIn() {
		foreach (Person2 p in occupants){
			if (p!=null)
				p.Exit(true);
		}
		foreach (Node n in nodes){
			n.BoxDisable ();
		}
		occupants.Clear ();
		if (open) {
			// calculate quality
			int totalcost=0;
			foreach (GameObject g in items){
				totalcost += g.GetComponent<Furniture>().buyCost;
				if (g.GetComponent<Furniture>() is Trap)
					(g.GetComponent<Furniture>() as Trap).Reset();

			}
			quality = (totalcost/100)+1;
			GameObject personGen = RandomPerson (true);
			GameObject pero = Instantiate(personGen,spawnPosition,Quaternion.identity) as GameObject;
			Person2 p = pero.GetComponent<Person2>();
			p.SetRoom (this);
			occupants.Add (p);
			for (int i=0; i<UnityEngine.Random.Range(1,4); i++){
				personGen = RandomPerson (false);
				pero = Instantiate(personGen,spawnPosition,Quaternion.identity) as GameObject;
				p = pero.GetComponent<Person2>();
				p.SetRoom (this);
				occupants.Add (p);
			}
		}
	}

	// Called at the end of the night
	public void CheckOut() {
//		Debug.Log ("Checking out "+occupants.Count+ " people");
		foreach (Person2 p in occupants){
//			Debug.Log (p.name + " is checking out");
			p.Leave ();
		}
		foreach(Node n in nodes){
			n.BoxEnable();
		}

		//occupants.Clear ();
	}

	public void DisplayGrid(bool on){
		foreach (Node n in nodes){
			n.DisplayGrid(on);
		}
	}

	private void OnGUI(){
	}
}
