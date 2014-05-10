using UnityEngine;
using System.Collections;

public class TutorialSlideshow : MonoBehaviour {

	Texture2D[] NightSlides;
	Texture2D[] DaySlides;
	int slideIndex=0;
	int height=20, width=50;

	void Start () {
		NightSlides = new Texture2D[5];//Resources.LoadAll<Texture2D>("Sprites/Help/Night");
		DaySlides = Resources.LoadAll<Texture2D>("Sprites/Help/Day");
	}

	void OnGUI(){
		if (GameVars.IsPausedTutorial){
			if (GUI.Button (new Rect (Screen.width - width, 0, width, height), "Close")){
				GameVars.IsPausedTutorial=false;
				Time.timeScale=1.0f;
			} 
			if (GameVars.IsNight){
				if (slideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width, height, width, height), "Back")){
						slideIndex--;
					}
				}
				if (slideIndex<NightSlides.Length-1){
					if (GUI.Button (new Rect (Screen.width - width, 2*height, width, height), "Next")){
						slideIndex++;
					}
				}
			} else {
				if (slideIndex>0){
					if (GUI.Button (new Rect (Screen.width - width, height, width, height), "Back")){
						slideIndex--;
					}
				}
				if (slideIndex<DaySlides.Length-1){
					if (GUI.Button (new Rect (Screen.width - width, 2*height, width, height), "Next")){
						slideIndex++;
					}
				}
			}
		} else {
			if (GUI.Button (new Rect (Screen.width - width, 0, width, height), "Help")){
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
