﻿using UnityEngine;
using System.Collections;

public class Person2_Adult : Person2 {
	
	public Person2_Adult(){
	//	fearDropMax=2;
		moneyDropMax=2;
		admireCooldownMax=30f;
		admireCooldownMin=18f;
		sanityMax=40;
		baseSpeedMin=15f;
		baseSpeedMax=18f;
	}

	protected override void Start(){
		screamSound=Resources.Load<AudioClip>("Sounds/scream_adult_male_4");
		base.Start ();
	}
}