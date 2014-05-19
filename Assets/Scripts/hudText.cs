using UnityEngine;
using System.Collections;

public class hudText : MonoBehaviour {

	private Rect roomStringRect, moneyStringRect, modeStringRect, fearStringRect, dayStringRect;
	//private Rect nightBarRect;
	private string roomCameraCenter;
	//private Texture2D nightProgressTexture;

	public Game2 game;
	private Font mytype;
	public GUIStyle style, buttonstyle;
//	public GUISkin skin = new GUISkin();
//	public int LEFT_EDGE;
	//public TutorialSlideshow tutorial;

	// Use this for initialization
	private void Start () {
		mytype = Resources.Load<Font>("Fonts/my_type_of_font/mytype");
		int yheight=1;
		int width = 90;
//		nightProgressTexture = Resources.Load<Texture2D>("Sprites/gui/nightskyTexture");
		if (game==null){
			game = GameObject.Find ("Main Game").GetComponent<Game2>();
		}
			//if (tutorial==null){
		//	tutorial = GameObject.Find ("Main Game").GetComponent<TutorialSlideshow>();
		//}
		dayStringRect = new Rect(1,yheight,width,40);
		yheight+=41;
		roomStringRect = new Rect(1,yheight,width,40);
		//nightBarRect = new Rect(70,0,Screen.width - 70 - tutorial.Width,yheight);
		yheight+=41;
		moneyStringRect = new Rect(1,yheight,width,40);
		yheight+=41;
		fearStringRect = new Rect(1,yheight,width,40);
		yheight+=41;
		modeStringRect = new Rect(1,yheight,width,40);
//		LEFT_EDGE = 2*width;
		
	}

	private void OnGUI(){
		GUI.skin.font = mytype;
		GUI.backgroundColor=new Color(1f,0f,1f,1f);
		GUI.contentColor=Color.white;
		GUI.Box (moneyStringRect, "Money\n$ "+game.money,style);
		GUI.Box (roomStringRect, roomCameraCenter,style);
		GUI.Box (dayStringRect, (GameVars.IsNight? "Night " : "Day ")+game.Day.ToString(),style);
		if (GameVars.IsNight){
			GUI.Box (fearStringRect, "Time\n"+game.DigiClock(),style);
			if (GUI.Button (modeStringRect, "Check out",buttonstyle)){
				game.StartDay ();
			}
			//GUI.Box (fearStringRect, "Fear\n"+game.fearEnergy);
		//	GUI.DrawTexture (nightBarRect,nightProgressTexture);
		} else if (GUI.Button (modeStringRect, "Check In",buttonstyle)) {
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
