using UnityEngine;
using System.Collections;

public class Ability_Monster : Ability {

	// Ability #4

	public Ability_Monster () {
		FearDamage=9;
		Name="Summon Monster";
		Description = "A monster that chases people around. Explodes into fear damage on impact.";
		Duration = 15f;
		minFear = 60;
		useCost = 20;
		cooldownStart = 15f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Monster");
		effectSound=Resources.Load<AudioClip>("Sounds/ghost_giggle_3");
		base.Start ();
	}

}
