using UnityEngine;
using System.Collections;

public class Padlock : MonoBehaviour {

//	Game2 game;
	Room[] rooms;

	// Called manually
	public void Initialize(Room[] r){
	//	game = GameObject.Find ("Main Game").GetComponent<Game2>();
		rooms = r;
//		Debug.Log ("Padlock: Start with "+game.RoomsOpen+" open rooms.");
		for (int i=0; i<rooms.Length; i++){
			if (!rooms[i].open){
				transform.position = new Vector3(transform.position.x,rooms[i].transform.position.y,
				                                 transform.position.z);
				return;
			}
		}

	}

	public void MoveToFloor(int floorno){
		if (floorno >= rooms.Length){
			//enabled=false;
			//Delete ();
			//Destroy (this);
		} else {
			transform.position = new Vector3(transform.position.x,
			            rooms[floorno].transform.position.y,
			            transform.position.z);
		}
	}

	public void Delete(){
		GetComponent<SpriteRenderer>().enabled=false;
		GetComponent<BoxCollider2D>().enabled=false;
		Destroy (gameObject);
	}
}
