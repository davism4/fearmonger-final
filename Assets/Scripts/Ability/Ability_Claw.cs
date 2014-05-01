using UnityEngine;
using System.Collections;

public class Ability_Claw : Ability {

	public Ability_Claw () {
		FearDamage=GameVars.damage_claw;
		Name="Reaching Claw";
		Description = "A hand appears and reaches out, pulling the closest victim toward its origin.";
		//MinLevel=6;
		Duration = GameVars.duration_claw; // instant
		//EnergyCost = 6;
		//BuyCost = 30;
		cooldownStart = 5f;
		minFearCost=40;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Claw");
		effectSound=Resources.Load<AudioClip>("Sounds/reaching_claw");
		base.Start ();
	}
}
