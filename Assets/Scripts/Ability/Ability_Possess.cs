using UnityEngine;
using System.Collections;

public class Ability_Possess : Ability {

	// Ability #5

	public Ability_Possess () {
		Name="Possession";
		FearDamage = 5;
		Description = "Selected person becomes possessed, scaring the other people in the room.";
		Duration = 20f;
		minFear = 80;
		useCost = 20;
		cooldownStart=25f;
	}

	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Possession");
		effectSound=Resources.Load<AudioClip>("Sounds/PLACEHOLDER-darksound");
		base.Start();
	}

	public override void UseAbility (RaycastHit2D hit)
	{
		if (hit.collider.CompareTag ("Person")){
			base.UseAbility (hit);
			hazardInstance.GetComponent<Hazard_Possess>().SetVictim(hit.collider.gameObject);
		}
	}
}
