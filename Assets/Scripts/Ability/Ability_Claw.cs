﻿using UnityEngine;
using System.Collections;

public class Ability_Claw : Ability {

	// Ability #3

	public Ability_Claw () {
		FearDamage=8;
		Name="Claw";
		Description = "A hand appears and reaches out and pulls in the nearest victim.";
		Duration = 5f;
		minFear=40;
		useCost=10;
		cooldownStart = 15f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Claw");
		effectSound=Resources.Load<AudioClip>("Sounds/reaching_claw");
		base.Start ();
	}
}
