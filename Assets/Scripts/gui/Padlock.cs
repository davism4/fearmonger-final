using UnityEngine;
using System.Collections;

public class Padlock : MonoBehaviour {

	Game2 game;
	Room[] rooms;

	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Main Game").GetComponent<Game2>();

	}

	public void Initialize(){
		rooms = game.rooms;
		Debug.Log (game.RoomsOpen);
		for (int i=0; i<rooms.Length; i++){
			if (!rooms[i].open){
				transform.position = new Vector3(transform.position.x,rooms[i].transform.position.y,
				                                 transform.position.z);
				return;
			}
		}

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void MoveToFloor(int floorno){
	}
}
