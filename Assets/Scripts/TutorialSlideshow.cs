using UnityEngine;
using System.Collections;

public class TutorialSlideshow : MonoBehaviour {

	//Texture2D[] NightSlides;
	//Texture2D[] DaySlides;
	int DayslideIndex=0;
	int NightslideIndex = 0;
	string[] NightSlides = new string[4]; // use as many as necessary
	string[] DaySlides = new string[6]; // use as many as necessary


	int height, width;

	// Use this to format the text boxes. Make sure to select Word Wrap.
	public GUIStyle style, mainstyle;
//	private Font mytype;

	void Start () {
//		mytype = Resources.Load<Font>("Fonts/my_type_of_font/mytype");

		DaySlides[0] =
			"HOW TO PLAY (DAYTIME)\n" +
			"You are the owner of a haunted " +
			"hotel. Your goal is to generate money and fear, by luring in the wealthiest " +
				"visitors, and then giving them a scare!\n" +
				"During the daytime when nobody is around, you will set up furniture and traps " +
				"in the hotel rooms. You can also purchase new floors to use for your hotel. " +
				"\n\nTry to unlock all of the floors!";
		DaySlides[1] =
			"Each floor has 5 spaces where you can place furniture. To place furniture, select " +
			"a type of furniture from the buttons at the bottom of the screen. Some furniture is for " +
			"making a room more valuable. More expensive pieces of furniture equal more value for " +
				"that room. Other furniture pieces are traps, that you can click on at night to activate.";
		DaySlides[2] =
			"Some pieces of furniture are special and activate when clicked on:\n\n" +
			"Lamp: Click on it to toggle the light on or off. When a person clicks this automatically, "+
			"they regenerate some sanity.\n\n"+
			"Ornate lamp: A more valuable version of the normal lamp.\n\n" +
			"Scary lamp: It scares people when it turns on. If left off, person will attempt to " +
			"turn it on, and then scare themselves.\n\n" +
			"Armoire: Open the doors and scare anyone nearby.\n\n" +
				"Evil Portrait: Change the picture to a demon that frightens those who look at it.";
		DaySlides[3] =
			"You can spend money to buy new floors. Go up using the arrow on the right " +
			"and then click on the padlock. If you have enough money, the floor will unlock. " +
			"More floors means that more people can check in.";
		DaySlides[4] =
			"Remember, the nicer the floors are, the more likely it is to bring in " +
			"wealthy visitors, who will drop more money overnight. Here are the costs for " +
			"all of the floors:\n" +
			"Floors 1-2: Free\n" +
			"Floor 3: $2,000\n" +
			"Floor 4: $2,500\n" +
			"Floor 5: $3,000\n" +
			"Floor 6: $3,500\n" +
			"Floor 7: $4,000\n" +
			"Floor 8: $4,500\n" +
			"Floor 9: $5,000\n" +
			"Floor 10: $5,500!";
		DaySlides[5] =
			"When you are ready, press \"Check In\" to bring in visitors.";
		NightSlides[0] =
			"HOW TO PLAY (NIGHT)\n" +
			"When night starts, people check in and give you initial money. " +
			"They will also drop money at intervals during the night, based on how wealthy they are " +
				"and how nice the room is. Your furniture will also sustain damage when people use it.\n" +
				"You have from 6pm to 6am to meet your nightly goals (This is aroud 5 minutes realtime).";
		NightSlides[1] =
			"At night, your goal is to generate as much fear as possible. At the end of the " +
			"night, all of the fear you have collected will give you a huge money bonus.\n\n" +
			"Warning: Your fear is constantly decreasing, so you need to keep it up!\n\n" +
			"Be careful, though. If you scare too many people out of the hotel, then there " +
			"won't be enough people around to drop money. If the hotel is empty during the " +
			"night, then you will automatically go to daytime. A good strategy is to let" +
			"a person recover after being scared. ";
		NightSlides[2] =
			"In addition to clicking on traps you have placed, you can also spend some of " +
			"your fear to use special abilities. There are 5 abilities you can use:\n\n" +
			"GHOST: Summon a ghost where you click.\n\n" +
				"REPAIR: Fix damaged furniture and restore their durability. Click on a piece of " +
				"furniture to use this ability. It can also reset your traps. Requires 20 fear.\n\n" +
				"CLAW: Finds the nearest person and drags them back to the claw's origin. Requires 40 fear.\n\n" +
				"MONSTER: Moves around the room and terrifies anyone in its way. Requires 60 fear.\n\n" +
				"POSSESSION: Turn a person into your own puppet and scare their roommates. " +
				"Click on a person to use this ability. Requires 80 fear.";
		NightSlides[3] =
			"There are two residents to look out for: The thug, and the priest. " +
			"The thug will actively try to destroy your furniture with his club, and the " +
			"priest will try to destroy your scary traps. It is a wise idea to scare them " +
			"out of the room as quickly as possible, although they will check back in " +
				"after some time.";
	}

	int y;
	string str;
	void OnGUI(){
		//GUI.skin.font = mytype;
		style.fontSize = Mathf.Min(2*height/5, width/4);
		height = Screen.height/8;
		width = Screen.width/11;
		if (!GameVars.IsPausedTutorial) {
			if (GUI.Button (new Rect (Screen.width - width-1, 1, width-1, height), "HELP?",style)){
				GameVars.IsPausedTutorial=true;
				Time.timeScale=0.0f;
			} else {
				NightslideIndex=0;
				DayslideIndex=0;
			}
		} else {
			y=1;
			if (GUI.Button (new Rect(Screen.width-width-1,1,width-1,height), "Close",style)){
				GameVars.IsPausedTutorial=false;
				Time.timeScale=1.0f;
			}
			y+=height+1;
			if (GameVars.IsNight){
				if (NightslideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Back",style)){
						NightslideIndex--;
					} else {
						GUI.Box (new Rect (Screen.width - width-1, y, width-1, height), "",style);
					}
				}
				y+=height+1;
				if (NightslideIndex<NightSlides.Length-1){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Next",style)){
						NightslideIndex++;
					}
				}
				str = NightSlides[NightslideIndex];
			} else {
				if (DayslideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Back",style)){
						DayslideIndex--;
					}
				}
				y+=height+1;
				if (DayslideIndex<DaySlides.Length-1){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Next",style)){
						DayslideIndex++;
					}
				}
				str = DaySlides[DayslideIndex];
			}
	//		Debug.Log(Screen.height - str.Length/2);
			mainstyle.fontSize = Screen.height/20;
			GUI.Box (new Rect(width, 1, Screen.width - 2*width-2, Screen.height - height),str,mainstyle);
		}
	}

	/*void Update(){
		if (GameVars.IsPausedTutorial){

		} else {
			
		}
	}*/
}
