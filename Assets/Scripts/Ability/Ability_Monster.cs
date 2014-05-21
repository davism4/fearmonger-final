using UnityEngine;
using System.Collections;

public class Ability_Monster : Ability {

	// Ability #4

	public Ability_Monster () {
		FearDamage=9;
		Name="Monster";
		Description = "Chases people around the room.";
		Duration = 15f;
		minFear = 60;
		useCost = 15;
		cooldownStart = 15f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Monster");
		effectSound=Resources.Load<AudioClip>("Sounds/ghost_giggle_3");
		base.Start ();
	}

}
