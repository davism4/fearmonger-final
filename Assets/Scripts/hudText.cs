using UnityEngine;
using System.Collections;

public class hudText : MonoBehaviour {

	private Rect roomStringRect, moneyStringRect, timeStringRect, fearStringRect, dayStringRect;
	//private Rect nightBarRect;
	private string roomCameraCenter;
	//private Texture2D nightProgressTexture;

	public Game2 game;
	//public TutorialSlideshow tutorial;

	// Use this for initialization
	private void Start () {
		int yheight=0;
//		nightProgressTexture = Resources.Load<Texture2D>("Sprites/gui/nightskyTexture");
		if (game==null){
			game = GameObject.Find ("Main Game").GetComponent<Game2>();
		}
		//if (tutorial==null){
		//	tutorial = GameObject.Find ("Main Game").GetComponent<TutorialSlideshow>();
		//}
		dayStringRect = new Rect(0,yheight,70,25);
		yheight+=25;
		timeStringRect = new Rect(0,yheight,70,40);
		//nightBarRect = new Rect(70,0,Screen.width - 70 - tutorial.Width,yheight);
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
		//	GUI.DrawTexture (nightBarRect,nightProgressTexture);
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
