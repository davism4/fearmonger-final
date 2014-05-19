using UnityEngine;
using System.Collections;

public class TutorialSlideshow : MonoBehaviour {

	//Texture2D[] NightSlides;
	//Texture2D[] DaySlides;
	int DayslideIndex=0;
	int NightslideIndex = 0;
	string[] NightSlides = new string[4]; // use as many as necessary
	string[] DaySlides = new string[6]; // use as many as necessary
	int height=40, width=50;
	int slideleft = 80;
	int slidetop = 20;
	int slideright, slidebottom;
	int bottomMargin = 20;

	// Use this to format the text boxes. Make sure to select Word Wrap.
	public GUIStyle style;

	Rect closeButtonRect, mainRect;

	void Start () {

//		slideleft = GameObject.FindObjectOfType<hudText>().LEFT_EDGE;
		slidebottom = Screen.height - bottomMargin;
		slideright = Screen.width - width;
		closeButtonRect = new Rect (Screen.width - width, 0, width, height);
		mainRect = new Rect(slideleft, 10, Screen.width - width - slideleft - 10, slidebottom - 10);

		DaySlides[0] =
			"How to Play: Daytime\n\nYou are the manager of a hotel...";
		DaySlides[1] =
			"Each floor has 5 spaces where you can place furniture. To place furniture, select\n" +
			"a type of furniture from the buttons at the bottom of the screen. Some furniture is for\n" +
			"making a room more valuable. More expensive pieces of furniture equal more value for\n" +
				"that room. Other furniture pieces are traps, that you can click on at night to activate.";
		DaySlides[2] =
			"Some pieces of furniture are special and activate when clicked on:\n" +
			"Lamp: Click on it to toggle the light on or off.\n" +
			"Ornate lamp: A more valuable version of the normal lamp.\n" +
			"Scary lamp: It scares people when it turns on. If left off, person will attempt to \n" +
			"turn it on, and then scare themselves.\n" +
			"Armoire: Open the doors and scare anyone nearby.\n" +
			"Evil Portrait: Change the picture to a demon that frightens those who look at it.\n" +
				"...";
		DaySlides[3] =
			"You can spend money to buy new floors. Go up using the arrow on the right\n" +
			"and then click on the padlock. If you have enough money, the floor will unlock.\n" +
			"More floors means that more people can check in.";
		DaySlides[4] =
			"Remember, the nicer the floors are, the more likely it is to bring in\n" +
			"wealthy visitors, who will drop more money overnight.";
		DaySlides[5] =
			"When you are ready, press \"Check In\" to bring in visitors.";
		NightSlides[0] =
			"When night starts, people check in and give you initial money.\n" +
			"They will also drop money at intervals during the night, based on how wealthy they are\n" +
				"and how nice the room is. Your furniture will also sustain damage when people use it";
		NightSlides[1] =
			"At nighttime, your goal is to generate as much fear as possible...\n" +
			"Be careful, though. If you scare too many people out of the hotel, then there\n" +
			"won't be enough people around to drop money. If the hotel is empty during the\n" +
			"night, then you will automatically go to daytime. A good strategy is to let\n" +
			"a person recover after being scared. ";
		NightSlides[2] =
			"There are 5 abilities you can use:\n" +
			"Ghost: summon a ghost where you click.\n" +
				"Repair: fix damaged furniture and restore their durability\n" +
				"Claw: finds the nearest person and drags them back to the claw's origin\n" +
				"Monster: moves around the room and terrifies anyone in its way\n" +
				"Possession: turn a person into your own weapon of fear";
		NightSlides[3] =
			"There are two residents to look out for: The thug, and the priest.\n" +
			"The thug will actively try to destroy your furniture with his club, and the\n" +
			"priest will try to destroy your scary traps. It is a wise idea to scare them\n" +
			"out of the room as quickly as possible, although they will check back in\n" +
				"after some time.";
	}

	void OnGUI(){
		if (GameVars.IsPausedTutorial){
			if (GUI.Button (closeButtonRect, "Close"/*,style*/)){
				GameVars.IsPausedTutorial=false;
				Time.timeScale=1.0f;
			} 
			if (GameVars.IsNight){
				if (NightslideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width, height, width, height), "Back"/*,style*/)){
						NightslideIndex--;
					}
				}
				if (NightslideIndex<NightSlides.Length-1){
					if (GUI.Button (new Rect (Screen.width - width, 2*height, width, height), "Next"/*,style*/)){
						NightslideIndex++;
					}
				}
				GUI.Box (mainRect,NightSlides[NightslideIndex]/*,style*/);
			} else {
				if (DayslideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width, height, width, height), "Back"/*,style*/)){
						DayslideIndex--;
					}
				}
				if (DayslideIndex<DaySlides.Length-1){
					if (GUI.Button (new Rect (Screen.width - width, 2*height, width, height), "Next"/*,style*/)){
						DayslideIndex++;
					}
				}
				GUI.Box (mainRect,DaySlides[DayslideIndex]/*,style*/);
			}
		} else {
			if (GUI.Button (new Rect (Screen.width - width, 0, width, height), "Help"/*,style*/)){
				GameVars.IsPausedTutorial=true;
				Time.timeScale=0.0f;
			}
		}
	}

	void Update(){
		if (GameVars.IsPausedTutorial){

		} else {
			
		}
	}
}
