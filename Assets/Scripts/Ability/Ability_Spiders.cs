using UnityEngine;
using System.Collections;

public class Ability_Spiders : Ability {

	public Ability_Spiders () {
		FearDamage=GameVars.damage_spiders;
		Name="Spiders";
		Description = "Spiders come out of the ground and walk around the room.";
		//MinLevel=1;
		Duration = GameVars.duration_spiders;
		minFearCost=0;
		//HazardTransform=Resources.Load<GameObject>("Sprites/Abilities/SpiderParticles").transform;
		cooldownStart = 3f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Spiders");
		effectSound=Resources.Load<AudioClip>("Sounds/PLACEHOLDER-spidersound");
		base.Start ();
	}
	
	/*public override void UseAbility(Game game, Vector2 clickLocation){
		//Transform particles = Resources.Load<GameObject>("Sprites/Abilities/SpiderParticles").transform;
	//	Vector3 clickLocation3d = new Vector3(clickLocation.x, clickLocation.y, GameVars.DepthPeopleHazards);
		//GameObject.Instantiate(particles,clickLocation3d,Quaternion.identity);
		base.UseAbility(game, clickLocation);
	}*/
}
