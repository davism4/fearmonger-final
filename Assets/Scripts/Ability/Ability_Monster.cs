using UnityEngine;
using System.Collections;

public class Ability_Monster : Ability {

	public Ability_Monster () {
		FearDamage=7;
		Name="Summon Monster";
		Description = "A monster that chases people around. Explodes into fear damage on impact.";
		Duration = 8f;
		minFear=40;
		useCost = 13;
		cooldownStart = 15f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Monster");
		effectSound=Resources.Load<AudioClip>("Sounds/ghost_giggle_3");
		base.Start ();
	}

}
