using UnityEngine;
using System.Collections;

public class triangledown : MonoBehaviour {

	private Game2 game;
	private SpriteRenderer sr;
	private bool canMoveDown=true;

	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Main Game").GetComponent<Game2>();
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		canMoveDown = (bool)(game.currentRoomNumber>0);
		if (canMoveDown) {
			sr.color = Color.white;
		} else {
			sr.color = new Color(0f,0f,0f,0f);
		}
	}
	
}
