using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public Game2 game;
	public List<Person2> occupants=null;
	private Vector3 spawnPosition;
	public AudioClip doorSound;
	public bool open=false;
	private float yfloor, yceiling, xleft, xright;
	private BoxCollider2D box;
	public float numberOccupants;
	public int quality; // how nice the room is -> influences how wealthy the people are
	
	Grid2D grid; // each room has its own grid?
	Vector3 gridOffset; // ?

	public float YFloor { get {return yfloor;} }
	public float YCeiling { get {return yceiling;} }
	public float XRight { get {return xright;} }
	public float XLeft { get {return xleft;} }
	private GameObject[,] people;

	// GRID LOGIC HERE

	// Use this for initialization
	private void Start () {
		game = GameObject.Find ("Main Game").GetComponent<Game2>();
		occupants = new List<Person2>();
		box = transform.GetComponent<BoxCollider2D>();
		yfloor = transform.position.y-transform.localScale.y*box.size.y/2;
		yceiling = transform.position.y+transform.localScale.y*box.size.y/2;
		xleft = transform.position.x-transform.localScale.x*box.size.x/2;
		xright = transform.position.x+transform.localScale.x*box.size.x/2;
		spawnPosition = transform.FindChild("SpawnPosition").position;
		grid = new Grid2D();
		//gridOffset = new Vector3(xLeft,yFloor,0);
		//grid.offset = gridOffset;

		Debug.Log ("Ceiling: " + yceiling);
		Debug.Log ("floor: " + yfloor);
		Debug.Log ("left: " + xleft);
		Debug.Log ("right: " + xright);

		/*
		grid.Width = 5;
		grid.Height = 1;
		grid.CellWidth = Mathf.Abs (xright-xleft)/grid.Width;
		Debug.Log ("CellWidth: " + grid.CellWidth);
		grid.CellHeight = Mathf.Abs (yceiling-yfloor)/grid.Height;
		Debug.Log ("CellHeight: " + grid.CellHeight);
		*/

		people = new GameObject[3,2];
		people[0,0] = Resources.Load<GameObject>("Prefabs/Person/Boy");
		people[0,1] = Resources.Load<GameObject>("Prefabs/Person/Girl");
		people[1,0] = Resources.Load<GameObject>("Prefabs/Person/Man");
		people[1,1] = Resources.Load<GameObject>("Prefabs/Person/Woman");
		people[2,0] = Resources.Load<GameObject>("Prefabs/Person/RichMan");
		people[2,1] = Resources.Load<GameObject>("Prefabs/Person/RichLady");
		//personThug = Resources.Load<GameObject>("Prefabs/Person/Thug");
		//personPriest = Resources.Load<GameObject>("Prefabs/Person/Priest");
	}
	
	// Update is called once per frame
	private void Update () {
	
	}

	public void PlayDoorSound(){
		
		if (doorSound!=null)
			AudioSource.PlayClipAtPoint (doorSound, transform.position);
	}

	public int TrapCount(){
		return 0;
	}

	public int NonTrapFurnitureCount(){
		return 0;
	}

	// Generates a random person based on the quality of the room
	// Chance of a rich person appearing is proportional to the quality
	private GameObject RandomPerson(bool adult){
		int gender = UnityEngine.Random.Range (0,1);
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
		if (open) {
			// calculate quality
			int totalcost=0;
			// Get a list of furniture from the Grid
			/*foreach (Furniture f in the list){
				totalcost += f.buyCost;
				if (f is Trap)
					(f as Trap).Reset();

			}*/
			quality = (totalcost/100)+1;
			GameObject personGen = RandomPerson (true);
			Instantiate(personGen,spawnPosition,Quaternion.identity);
			Person2 p = personGen.GetComponent<Person2>();
			p.SetRoom (this);
			occupants.Add (p);
			for (int i=0; i<UnityEngine.Random.Range(1,3); i++){
				personGen = RandomPerson (false);
				Instantiate(personGen,spawnPosition,Quaternion.identity);
				p = personGen.GetComponent<Person2>();
				p.SetRoom (this);
				occupants.Add (p);
			}
		}
	}

	// Called at the end of the night
	public void CheckOut() {
		foreach (Person2 p in occupants){
			if (p!=null)
				p.isLeaving=true;
		}
		occupants.Clear ();
	}
}
