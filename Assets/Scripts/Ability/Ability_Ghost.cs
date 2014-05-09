using UnityEngine;
using System.Collections;

public class Ability_Ghost : Ability {

	public Ability_Ghost () {
		FearDamage=GameVars.damage_spiders;
		Name="Ghost";
		Description = "A spooky ghost appears and scares people.";
		//MinLevel=1;
		Duration = GameVars.duration_spiders;
		minFearCost=0;
		cooldownStart = 3f;
	}
	
	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Ghost");
		effectSound=Resources.Load<AudioClip>("Sounds/PLACEHOLDER-spidersound");
		base.Start ();
	}
}
