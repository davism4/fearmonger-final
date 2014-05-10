using UnityEngine;
using System.Collections;

public class triangleup : MonoBehaviour {
	
	private Game2 game;
	private SpriteRenderer sp;
	private bool canMoveUp=true;
	
	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Main Game").GetComponent<Game2>();
		sp = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		canMoveUp = (bool)(game.currentRoomNumber<game.rooms.Length-1);
		if (canMoveUp) {
			sp.color = Color.white;
		} else {
			sp.color = new Color(0f,0f,0f,0f);
		}
	}
}
