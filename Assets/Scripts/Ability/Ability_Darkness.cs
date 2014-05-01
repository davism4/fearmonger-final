using UnityEngine;
using System.Collections;

public class Ability_Darkness : Ability {
	
	public Ability_Darkness () {
		FearDamage=GameVars.damage_darkness;
		Name="Dark Orb";
		Description = "The air turns dark and cold, and nearby lights go out.";
		//MinLevel=2;
		Duration = GameVars.duration_darkness;
		minFearCost=20;
		cooldownStart = 2f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Darkness");
		effectSound = Resources.Load<AudioClip> ("Sounds/dark_orb");
		base.Start ();
	}
}
