using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialSlideshow : MonoBehaviour {

	//Texture2D[] NightSlides;
	//Texture2D[] DaySlides;
	int DayslideIndex=0;
	int NightslideIndex = 0;
	List<string> NightSlides = new List<string>();
	List<string> DaySlides = new List<string>();
//	string[] NightSlides = new string[9]; // use as many as necessary
//	string[] DaySlides = new string[6]; // use as many as necessary
	Ability_Claw ability3;
	Ability_Ghost ability1;
	Ability_Monster ability4;
	Ability_Possess ability5;
	Ability_Repair ability2;

	int height, width;

	// Use this to format the text boxes. Make sure to select Word Wrap.
	public GUIStyle style, mainstyle;
//	private Font mytype;

	void Start () {
//		mytype = Resources.Load<Font>("Fonts/my_type_of_font/mytype");
		ability1 = transform.GetComponent<Ability_Ghost>();
		ability2 = transform.GetComponent<Ability_Repair>();
		ability3 = transform.GetComponent<Ability_Claw>();
		ability4 = transform.GetComponent<Ability_Monster>();
		ability5 = transform.GetComponent<Ability_Possess>();
		DaySlides.Add (
			"HOW TO PLAY (DAYTIME)\n" +
			"You are the owner of a haunted " +
			"hotel. Your goal is to generate money and fear, by luring in the wealthiest " +
				"visitors, and then giving them a scare!\n" +
				"During the daytime when nobody is around, you will set up furniture and traps " +
				"in the hotel rooms. You can also purchase new floors to use for your hotel. " +
				"\n\nHow many floors can YOU buy?"
			);
		DaySlides.Add (
			"Each floor has 5 spaces where you can place furniture. To place furniture, select " +
			"a type of furniture from the buttons at the bottom of the screen. You can also sell furniture by " +
				"clicking on it, although you get less money back if it is damaged."
			);
		DaySlides.Add (
			"You can navigate up and down floors by clicking on the up/down triangles on the right " +
			"side of the screen. To buy a new floor, go to your top floor and click on the button that " +
			"appears at the top of the screen. If you have enough money, your hotel grows by another floor."
			);
		DaySlides.Add (
			"Some furniture is for " +
			"making a room more valuable. More expensive pieces of furniture equal more value for " +
				"that room. The bed and chairs are normal furniture."
			);
		DaySlides.Add (
			"Lamps are a special type of furniture that generate light and can be turned on or off. When " +
			"a person touches a lamp that is turned off, they will turn it on. The normal and fancy lamps " +
			"heal people's sanity when they turn it on. The scary lamp, on the other hand, scares them when " +
			"they turn it on. You can also manually toggle lamps by clicking on them."
			);
		DaySlides.Add (
			"ther furniture pieces are traps, that you can click on at night to activate. When people are " +
			"in front of the Armoire or Portrait, click on the furniture to activate it like a trap. Any one " +
			"standing nearby will get scared. Once you activate a trap, you cannot use it again until the next " +
			"night or until you repair it."
			);
		DaySlides.Add (
			"Remember, the nicer the floors are, the more likely it is to bring in " +
			"wealthy visitors, who will drop more money overnight.\n\n" +
			"Once you have placed some furniture in the hotel, press \"START\" to bring in visitors."
			);
		NightSlides.Add (
			"HOW TO PLAY (NIGHT)\n" +
			"When night starts, people check in and give you initial money. " +
			"They will also drop money at intervals during the night, based on how wealthy they are " +
				"and how nice the room is. Your furniture will also sustain damage when people use it.\n" +
				"You have from 6pm to 6am to meet your nightly goals (This is aroud 5 minutes realtime), " +
				"but you can also quit manually by clicking the \"Finish\" button."
			);
		NightSlides.Add (
			"At night, generate as much fear and money as possible. People will drop money throughout the " +
			"night based on how nice their room is. At the end of the " +
			"night, all of the fear you have collected will give you a huge money bonus.\n\n" +
			"Warning: Your fear is constantly decreasing, so you need to keep it up!\n\n" +
			"Be careful, though. If you scare too many people out of the hotel, then there " +
			"won't be enough people around to drop money. If the hotel is empty during the " +
			"night, then you will automatically go to daytime."
			);
		NightSlides.Add ( 
			"The thug will actively try to destroy your furniture with his club, and the " +
			"priest will try to destroy your scary traps. It is a wise idea to scare them " +
			"out of the room as quickly as possible, although they will check back in " +
			"after some time."
			);
		NightSlides.Add (
			"In addition to clicking on traps you have placed, you can also spend some of " +
			"your fear to use special abilities. There are 5 abilities you can use: Ghost, " +
			"Repair, Claw, Monster, and Possession. Each does some damage to people who " +
			"touch it. An ability requires a minimum amount of fear and " +
			"energy cost to use, and has a cooldown on use. By using the ability, you pay its " +
			"energy cost from your current fear level."
			);
		NightSlides.Add (
			"GHOST ability: Summon a ghost where you click.\n"+
			"Minimum fear level: "+ability1.MinFear+"%\n" +
			"Cooldown time: "+ability1.TimeCooldown+" seconds\n" +
			"Damage: "+ability1.Damage+"\n"+
			"Duration: "+ability1.TimeDuration+" seconds\n" +
			"Cost per use: free"
			);
		NightSlides.Add (
			"REPAIR ability: Fix damaged furniture and restore their durability by up to 30%. Click on a piece of " +
			"furniture to use this ability. It can also reset your traps.\n" +
			"Minimum fear level: "+ability2.MinFear+"%\n" +
			"Cooldown time: "+ability2.TimeCooldown+" seconds\n" +
			"Damage: "+ability2.Damage+"\n"+
			"Duration: "+ability2.TimeDuration+" seconds\n" +
			"Cost per use: -"+ability2.UseCost+"% fear"
			);
		NightSlides.Add (
			"CLAW ability: Finds the nearest person and drags them back to the claw's origin.\n" +
			"Minimum fear level: "+ability3.MinFear+"%\n" +
			"Cooldown time: "+ability3.TimeCooldown+" seconds\n" +
			"Damage: "+ability3.Damage+"\n"+
			"Duration: "+ability3.TimeDuration+" seconds\n" +
			"Cost per use: -"+ability3.UseCost+"% fear"
			);
		NightSlides.Add (
			"MONSTER ability: Moves around the room and terrifies anyone in its way.\n" +
			"Minimum fear level: "+ability4.MinFear+"%\n" +
			"Cooldown time: "+ability4.TimeCooldown+" seconds\n" +
			"Damage: "+ability4.Damage+"\n"+
			"Duration: "+ability4.TimeDuration+" seconds\n" +
			"Cost per use: -"+ability4.UseCost+"% fear"
			);
		NightSlides.Add (
			"POSSESSION ability: Take over a person's soul and fly them around like a puppet.\n" +
			"Minimum fear level: "+ability5.MinFear+"%\n" +
			"Cooldown time: "+ability5.TimeCooldown+" seconds\n" +
			"Damage: "+ability5.Damage+"\n"+
			"Duration: "+ability5.TimeDuration+" seconds\n" +
			"Cost per use: -"+ability5.UseCost+"% fear"
			);
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
					} 
				} else {
					GUI.Box (new Rect (Screen.width - width-1, y, width-1, height), "",style);
				}
				y+=height+1;
				if (NightslideIndex<NightSlides.Count-1){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Next",style)){
						NightslideIndex++;
					}
				} else {
					GUI.Box (new Rect (Screen.width - width-1, y, width-1, height), "",style);
				}
				str = NightSlides[NightslideIndex];
			} else {
				if (DayslideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Back",style)){
						DayslideIndex--;
					}
				} else {
					GUI.Box (new Rect (Screen.width - width-1, y, width-1, height), "",style);
				}
				y+=height+1;
				if (DayslideIndex<DaySlides.Count-1){
					if (GUI.Button (new Rect (Screen.width - width-1, y, width-1, height), "Next",style)){
						DayslideIndex++;
					}
				} else {
					GUI.Box (new Rect (Screen.width - width-1, y, width-1, height), "",style);
				}
				str = DaySlides[DayslideIndex];
			}
	//		Debug.Log(Screen.height - str.Length/2);
			mainstyle.fontSize = Screen.height/22;
			GUI.Box (new Rect(width, 1, Screen.width - 2*width-2, Screen.height - height),str,mainstyle);
		}
	}

	/*void Update(){
		if (GameVars.IsPausedTutorial){

		} else {
			
		}
	}*/
}
