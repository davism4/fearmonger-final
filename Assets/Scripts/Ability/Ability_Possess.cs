using UnityEngine;
using System.Collections;

public class Ability_Possess : Ability {

	public Ability_Possess () {
		Name="Possession";
		Description = "Selected person becomes possessed, scaring the other people in the room.";
		//MinLevel=12;
		Duration = GameVars.duration_possession;
		minFearCost=80;
		FearDamage=GameVars.damage_possession;
		cooldownStart=30f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Possession");
		effectSound=Resources.Load<AudioClip>("Sounds/PLACEHOLDER-darksound");
		base.Start();
	}

}
