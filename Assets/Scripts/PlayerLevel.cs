using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Manages energy usage and player level.
public class PlayerLevel : MonoBehaviour {
	
	/*======== VARIABLES ========*/
	private int currentlevel=1,maxlevel=1,
	buyPoints=2,
	// when energy is below minimum energy, it regenerates
	energyMax=10,
	energyMin=2;
	private int energyCurrent;
	private int expCurrent;
	private int expToNextLevel, expToCurrentLevel;
	private Game game;
	public Font statusFont;
	public GUIStyle guistyle;
	private float decayTimer=0f,decayTimerMax;


	// when energy is below minimum energy, it regenerate
	// By convention, timers start at zero and increment to max, then resets back to zero
	public float energyRegenTimer=0f; // timer, in seconds
	private float energyRegenTimerMax=1f; // time, in seconds, to regen 1 energy
	
	// "Read-only" variables
	public int Level {
		get {return currentlevel;}
	}
	public int BuyPoints {
		get {return buyPoints;}
	}
	public int EnergyCurrent {
		get {return energyCurrent;}
	}
	
	/*======== FUNCTIONS ========*/

	void CHEAT_DEBUG(){
		if (Input.GetKeyDown ("space")){
			expCurrent=expToNextLevel;
			LevelUp ();
		} else if (expCurrent>1 && Input.GetKeyDown ("a")){
			expCurrent--;
		}
	}

	private void Start () {
		energyCurrent=energyMax;
		expToNextLevel=20;
		game=GameObject.Find("GameManager").GetComponent<Game>();
		decayTimerMax = GameVars.Difficulty; // automatically = difficulty/1
	}

	private void LevelDown(){
		currentlevel--;
		expToNextLevel=expToCurrentLevel;
		expToCurrentLevel = expToNextLevel - 15*currentlevel;
		PersonLists.GetNewCombos (currentlevel);
		game.WriteText("Your level has dropped to "+currentlevel+"!"); // also some message text is displayed
		decayTimerMax = GameVars.Difficulty/currentlevel;
	}

	private void LevelUp(){
		currentlevel++;
		expToCurrentLevel=expToNextLevel;
		PersonLists.GetNewCombos (currentlevel);
		// you haven't reached this level before
		if (currentlevel>maxlevel) { 
			maxlevel++;
			energyMax = 8+2*currentlevel; //level 1: 10, level 11: 30, etc.
			energyMin ++;
			buyPoints ++;
			if (currentlevel == 3 || currentlevel == 6 || currentlevel == 9) 
			{
				game.unlockFloor ();
			}
		}
		expCurrent+=currentlevel;//extra bonus, makes it easier to not level down
		decayTimer=0f;
		expToNextLevel = expToCurrentLevel + 15*currentlevel;
		decayTimerMax = GameVars.Difficulty/currentlevel;
		game.WriteText("Your level has increased to "+currentlevel+"!"); // also some message text is displayed
	}
	
	
	public void AddExperience(int e){
		expCurrent += e;
		if (energyCurrent+e>energyMax){
			energyCurrent=energyMax;
		} else {
			energyCurrent += e;
		}
		if (expCurrent+e>=expToNextLevel){
			LevelUp ();
		}
	}
	
	// generally this is called from using an ability
	public void UseEnergy(int e){
		energyCurrent -= e;
		if (energyCurrent<0)
			energyCurrent=0;
	}
	
	public void BuyAbility(Ability ability) {
		ability.Unlock();
		buyPoints -= ability.BuyCost;
	}
	
	public bool CanUseAbility(Ability ability) {
		if (!ability.Locked && energyCurrent<ability.EnergyCost) {
			game.WriteText ("You need "+ability.EnergyCost+" energy to use "+ability.Name+".");
			return false;
		}
		else return true;
	}
	
	private void Update() {
		CHEAT_DEBUG();
		if (currentlevel>0 && expCurrent>0){
			if (decayTimer<decayTimerMax){
				decayTimer+=GameVars.Tick*Time.deltaTime;
			} else {
				expCurrent--;
				decayTimer=0f;
			}
		}
		if (currentlevel>1 && expCurrent<expToCurrentLevel){
			LevelDown ();
		}
		// energy regeneration to bring it up to energyMin
		if (energyCurrent < energyMin) {
			if (energyRegenTimer<energyRegenTimerMax) {
				energyRegenTimer += Time.deltaTime*GameVars.Tick;
			} else {
				energyCurrent++;
				energyRegenTimer=0f;
			}
		} else if (energyCurrent < energyMax){
			// or, if it's less than max, it refills slowly
			if (energyRegenTimer<energyRegenTimerMax) {
				energyRegenTimer += 0.25f*Time.deltaTime*GameVars.Tick;
			} else {
				energyCurrent++;
				energyRegenTimer=0f;
			}
		}
	}
	
	private void OnGUI()
	{
		//ability points goes near abilities
		GUI.Box (new Rect (10, 10, Screen.width, 60), "Level " + currentlevel +
		         ", XP: "+expCurrent+"/"+expToNextLevel + 
		         "\t\t\tEnergy: " + energyCurrent + "/" + energyMax +
		         "\t\t\tPurchase Points: " + buyPoints, guistyle);
	}
}
