using UnityEngine;
using System.Collections;

public class Person_Candle : Person {

	private Light lightSource;
	private float lightTimer=0f, lightTimerMax=1.5f; // how long to turn the light back on
	private bool isLightOff=false;

	// More support defense. Special ability: provides light
	protected override void Start () {
		isAdult=true;
		defenseBase=1;
		defenseSupport=3;
		sanityMax=25;
		sanityCurrent=sanityMax;
		base.Start ();
		lightSource=transform.GetComponent<Light>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		// when light is turned off, turn it back on after some delay
		if (isLightOff){
			if(lightTimer < lightTimerMax){
				lightTimer += GameVars.Tick*Time.deltaTime;
			}
			else {
				lightTimer=0f;
				isLightOff=false;
				lightSource.enabled=true;
			}
		}
	}

	// turn the light off
	public void TurnOff(){
		lightSource.enabled = false;
		isLightOff=true;
	}
}
