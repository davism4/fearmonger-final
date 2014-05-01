using UnityEngine;
using System.Collections;

public class hudText : MonoBehaviour {

	private Rect roomStringRect, moneyStringRect, timeStringRect, fearStringRect;
	private string roomCameraCenter;

	[HideInInspector] public Game2 game;

	// Use this for initialization
	private void Start () {
		if (game==null){
			game = GameObject.Find ("Main Game").GetComponent<Game2>();
		}
		timeStringRect = new Rect(0,0,70,45);
		moneyStringRect = new Rect(0,60,70,45);
		fearStringRect = new Rect(0,120,70,45);
		roomStringRect = new Rect(0,180,60,45);
		
	}

	private void OnGUI(){
		GUI.Box (timeStringRect, "Time\n"+game.DigiClock());
		GUI.Box (moneyStringRect, "Money\n$ "+game.money);
		GUI.Box (fearStringRect, "Fear\n"+game.fearEnergy);
		GUI.Box (roomStringRect, roomCameraCenter);
	}
	
	// Update is called once per frame
	private void Update () {
		int i=0;
		while (i<game.rooms.Length) {
			if (Camera.main.transform.position.y<=game.rooms[i].YCeiling)
				break;
			else
				i++;
		}
		roomCameraCenter = "Room\n" + (i+1);
	}
}
