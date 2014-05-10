using UnityEngine;
using System.Collections;

public class Ability_Claw : Ability {

	public Ability_Claw () {
		FearDamage=5;
		Name="Phantom Claw";
		Description = "A hand appears and reaches out and pulls in the closest victim.";
		Duration = 8f;
		minFear=30;
		useCost=10;
		cooldownStart = 9f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Claw");
		effectSound=Resources.Load<AudioClip>("Sounds/reaching_claw");
		base.Start ();
	}
}
