using UnityEngine;
using System.Collections;

public class Ability_Ghost : Ability {

	// Ability #1

	public Ability_Ghost () {
		cooldownStart = 5f;
		FearDamage=5;
		Name="Ghost";
		Description = "A spooky ghost appears and scares people.";
		Duration = 1f;
		minFear=0;
		useCost=0;
	}
	
	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Ghost");
		effectSound=Resources.Load<AudioClip>("Sounds/Ghost 02");
		base.Start ();
	}
}
