using UnityEngine;
using System.Collections;

public class Ability_Monster : Ability {

	public Ability_Monster () {
		FearDamage=GameVars.damage_monster;
		Name="Summon Monster";
		Description = "A monster that chases people around. Explodes into fear damage on impact.";
		//MinLevel=5;
		Duration = GameVars.duration_monster;
		minFearCost=60;
		cooldownStart = 15f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Monster");
		effectSound=Resources.Load<AudioClip>("Sounds/ghost_giggle_3");
		base.Start ();
	}
	
	public override void UseAbility(Game game, Vector2 clickLocation){
		base.UseAbility(game, clickLocation);
	}
}
