using UnityEngine;
using System.Collections;

public class hudText : MonoBehaviour {

	//private Rect roomStringRect, moneyStringRect, modeStringRect, fearStringRect, dayStringRect;
	//private Rect nightBarRect;
	private string roomCameraCenter;
	//private Texture2D nightProgressTexture;

	public Game2 game;
//	private Font mytype;
	public GUIStyle style, buttonstyle, upStyle, downStyle;
	private bool canMoveUp, canMoveDown;


	//int widthR, widthL;
	int width, height;

//	public GUISkin skin = new GUISkin();
//	public int LEFT_EDGE;
	//public TutorialSlideshow tutorial;

	// Use this for initialization
	private void Start () {
//		mytype = Resources.Load<Font>("Fonts/my_type_of_font/mytype");

//		nightProgressTexture = Resources.Load<Texture2D>("Sprites/gui/nightskyTexture");
		if (game==null){
			game = GameObject.Find ("Main Game").GetComponent<Game2>();
		}
			//if (tutorial==null){
		//	tutorial = GameObject.Find ("Main Game").GetComponent<TutorialSlideshow>();
		//}

//		LEFT_EDGE = 2*width;
		
	}

	int y;
	string str;
	private void OnGUI(){
		if (!GameVars.IsPausedTutorial){
			height = Screen.height/8;
			if (!GameVars.IsNight && (game.RoomsOpen < game.rooms.Length) && (game.CurrentRoomNumber+1==game.RoomsOpen)){
				str = "Buy floor #"+(game.RoomsOpen+1)+" for $"+game.NextRoomCost;
				buttonstyle.fontSize = Screen.height/str.Length;
				width = 15*buttonstyle.fontSize;
				if (GUI.Button(new Rect(Screen.width/2-width/2,1,width,buttonstyle.fontSize*1.5f),str,buttonstyle)){
					game.BuyRoom ();
				}
			}
		
			canMoveUp = (bool)(game.CurrentRoomNumber<Mathf.Min (game.RoomsOpen-1,game.rooms.Length-1));
			canMoveDown = (bool)(game.CurrentRoomNumber>0);

			width = 2*Screen.width/21;
			y=1;
			// Box 1: what day it is
			str = (GameVars.IsNight? "Night " : "Day ")+game.Day.ToString();
			style.fontSize = Mathf.Min(2*height/5, width/(str.Length-2));
			//dayStringRect = new Rect(1,y,width,height);
			GUI.Box (new Rect(1,y,width,height), str,style);
			y+=height+1;
			//roomStringRect = new Rect(1,y,width,height);
			str = "$"+game.money;
			style.fontSize = Mathf.Min(2*height/5, width/Mathf.Max(4,str.Length-4));
			GUI.Box (new Rect(1,y,width,height), "Money:\n"+str,style);
			//nightBarRect = new Rect(70,0,Screen.width - 70 - tutorial.Width,yheight);
			y+=height+1;
			if (GameVars.IsNight){
				str = game.DigiClock();
				style.fontSize = Mathf.Min(2*height/5, width/(str.Length-3));
				GUI.Box (new Rect(1,y,width,height), "Time\n"+str,style);
				y+=height+1;
			/*	if (GUI.Button (new Rect(1,y,width,height), "Check out",buttonstyle)){
					game.StartDay ();
				}*/
				//GUI.Box (fearStringRect, "Fear\n"+game.fearEnergy);
				//	GUI.DrawTexture (nightBarRect,nightProgressTexture);
			} else{
				str = "START";
				buttonstyle.fontSize = Mathf.Min(2*height/5, width/(str.Length-2));
				if (GUI.Button (new Rect(1,y,width,height), str,buttonstyle))
					game.StartNight ();
			}
			// room navigator
			style.fontSize = Mathf.Min(2*height/5, (int)(width*0.22f));

			if (canMoveUp && GUI.Button(new Rect(Screen.width-width-1,(Screen.height-3*height)/2,
				                        width, height),"",upStyle)){
					game.MoveUp ();
			}
			if (canMoveDown && GUI.Button(new Rect(Screen.width-width-1,(Screen.height+1*height)/2,
				                        width, height),"",downStyle)){
					game.MoveDown ();
			}
			GUI.Label (new Rect(Screen.width-width-1,(Screen.height-height)/2,width-1,height),
			         roomCameraCenter, style);

			//widthR = Screen.width/11;
			//GUI.skin.font = mytype;
			//GUI.backgroundColor=new Color(1f,0f,1f,1f);
			//GUI.contentColor=Color.white;
		//	GUI.Box (moneyStringRect, "Money\n$ "+game.money,style);
		//	GUI.Box (roomStringRect, roomCameraCenter,style);
		//	GUI.Box (dayStringRect, (GameVars.IsNight? "Night " : "Day ")+game.Day.ToString(),style);

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
		roomCameraCenter = "Floor\n" + (i+1);
	}
}
