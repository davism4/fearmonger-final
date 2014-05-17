using UnityEngine;
using System.Collections;

public class hudText : MonoBehaviour {

	private Rect roomStringRect, moneyStringRect, timeStringRect, fearStringRect, dayStringRect;
	private string roomCameraCenter;

	[HideInInspector] public Game2 game;

	// Use this for initialization
	private void Start () {
		int yheight=0;
		if (game==null){
			game = GameObject.Find ("Main Game").GetComponent<Game2>();
		}
		dayStringRect = new Rect(0,yheight,70,25);
		yheight+=25;
		timeStringRect = new Rect(0,yheight,70,40);
		yheight+=40;
		moneyStringRect = new Rect(0,yheight,70,40);
		yheight+=40;
		fearStringRect = new Rect(0,yheight,70,40);
		yheight+=40;
		roomStringRect = new Rect(0,yheight,60,25);
		
	}

	private void OnGUI(){
		GUI.Box (moneyStringRect, "Money\n$ "+game.money);
		GUI.Box (roomStringRect, roomCameraCenter);
		GUI.Box (dayStringRect, (GameVars.IsNight? "Night " : "Day ")+game.Day.ToString());
		if (GameVars.IsNight){
			GUI.Box (timeStringRect, "Time\n"+game.DigiClock());
			GUI.Box (fearStringRect, "Fear\n"+game.fearEnergy);
		} else if (GUI.Button (timeStringRect, "Check In")) {
			game.StartNight ();
		}

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
		roomCameraCenter = "Room " + (i+1);
	}
}
